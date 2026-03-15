// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Fast.Center.Entity;
using Fast.Core;
using Fast.DynamicApplication;
using Fast.File.Applications.Dto;
using Fast.NET.Core;
using Fast.Runtime;
using Fast.Shared;
using Fast.SqlSugar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SqlSugar;
using Yitter.IdGenerator;

namespace Fast.File.Applications;

/// <summary>
/// <see cref="FileApplication"/> 文件服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.File, Name = "fileStorage", Order = 997)]
public class FileApplication : IDynamicApplication
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
    private readonly Dictionary<string, int> ImageSizes = new() {{"thumb", 100}, {"small", 300}, {"normal", 600}};

    /// <summary>
    /// 分片上传元数据缓存
    /// </summary>
    private static readonly ConcurrentDictionary<string, ChunkUploadMetadata> ChunkUploads = new();

    /// <summary>
    /// <see cref="FileApplication"/> 文件服务
    /// </summary>
    /// <param name="user"></param>
    /// <param name="repository"></param>
    /// <param name="uploadFileSettingsOptions"></param>
    /// <param name="httpContextAccessor"></param>
    public FileApplication(IUser user, ISqlSugarRepository<FileModel> repository,
        IOptions<UploadFileSettingsOptions> uploadFileSettingsOptions, IHttpContextAccessor httpContextAccessor)
    {
        _user = user;
        _repository = repository;
        _uploadFileSettingsOptions = uploadFileSettingsOptions.Value;
        _httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    /// 预览文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    [HttpGet("/fileStorage/{fileName}")]
    [ApiInfo("预览文件", HttpRequestActionEnum.Download)]
    [AllowAnonymous, DisabledRequestLog]
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
    [ApiDescriptionSettings(false)]
    [HttpGet("/fileStorage/{fileName}@!{size}")]
    [ApiInfo("预览文件", HttpRequestActionEnum.Download)]
    [AllowAnonymous, DisabledRequestLog]
    public async Task<IActionResult> Preview([FromRoute, Required(ErrorMessage = "文件名称不能为空")] string fileName,
        [FromRoute, Required(ErrorMessage = "文件大小不能为空")] string size)
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
        if (!string.IsNullOrWhiteSpace(size))
        {
            if (!ImageSizes.ContainsKey(size))
            {
                throw new UserFriendlyException("不支持的图片尺寸！");
            }

            size = $"@{size}";
        }
        else
        {
            size = "";
        }

        // 获取文件后缀
        var fileSuffix = Path.GetExtension(fileName);
        var fileIdStr = fileName[..^fileSuffix.Length];
        if (!long.TryParse(fileIdStr, out var fileId))
            throw new UserFriendlyException("文件不存在！");

        // 这里作为预览文件，必须禁用 AOP，所以直接使用 NEW 的方式
        var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));
        var fileInfoModel = await db.Queryable<FileModel>()
            .InSingleAsync(fileId);
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

        var localFileName = $"{fileInfoModel.FileId}{size}.{fileInfoModel.FileSuffix}";

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
    [HttpPost]
    [ApiInfo("下载文件", HttpRequestActionEnum.Download)]
    public async Task<IActionResult> Download(DownloadFileInput input)
    {
        var fileInfoModel = await _repository.Entities.ClearFilter<IBaseTEntity>()
            .InSingleAsync(input.FileId);
        if (fileInfoModel == null)
            throw new UserFriendlyException("文件不存在！");

        var filePath = Path.Combine(Environment.CurrentDirectory, fileInfoModel.FilePath, fileInfoModel.FileObjectName);
        if (!System.IO.File.Exists(filePath))
            throw new UserFriendlyException("文件丢失或已被删除！");

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new FileStreamResult(stream, fileInfoModel.FileMimeType) {FileDownloadName = fileInfoModel.FileOriginName};
    }

    /// <summary>
    /// 分片下载文件（支持Range请求）
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    [HttpGet("/fileStorage/range/{fileName}")]
    [ApiInfo("分片下载文件", HttpRequestActionEnum.Download)]
    [AllowAnonymous, DisabledRequestLog]
    public async Task<IActionResult> RangeDownload(
        [FromRoute, Required(ErrorMessage = "文件名称不能为空")] string fileName)
    {
        // 防止路径遍历攻击
        if (fileName.Contains("..") || fileName.Contains('/') || fileName.Contains('\\'))
            throw new UserFriendlyException("文件名称包含非法字符！");

        var fileSuffix = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(fileSuffix))
            throw new UserFriendlyException("文件不存在！");

        var fileIdStr = Path.GetFileNameWithoutExtension(fileName);
        if (!long.TryParse(fileIdStr, out var fileId))
            throw new UserFriendlyException("文件不存在！");

        var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));
        var fileInfoModel = await db.Queryable<FileModel>()
            .InSingleAsync(fileId);
        if (fileInfoModel == null)
            throw new UserFriendlyException("文件不存在！");

        var localFilePath = Path.Combine(Environment.CurrentDirectory, fileInfoModel.FilePath,
            fileInfoModel.FileObjectName);
        if (!System.IO.File.Exists(localFilePath))
            throw new UserFriendlyException("文件丢失或已被删除！");

        var fileInfo = new FileInfo(localFilePath);
        var fileLength = fileInfo.Length;

        _httpContext.Response.Headers["Accept-Ranges"] = "bytes";
        _httpContext.Response.Headers["Content-Disposition"] =
            $"attachment; filename=\"{Uri.EscapeDataString(fileInfoModel.FileOriginName)}\"";

        var rangeHeader = _httpContext.Request.Headers["Range"].ToString();
        if (!string.IsNullOrWhiteSpace(rangeHeader) && rangeHeader.StartsWith("bytes="))
        {
            var range = rangeHeader["bytes=".Length..];
            var parts = range.Split('-');
            var start = long.Parse(parts[0]);
            var end = parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1])
                ? long.Parse(parts[1])
                : fileLength - 1;

            if (start < 0 || start >= fileLength || end >= fileLength || start > end)
            {
                _httpContext.Response.StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable;
                _httpContext.Response.Headers["Content-Range"] = $"bytes */{fileLength}";
                return new EmptyResult();
            }

            var contentLength = end - start + 1;
            _httpContext.Response.StatusCode = (int)HttpStatusCode.PartialContent;
            _httpContext.Response.Headers["Content-Range"] = $"bytes {start}-{end}/{fileLength}";
            _httpContext.Response.Headers["Content-Length"] = contentLength.ToString();

            var stream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            stream.Seek(start, SeekOrigin.Begin);
            return new FileStreamResult(stream, fileInfoModel.FileMimeType);
        }

        _httpContext.Response.Headers["Content-Length"] = fileLength.ToString();
        var fullStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new FileStreamResult(fullStream, fileInfoModel.FileMimeType)
        {
            FileDownloadName = fileInfoModel.FileOriginName
        };
    }

    /// <summary>
    /// 上传Logo
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("上传Logo", HttpRequestActionEnum.Upload)]
    public async Task<string> UploadLogo(IFormFile file)
    {
        return await LocalUploadFile(file, _uploadFileSettingsOptions.Logo);
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
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
    [HttpPost]
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
    [HttpPost]
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
    [HttpPost]
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
            FileLocation = $"{httpRequest.Scheme}://{httpRequest.Host}/fileStorage/{fileObjectName}",
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
        await using (var fileStream = System.IO.File.Create(localFullPath))
        {
            await file.CopyToAsync(fileStream);
        }

        // 判断是否为图片
        if (Images.Contains(file.ContentType.ToLower()))
        {
            // 异步读取原始图片
            using var image = await Image.LoadAsync(localFullPath);

            foreach (var item in ImageSizes)
            {
                var width = item.Value;
                // 按原图比例计算高度
                var ratio = (float) width / image.Width;
                var height = (int) (image.Height * ratio);

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

    /// <summary>
    /// 初始化分片上传
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("初始化分片上传", HttpRequestActionEnum.Upload)]
    public async Task<InitChunkUploadOutput> InitChunkUpload(InitChunkUploadInput input)
    {
        var fileInfoSettings = _uploadFileSettingsOptions.Default;

        // 分片数量上限校验
        const int maxChunksAllowed = 10000;
        if (input.TotalChunks > maxChunksAllowed)
            throw new UserFriendlyException($"分片数量超出限制，最大允许{maxChunksAllowed}个分片！");

        // 文件大小校验
        var fileSizeKb = input.FileSize / 1024L;
        if (fileInfoSettings.MaxSize > 0 && fileSizeKb > fileInfoSettings.MaxSize)
            throw new UserFriendlyException($"文件大小超出限制，最大允许{fileInfoSettings.MaxSize / 1024}MB。");

        // 文件类型校验
        if (fileInfoSettings.ContentType?.Any() == true)
        {
            var contentType = input.ContentType.ToLower();
            if (!fileInfoSettings.ContentType.Contains(contentType))
                throw new UserFriendlyException($"文件类型不支持，当前类型：{contentType}");
        }

        // 秒传：如果提供了文件哈希，检查是否已存在
        if (!string.IsNullOrWhiteSpace(input.FileHash))
        {
            var existFileModel = await _repository.SingleOrDefaultAsync(s => s.FileHash == input.FileHash);
            if (existFileModel != null)
            {
                return new InitChunkUploadOutput
                {
                    UploadId = string.Empty,
                    Uploaded = true,
                    FileLocation = existFileModel.FileLocation
                };
            }
        }

        var uploadId = Guid.NewGuid().ToString("N");
        var chunkTempPath = Path.Combine(Environment.CurrentDirectory, "Upload", "Chunks", uploadId);
        Directory.CreateDirectory(chunkTempPath);

        var metadata = new ChunkUploadMetadata
        {
            UploadId = uploadId,
            FileName = input.FileName,
            FileSize = input.FileSize,
            ChunkSize = input.ChunkSize,
            TotalChunks = input.TotalChunks,
            ContentType = input.ContentType,
            FileHash = input.FileHash,
            ChunkTempPath = chunkTempPath,
            CreatedTime = DateTime.Now
        };

        // 保存元数据到文件
        var metadataPath = Path.Combine(chunkTempPath, "metadata.json");
        var metadataJson = JsonSerializer.Serialize(metadata);
        await System.IO.File.WriteAllTextAsync(metadataPath, metadataJson);

        ChunkUploads[uploadId] = metadata;

        return new InitChunkUploadOutput
        {
            UploadId = uploadId,
            Uploaded = false,
            FileLocation = null
        };
    }

    /// <summary>
    /// 上传分片
    /// </summary>
    /// <param name="uploadId">上传标识</param>
    /// <param name="chunkIndex">分片索引（从0开始）</param>
    /// <param name="file">分片文件</param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("上传分片", HttpRequestActionEnum.Upload)]
    public async Task UploadChunk(
        [FromForm, Required(ErrorMessage = "上传标识不能为空")] string uploadId,
        [FromForm, Required(ErrorMessage = "分片索引不能为空")] int chunkIndex,
        [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new UserFriendlyException("分片文件不能为空！");

        var metadata = await GetChunkMetadata(uploadId);

        if (chunkIndex < 0 || chunkIndex >= metadata.TotalChunks)
            throw new UserFriendlyException($"分片索引无效，有效范围：0-{metadata.TotalChunks - 1}");

        // 分片大小校验（允许最后一个分片略小，其余分片不超过声明大小的110%）
        var maxAllowedChunkSize = (long)(metadata.ChunkSize * 1.1);
        if (file.Length > maxAllowedChunkSize)
            throw new UserFriendlyException($"分片文件大小超出限制！");

        var chunkFilePath = Path.Combine(metadata.ChunkTempPath, $"chunk_{chunkIndex}");
        await using (var fileStream = System.IO.File.Create(chunkFilePath))
        {
            await file.CopyToAsync(fileStream);
        }

        // 更新内存中的已上传列表
        if (!metadata.UploadedChunks.Contains(chunkIndex))
        {
            metadata.UploadedChunks.Add(chunkIndex);
        }
    }

    /// <summary>
    /// 获取分片上传进度
    /// </summary>
    /// <param name="uploadId">上传标识</param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取分片上传进度", HttpRequestActionEnum.Query)]
    public async Task<ChunkUploadProgressOutput> GetChunkUploadProgress(
        [FromQuery, Required(ErrorMessage = "上传标识不能为空")] string uploadId)
    {
        var metadata = await GetChunkMetadata(uploadId);

        // 扫描实际已上传的分片文件
        var uploadedChunks = new List<int>();
        for (var i = 0; i < metadata.TotalChunks; i++)
        {
            var chunkFilePath = Path.Combine(metadata.ChunkTempPath, $"chunk_{i}");
            if (System.IO.File.Exists(chunkFilePath))
            {
                uploadedChunks.Add(i);
            }
        }

        return new ChunkUploadProgressOutput
        {
            UploadId = uploadId,
            FileName = metadata.FileName,
            TotalChunks = metadata.TotalChunks,
            UploadedChunks = uploadedChunks,
            IsComplete = uploadedChunks.Count == metadata.TotalChunks
        };
    }

    /// <summary>
    /// 合并分片
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("合并分片", HttpRequestActionEnum.Upload)]
    public async Task<string> MergeChunk(MergeChunkInput input)
    {
        var metadata = await GetChunkMetadata(input.UploadId);

        // 校验所有分片是否已上传
        for (var i = 0; i < metadata.TotalChunks; i++)
        {
            var chunkFilePath = Path.Combine(metadata.ChunkTempPath, $"chunk_{i}");
            if (!System.IO.File.Exists(chunkFilePath))
                throw new UserFriendlyException($"分片 {i} 尚未上传！");
        }

        var fileInfoSettings = _uploadFileSettingsOptions.Default;
        var dateTime = DateTime.Now;
        var fileSuffix = Path.GetExtension(metadata.FileName).ToLower();
        if (string.IsNullOrWhiteSpace(fileSuffix))
            throw new UserFriendlyException("文件没有有效后缀名!");

        var httpRequest = FastContext.HttpContext.Request;
        var fileId = YitIdHelper.NextId();
        var fileObjectName = $"{fileId}{fileSuffix}";

        // 构建存储路径
        var filePath = fileInfoSettings.Path;
        if (!string.IsNullOrWhiteSpace(_user?.TenantNo))
        {
            filePath = Path.Combine(filePath, _user.TenantNo);
        }

        if (fileInfoSettings.UseTypeFolder)
        {
            var contentType = metadata.ContentType.ToLower();
            if (Images.Contains(contentType))
                filePath = Path.Combine(filePath, "image");
            else if (Videos.Contains(contentType))
                filePath = Path.Combine(filePath, "video");
            else if (Audios.Contains(contentType))
                filePath = Path.Combine(filePath, "audio");
            else if (Texts.Contains(contentType))
                filePath = Path.Combine(filePath, "text");
            else if (Documents.Contains(contentType))
                filePath = Path.Combine(filePath, "document");
            else if (Archives.Contains(contentType))
                filePath = Path.Combine(filePath, "archive");
            else
                filePath = Path.Combine(filePath, "other");
        }

        if (fileInfoSettings.UseDateFolder)
        {
            filePath = Path.Combine(filePath, dateTime.ToString("yyyy/MM/dd"));
        }

        var localFilePath = Path.Combine(Environment.CurrentDirectory, filePath);
        if (!Directory.Exists(localFilePath))
            Directory.CreateDirectory(localFilePath);
        var localFullPath = Path.Combine(localFilePath, fileObjectName);

        // 合并分片文件
        try
        {
            await using (var mergedStream = System.IO.File.Create(localFullPath))
            {
                for (var i = 0; i < metadata.TotalChunks; i++)
                {
                    var chunkFilePath = Path.Combine(metadata.ChunkTempPath, $"chunk_{i}");
                    await using var chunkStream = System.IO.File.OpenRead(chunkFilePath);
                    await chunkStream.CopyToAsync(mergedStream);
                }
            }
        }
        catch (FileNotFoundException)
        {
            // 清理不完整的合并文件
            if (System.IO.File.Exists(localFullPath))
                System.IO.File.Delete(localFullPath);
            CleanupChunkTempFiles(metadata);
            throw new UserFriendlyException("分片文件异常，请重新上传！");
        }

        // 计算合并后文件的哈希
        string fileHash;
        using (var md5 = MD5.Create())
        {
            await using var fileStream = System.IO.File.OpenRead(localFullPath);
            var hashBytes = await md5.ComputeHashAsync(fileStream);
            fileHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        // 秒传：再次检查是否存在重复文件
        var existFileModel = await _repository.SingleOrDefaultAsync(s => s.FileHash == fileHash);
        if (existFileModel != null)
        {
            // 删除合并后的文件
            System.IO.File.Delete(localFullPath);
            CleanupChunkTempFiles(metadata);
            return existFileModel.FileLocation;
        }

        var fileSizeKb = new FileInfo(localFullPath).Length / 1024L;
        var fileInfoModel = new FileModel
        {
            FileId = fileId,
            FileObjectName = fileObjectName,
            FileOriginName = metadata.FileName,
            FileSuffix = fileSuffix.TrimStart('.'),
            FileMimeType = metadata.ContentType,
            FileSizeKb = fileSizeKb,
            FilePath = filePath,
            FileLocation = $"{httpRequest.Scheme}://{httpRequest.Host}/fileStorage/{fileObjectName}",
            FileHash = fileHash
        };

        var userAgentInfo = FastContext.HttpContext.RequestUserAgentInfo();
        var ip = FastContext.HttpContext.RemoteIpv4();
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

        // 生成图片缩略图
        if (Images.Contains(metadata.ContentType.ToLower()))
        {
            using var image = await Image.LoadAsync(localFullPath);
            foreach (var item in ImageSizes)
            {
                var width = item.Value;
                var ratio = (float)width / image.Width;
                var height = (int)(image.Height * ratio);
                using var clone = image.Clone(ctx => ctx.Resize(width, height));
                var newName = $"{fileId}@{item.Key}{fileSuffix}";
                var newPath = Path.Combine(localFilePath, newName);
                await clone.SaveAsync(newPath);
            }
        }

        // 清理分片临时文件
        CleanupChunkTempFiles(metadata);

        return fileInfoModel.FileLocation;
    }

    /// <summary>
    /// 获取分片上传元数据
    /// </summary>
    private async Task<ChunkUploadMetadata> GetChunkMetadata(string uploadId)
    {
        if (string.IsNullOrWhiteSpace(uploadId))
            throw new UserFriendlyException("上传标识不能为空！");

        // 验证uploadId格式，防止路径遍历攻击
        if (!Guid.TryParseExact(uploadId, "N", out _))
            throw new UserFriendlyException("上传标识格式无效！");

        if (ChunkUploads.TryGetValue(uploadId, out var metadata))
            return metadata;

        // 从文件恢复元数据（应用重启后自动恢复）
        var chunkTempPath = Path.Combine(Environment.CurrentDirectory, "Upload", "Chunks", uploadId);
        var metadataPath = Path.Combine(chunkTempPath, "metadata.json");
        if (!System.IO.File.Exists(metadataPath))
            throw new UserFriendlyException("上传任务不存在或已过期！");

        var metadataJson = await System.IO.File.ReadAllTextAsync(metadataPath);
        metadata = JsonSerializer.Deserialize<ChunkUploadMetadata>(metadataJson);
        if (metadata == null)
            throw new UserFriendlyException("上传任务元数据损坏！");

        ChunkUploads[uploadId] = metadata;
        return metadata;
    }

    /// <summary>
    /// 清理分片临时文件
    /// </summary>
    private void CleanupChunkTempFiles(ChunkUploadMetadata metadata)
    {
        ChunkUploads.TryRemove(metadata.UploadId, out _);
        if (Directory.Exists(metadata.ChunkTempPath))
        {
            Directory.Delete(metadata.ChunkTempPath, true);
        }
    }

    /// <summary>
    /// 分片上传元数据
    /// </summary>
    private class ChunkUploadMetadata
    {
        public string UploadId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public long ChunkSize { get; set; }
        public int TotalChunks { get; set; }
        public string ContentType { get; set; }
        public string FileHash { get; set; }
        public string ChunkTempPath { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<int> UploadedChunks { get; set; } = new();
    }
}