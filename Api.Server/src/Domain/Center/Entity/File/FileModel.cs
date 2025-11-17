// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="FileModel"/> 文件表Model类
/// </summary>
[SugarTable("File", "文件表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(FileHash)}", nameof(TenantId), OrderByType.Asc, nameof(CreatedUserId),
    OrderByType.Asc,
    nameof(FileHash), OrderByType.Asc)]
public class FileModel : IBaseTEntity
{
    /// <summary>
    /// 文件Id
    /// </summary>
    [SugarColumn(ColumnDescription = "文件Id", IsPrimaryKey = true)]
    public long FileId { get; set; }

    /// <summary>
    /// 文件唯一标识
    /// </summary>
    [SugarColumn(ColumnDescription = "文件唯一标识", Length = 255)]
    public string FileObjectName { get; set; }

    /// <summary>
    /// 文件名称（上传时候的文件名）
    /// </summary>
    [SugarColumn(ColumnDescription = "文件名称（上传时候的文件名）", Length = 255)]
    public string FileOriginName { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    [SugarColumn(ColumnDescription = "文件后缀", Length = 16)]
    public string FileSuffix { get; set; }

    /// <summary>
    /// 文件Mime类型
    /// </summary>
    [SugarColumn(ColumnDescription = "文件Mime类型", Length = 100)]
    public string FileMimeType { get; set; }

    /// <summary>
    /// 文件大小kb
    /// </summary>
    [SugarColumn(ColumnDescription = "文件大小kb")]
    public long FileSizeKb { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    [SugarColumn(ColumnDescription = "存储路径", Length = 200)]
    public string FilePath { get; set; }

    /// <summary>
    /// 访问地址
    /// </summary>
    [SugarColumn(ColumnDescription = "访问地址", Length = 200)]
    public string FileLocation { get; set; }

    /// <summary>
    /// 文件哈希
    /// </summary>
    [SugarColumn(ColumnDescription = "文件哈希", Length = 32)]
    public string FileHash { get; set; }

    /// <summary>
    /// 上传设备
    /// </summary>
    [SugarColumn(ColumnDescription = "上传设备", Length = 50)]
    public string UploadDevice { get; set; }

    /// <summary>
    /// 上传操作系统（版本）
    /// </summary>
    [SugarColumn(ColumnDescription = "上传操作系统（版本）", Length = 50)]
    public string UploadOS { get; set; }

    /// <summary>
    /// 上传浏览器（版本）
    /// </summary>
    [SugarColumn(ColumnDescription = "上传浏览器（版本）", Length = 50)]
    public string UploadBrowser { get; set; }

    /// <summary>
    /// 上传省份
    /// </summary>
    [SugarColumn(ColumnDescription = "上传省份", Length = 20)]
    public string UploadProvince { get; set; }

    /// <summary>
    /// 上传城市
    /// </summary>
    [SugarColumn(ColumnDescription = "上传城市", Length = 20)]
    public string UploadCity { get; set; }

    /// <summary>
    /// 上传Ip
    /// </summary>
    [SugarColumn(ColumnDescription = "上传Ip", Length = 15)]
    public string UploadIp { get; set; }

    /// <summary>
    /// 创建者用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户Id", CreateTableFieldSort = 991)]
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户名称", Length = 20, CreateTableFieldSort = 992)]
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "创建时间", CreateTableFieldSort = 993)]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", CreateTableFieldSort = 997)]
    public long TenantId { get; set; }
}