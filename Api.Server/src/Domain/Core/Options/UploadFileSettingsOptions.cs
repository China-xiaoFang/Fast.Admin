// ReSharper disable once CheckNamespace

namespace Fast.Core;

/// <summary>
/// <see cref="UploadFileSettingsOptions"/> 上传文件配置选项
/// </summary>
public class UploadFileSettingsOptions : IPostConfigure
{
    /// <summary>
    /// Logo
    /// </summary>
    public UploadFileInfoSettings Logo { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public UploadFileInfoSettings Avatar { get; set; }

    /// <summary>
    /// 证件照
    /// </summary>
    public UploadFileInfoSettings IdPhoto { get; set; }

    /// <summary>
    /// 富文本
    /// </summary>
    public UploadFileInfoSettings Editor { get; set; }

    /// <summary>
    /// 默认
    /// </summary>
    public UploadFileInfoSettings Default { get; set; }

    /// <summary>后期配置</summary>
    public void PostConfigure()
    {
        Logo ??= new UploadFileInfoSettings
        {
            Path = "Upload/Logo",
            MaxSize = 2048,
            ContentType =
            [
                "image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"
            ]
        };
        Avatar ??= new UploadFileInfoSettings
        {
            Path = "Upload/Avatar",
            MaxSize = 2048,
            ContentType =
            [
                "image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"
            ]
        };
        IdPhoto ??= new UploadFileInfoSettings
        {
            Path = "Upload/IdPhoto",
            MaxSize = 5120,
            ContentType =
            [
                "image/jpg", "image/jpeg", "image/png"
            ]
        };
        Editor ??= new UploadFileInfoSettings
        {
            Path = "Upload/Editor",
            MaxSize = 10240,
            ContentType =
            [ // 图片类
                "image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp",
                // 视频类
                "video/mp4", "video/mpeg", "video/quicktime", "video/x-msvideo", "video/x-ms-wmv", "video/webm",
                "video/ogg"
            ]
        };
        Default ??= new UploadFileInfoSettings
        {
            Path = "Upload/Default",
            MaxSize = 102400,
            UseTypeFolder = true,
            UseDateFolder = true,
            ContentType =
            [
                // 图片类
                "image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp",
                // 视频类
                "video/mp4", "video/mpeg", "video/quicktime", "video/x-msvideo", "video/x-ms-wmv", "video/webm",
                "video/ogg",
                // 音频类
                "audio/mpeg", "audio/wav", "audio/ogg", "audio/mp4", "audio/flac",
                // 文本类
                "text/plain",
                "text/csv",
                "text/html",
                "text/markdown",
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
                "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                // 压缩包
                "application/zip", "application/x-rar-compressed", "application/x-7z-compressed", "application/gzip"
            ]
        };
    }
}

/// <summary>
/// <see cref="UploadFileInfoSettings"/> 上传文件信息配置
/// </summary>
public class UploadFileInfoSettings
{
    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 最大大小，单位kb
    /// </summary>
    public long MaxSize { get; set; }

    /// <summary>
    /// 使用类型文件夹
    /// </summary>
    public bool UseTypeFolder { get; set; }

    /// <summary>
    /// 使用日期文件夹
    /// </summary>
    public bool UseDateFolder { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string[] ContentType { get; set; }
}