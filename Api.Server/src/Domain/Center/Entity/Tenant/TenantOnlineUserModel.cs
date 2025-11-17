// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="TenantOnlineUserModel"/> 租户在线用户表Model类
/// </summary>
[SugarTable("TenantOnlineUser", "租户在线用户表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(DeviceId)}", nameof(DeviceId), OrderByType.Asc)]
[SugarIndex($"IX_{{table}}_{nameof(AppNo)}", nameof(AppNo), OrderByType.Asc)]
public class TenantOnlineUserModel : IBaseTEntity
{
    /// <summary>
    /// 连接Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "连接Id", Length = 32, IsPrimaryKey = true)]
    public string ConnectionId { get; set; }

    /// <summary>
    /// 设备类型
    /// </summary>
    [SugarColumn(ColumnDescription = "设备类型")]
    public AppEnvironmentEnum DeviceType { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    [SugarColumn(ColumnDescription = "设备Id", Length = 36)]
    public string DeviceId { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "应用编号", Length = 11)]
    public string AppNo { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "应用名称", Length = 20)]
    public string AppName { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    [SugarColumn(ColumnDescription = "账号Id")]
    public long AccountId { get; set; }

    /// <summary>
    /// 账号Key
    /// </summary>
    [SugarColumn(ColumnDescription = "账号Key", ColumnDataType = "varchar(12)")]
    public string AccountKey { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)")]
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "昵称", Length = 20)]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(ColumnDescription = "头像", Length = 200)]
    public string Avatar { get; set; }

    /// <summary>
    /// 用户Id/职员Id
    /// </summary>
    [SugarColumn(ColumnDescription = "用户Id/职员Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 用户Key
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "用户Key", ColumnDataType = "varchar(12)")]
    public string UserKey { get; set; }

    /// <summary>
    /// 账户
    /// </summary>
    [SugarColumn(ColumnDescription = "账户", Length = 20)]
    public string Account { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    [SugarColumn(ColumnDescription = "工号", ColumnDataType = "varchar(11)")]
    public string EmployeeNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "姓名", Length = 20)]
    public string EmployeeName { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [SugarColumn(ColumnDescription = "部门Id")]
    public long? DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [SugarColumn(ColumnDescription = "部门名称", Length = 20)]
    public string DepartmentName { get; set; }

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    [SugarColumn(ColumnDescription = "是否超级管理员")]
    public bool IsSuperAdmin { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    [SugarColumn(ColumnDescription = "是否管理员")]
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 最后登录设备
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录设备", Length = 50)]
    public string LastLoginDevice { get; set; }

    /// <summary>
    /// 最后登录操作系统（版本）
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录操作系统（版本）", Length = 50)]
    public string LastLoginOS { get; set; }

    /// <summary>
    /// 最后登录浏览器（版本）
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录浏览器（版本）", Length = 50)]
    public string LastLoginBrowser { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录省份", Length = 20)]
    public string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录城市", Length = 20)]
    public string LastLoginCity { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录Ip", Length = 15)]
    public string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录时间")]
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 是否在线
    /// </summary>
    [SugarColumn(ColumnDescription = "是否在线")]
    public bool IsOnline { get; set; }

    /// <summary>
    /// 下线时间
    /// </summary>
    [SugarColumn(ColumnDescription = "下线时间")]
    public DateTime? OfflineTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", CreateTableFieldSort = 997)]
    public long TenantId { get; set; }
}