using System.Security.Cryptography;
using Fast.Center.Entity;
using Fast.Center.Service.File.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Yitter.IdGenerator;

namespace Fast.Center.Service.File;

/// <summary>
/// <see cref="FileService"/> 文件服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "file", Order = 997)]
public class FileService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<FileModel> _repository;
    private readonly UploadFileSettingsOptions _uploadFileSettingsOptions;
    private readonly HttpContext _httpContext;

    /// <summary>
    /// 图片
    /// </summary>
    private readonly HashSet<string> Images = ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"];

    /// <summary>
    /// 视频
    /// </summary>
    private readonly HashSet<string> Videos =
        ["video/mp4", "video/mpeg", "video/quicktime", "video/x-msvideo", "video/x-ms-wmv", "video/webm", "video/ogg"];

    /// <summary>
    /// 音频
    /// </summary>
    private readonly HashSet<string> Audios = ["audio/mpeg", "audio/wav", "audio/ogg", "audio/mp4", "audio/flac"];

    /// <summary>
    /// 文本
    /// </summary>
    private readonly HashSet<string> Texts =
    [
        "text/plain",
        "text/csv",
        "text/html",
        "text/markdown"
    ];

    /// <summary>
    /// 文档
    /// </summary>
    private readonly HashSet<string> Documents =
    [
        // PDF
        "application/pdf",
        // Word
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        // Excel
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        // PowerPoint
        "application/vnd.ms-powerpoint",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation"
    ];

    /// <summary>
    /// 压缩包
    /// </summary>
    private readonly HashSet<string> Archives =
        ["application/zip", "application/x-rar-compressed", "application/x-7z-compressed", "application/gzip"];

    /// <summary>
    /// 图片尺寸
    /// </summary>
    private readonly Dictionary<string, int> ImageSizes = new()
        { { "thumb", 100 }, { "small", 300 }, { "normal", 600 } };

    public FileService(IUser user, ISqlSugarRepository<FileModel> repository,
        IOptions<UploadFileSettingsOptions> uploadFileSettingsOptions, IHttpContextAccessor httpContextAccessor)
    {
        _user = user;
        _repository = repository;
        _uploadFileSettingsOptions = uploadFileSettingsOptions.Value;
        _httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    /// 获取文件分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取文件分页列表", HttpRequestActionEnum.Paged)]
    public async Task<PagedResult<QueryFilePagedOutput>> QueryFilePaged(QueryFilePagedInput input)
    {
        var queryable = _repository.Entities;
        if (_user.IsSuperAdmin)
        {
            queryable = queryable.ClearFilter<IBaseTEntity>()
                .WhereIF(input.TenantId != null, wh => wh.TenantId == input.TenantId);
        }
        else if (!_user.IsAdmin)
        {
            queryable = queryable.Where(wh => wh.CreatedUserId == _user.UserId);
        }

        return await queryable.OrderByDescending(ob => ob.CreatedTime)
            .ToPagedListAsync(input,
                sl => new QueryFilePagedOutput
                {
                    FileId = sl.FileId,
                    FileObjectName = sl.FileObjectName,
                    FileOriginName = sl.FileOriginName,
                    FileSuffix = sl.FileSuffix,
                    FileMimeType = sl.FileMimeType,
                    FileSizeKb = sl.FileSizeKb,
                    FileLocation = sl.FileLocation,
                    UploadDevice = sl.UploadDevice,
                    UploadOS = sl.UploadOS,
                    UploadBrowser = sl.UploadBrowser,
                    UploadProvince = sl.UploadProvince,
                    UploadCity = sl.UploadCity,
                    UploadIp = sl.UploadIp,
                    CreatedUserName = sl.CreatedUserName,
                    CreatedTime = sl.CreatedTime
                });
    }

    /// <summary>
    /// 预览文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpGet("/file/{fileName}")]
    [ApiInfo("预览文件", HttpRequestActionEnum.Download)]
    [AllowAnonymous]
    public async Task<IActionResult> Preview([FromRoute, Required(ErrorMessage = "文件名称不能为空")] string fileName)
    {
        return await LocalPreview(fileName);
    }

    /// <summary>
    /// 预览文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="size">
    /// <see cref="string"/> 尺寸
    /// <para>thumb：缩略图</para>
    /// <para>small：小图</para>
    /// <para>normal：正常</para>
    /// </param>
    /// <returns></returns>
    [HttpGet("/file/{fileName}@!{size}")]
    [ApiInfo("预览文件", HttpRequestActionEnum.Download)]
    [AllowAnonymous]
    public async Task<IActionResult> Preview([FromRoute, Required(ErrorMessage = "文件名称不能为空")] string fileName,
        [FromRoute, Required(ErrorMessage = "文件大小不能为空")]
        string size)
    {
        return await LocalPreview(fileName, size);
    }

    /// <summary>
    /// 预览文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="size">
    /// <see cref="string"/> 尺寸
    /// <para>thumb：缩略图</para>
    /// <para>small：小图</para>
    /// <para>normal：正常</para>
    /// </param>
    /// <returns></returns>
    private async Task<IActionResult> LocalPreview(string fileName, string size = null)
    {
        size = string.IsNullOrWhiteSpace(size) ? "" : size.ToLower();

        // 获取文件后缀
        var fileSuffix = Path.GetExtension(fileName);
        var fileIdStr = fileName[..^fileSuffix.Length];
        if (!long.TryParse(fileIdStr, out var fileId))
            throw new UserFriendlyException("文件不存在！");

        var fileInfoModel = await _repository.SingleOrDefaultAsync(fileId);
        if (fileInfoModel == null)
            throw new UserFriendlyException("文件不存在！");

        // 判断是否为图片
        if (Images.Contains(fileInfoModel.FileMimeType.ToLower()))
        {
            // 添加缓存
            _httpContext.Response.Headers.CacheControl = "public,max-age=31536000";
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(size))
            {
                throw new UserFriendlyException("非图片类型不支持大小设置！");
            }
        }

        var localFileName = $"{fileInfoModel.FileId}@{size}.{fileInfoModel.FileSuffix}";

        var localFilePath = Path.Combine(Environment.CurrentDirectory, fileInfoModel.FilePath, localFileName);
        if (!System.IO.File.Exists(localFilePath))
            throw new UserFriendlyException("文件丢失或已被删除！");

        var stream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new FileStreamResult(stream, fileInfoModel.FileMimeType);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/download")]
    [ApiInfo("下载文件", HttpRequestActionEnum.Download)]
    [AllowAnonymous]
    public async Task<IActionResult> Download(DownloadFileInput input)
    {
        var fileInfoModel = await _repository.SingleOrDefaultAsync(input.FileId);
        if (fileInfoModel == null)
            throw new UserFriendlyException("文件不存在！");

        var filePath = Path.Combine(Environment.CurrentDirectory, fileInfoModel.FilePath, fileInfoModel.FileObjectName);
        if (!System.IO.File.Exists(filePath))
            throw new UserFriendlyException("文件丢失或已被删除！");

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new FileStreamResult(stream, fileInfoModel.FileMimeType)
            { FileDownloadName = fileInfoModel.FileOriginName };
    }

    /// <summary>
    /// 上传Logo
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/uploadLogo")]
    [ApiInfo("上传头像", HttpRequestActionEnum.Upload)]
    public async Task<string> UploadLogo(IFormFile file)
    {
        return await LocalUploadFile(file, _uploadFileSettingsOptions.Logo);
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/uploadAvatar")]
    [ApiInfo("上传头像", HttpRequestActionEnum.Upload)]
    public async Task<string> UploadAvatar(IFormFile file)
    {
        return await LocalUploadFile(file, _uploadFileSettingsOptions.Avatar);
    }

    /// <summary>
    /// 上传证件照
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/uploadIdPhoto")]
    [ApiInfo("上传证件照", HttpRequestActionEnum.Upload)]
    public async Task<string> UploadIdPhoto(IFormFile file)
    {
        return await LocalUploadFile(file, _uploadFileSettingsOptions.IdPhoto);
    }

    /// <summary>
    /// 上传富文本
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/uploadEditor")]
    [ApiInfo("上传富文本", HttpRequestActionEnum.Upload)]
    public async Task<string> UploadEditor(IFormFile file)
    {
        return await LocalUploadFile(file, _uploadFileSettingsOptions.Editor);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/uploadFile")]
    [ApiInfo("上传文件", HttpRequestActionEnum.Upload)]
    public async Task<string> UploadFile(IFormFile file)
    {
        return await LocalUploadFile(file);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file"><see cref="IFormFile"/> 文件</param>
    /// <param name="fileInfoSettings"><see cref="UploadFileInfoSettings"/> 文件配置信息</param>
    /// <returns></returns>
    private async Task<string> LocalUploadFile(IFormFile file, UploadFileInfoSettings fileInfoSettings = null)
    {
        if (file == null || file.Length == 0)
            throw new UserFriendlyException("上传文件不能为空！");

        fileInfoSettings ??= _uploadFileSettingsOptions.Default;

        var dateTime = DateTime.Now;

        // 文件大小
        var fileSizeKb = file.Length / 1024L;
        if (fileInfoSettings.MaxSize > 0 && fileSizeKb > fileInfoSettings.MaxSize)
            throw new UserFriendlyException($"文件大小超出限制，最大允许{fileInfoSettings.MaxSize / 1024}MB。");

        // 文件后缀
        var fileSuffix = Path.GetExtension(file.FileName)
            .ToLower();
        if (string.IsNullOrWhiteSpace(fileSuffix))
            throw new UserFriendlyException("文件没有有效后缀名!");

        if (fileInfoSettings.ContentType?.Any() == true)
        {
            var contentType = file.ContentType.ToLower();
            if (!fileInfoSettings.ContentType.Contains(contentType))
                throw new UserFriendlyException($"文件类型不支持，当前类型：{contentType}");
        }

        // 计算文件哈希
        using var md5 = MD5.Create();
        await using var stream = file.OpenReadStream();
        var hashBytes = await md5.ComputeHashAsync(stream);
        var fileHash = BitConverter.ToString(hashBytes)
            .Replace("-", "")
            .ToLower();

        // 判断是否存在重复文件
        var existFileModel = await _repository.SingleOrDefaultAsync(s => s.FileHash == fileHash);
        if (existFileModel != null)
            return existFileModel.FileLocation;

        var httpRequest = FastContext.HttpContext.Request;

        var fileId = YitIdHelper.NextId();
        // 本地文件名称
        var fileObjectName = $"{fileId}{fileSuffix}";

        // 本地文件路径
        var filePath = fileInfoSettings.Path;

        if (!string.IsNullOrWhiteSpace(_user?.TenantNo))
        {
            filePath = Path.Combine(filePath, _user.TenantNo);
        }

        // 判断是否启用类型文件夹
        if (fileInfoSettings.UseTypeFolder)
        {
            if (Images.Contains(file.ContentType.ToLower()))
                filePath = Path.Combine(filePath, "image");
            else if (Videos.Contains(file.ContentType.ToLower()))
                filePath = Path.Combine(filePath, "video");
            else if (Audios.Contains(file.ContentType.ToLower()))
                filePath = Path.Combine(filePath, "audio");
            else if (Texts.Contains(file.ContentType.ToLower()))
                filePath = Path.Combine(filePath, "text");
            else if (Documents.Contains(file.ContentType.ToLower()))
                filePath = Path.Combine(filePath, "document");
            else if (Archives.Contains(file.ContentType.ToLower()))
                filePath = Path.Combine(filePath, "archive");
            else
                filePath = Path.Combine(filePath, "other");
        }

        // 判断是否启用时间文件夹
        if (fileInfoSettings.UseDateFolder)
        {
            filePath = Path.Combine(filePath, dateTime.ToString("yyyy/MM/dd"));
        }

        var fileInfoModel = new FileModel
        {
            FileId = fileId,
            FileObjectName = fileObjectName,
            FileOriginName = file.FileName,
            FileSuffix = fileSuffix.TrimStart('.'),
            FileMimeType = file.ContentType,
            FileSizeKb = fileSizeKb,
            FilePath = filePath,
            FileLocation = $"{httpRequest.Scheme}://{httpRequest.Host}/{fileObjectName}",
            FileHash = fileHash
        };
        // 获取设备信息
        var userAgentInfo = FastContext.HttpContext.RequestUserAgentInfo();
        // 获取Ip信息
        var ip = FastContext.HttpContext.RemoteIpv4();
        // 获取万网信息
        var wanNetIpInfo = await FastContext.HttpContext.RemoteIpv4InfoAsync();
        fileInfoModel.UploadDevice = userAgentInfo.Device;
        fileInfoModel.UploadOS = userAgentInfo.OS;
        fileInfoModel.UploadBrowser = userAgentInfo.Browser;
        fileInfoModel.UploadProvince = wanNetIpInfo.Province;
        fileInfoModel.UploadCity = wanNetIpInfo.City;
        fileInfoModel.UploadIp = ip;
        fileInfoModel.CreatedUserId = _user?.UserId;
        fileInfoModel.CreatedUserName = _user?.NickName;
        fileInfoModel.CreatedTime = dateTime;
        await _repository.InsertAsync(fileInfoModel);

        // 本地存储
        var localFilePath = Path.Combine(Environment.CurrentDirectory, filePath);
        if (!Directory.Exists(localFilePath))
            Directory.CreateDirectory(localFilePath);
        var localFullPath = Path.Combine(localFilePath, fileObjectName);
        await using var fileStream = System.IO.File.Create(localFullPath);
        await file.CopyToAsync(fileStream);

        // 判断是否为图片
        if (Images.Contains(file.ContentType.ToLower()))
        {
            // 异步读取原始图片
            using var image = await Image.LoadAsync(localFullPath);

            foreach (var item in ImageSizes)
            {
                var width = item.Value;
                // 按原图比例计算高度
                var ratio = (float)width / image.Width;
                var height = (int)(image.Height * ratio);

                // 创建图片副本并调整大小
                using var clone = image.Clone(ctx => ctx.Resize(width, height));
                // 拼接缩略图文件名
                var newName = $"{fileId}@{item.Key}{fileSuffix}";
                var newPath = Path.Combine(localFilePath, newName);

                // 保存图片到本地，格式自动根据后缀判断
                await clone.SaveAsync(newPath);
            }
        }

        return fileInfoModel.FileLocation;
    }
}