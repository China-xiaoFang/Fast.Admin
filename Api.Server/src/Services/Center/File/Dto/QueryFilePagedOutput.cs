namespace Fast.Center.Service.File.Dto;

/// <summary>
/// <see cref="QueryFilePagedOutput"/> 获取文件分页列表输出
/// </summary>
public class QueryFilePagedOutput
{
    /// <summary>
    /// 文件Id
    /// </summary>
    public long FileId { get; set; }

    /// <summary>
    /// 文件唯一标识
    /// </summary>
    public string FileObjectName { get; set; }

    /// <summary>
    /// 文件名称（上传时候的文件名）
    /// </summary>
    public string FileOriginName { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    public string FileSuffix { get; set; }

    /// <summary>
    /// 文件Mime类型
    /// </summary>
    public string FileMimeType { get; set; }

    /// <summary>
    /// 文件大小kb
    /// </summary>
    public long FileSizeKb { get; set; }

    /// <summary>
    /// 访问地址
    /// </summary>
    public string FileLocation { get; set; }

    /// <summary>
    /// 上传设备
    /// </summary>
    public string UploadDevice { get; set; }

    /// <summary>
    /// 上传操作系统（版本）
    /// </summary>
    public string UploadOS { get; set; }

    /// <summary>
    /// 上传浏览器（版本）
    /// </summary>
    public string UploadBrowser { get; set; }

    /// <summary>
    /// 上传省份
    /// </summary>
    public string UploadProvince { get; set; }

    /// <summary>
    /// 上传城市
    /// </summary>
    public string UploadCity { get; set; }

    /// <summary>
    /// 上传Ip
    /// </summary>
    public string UploadIp { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}