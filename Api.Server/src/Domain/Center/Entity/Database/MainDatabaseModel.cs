

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="MainDatabaseModel"/> 主数据库表Model类
/// </summary>
[SugarTable("DatabaseMain", "主数据库表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(DatabaseType)}", nameof(TenantId), OrderByType.Asc, nameof(DatabaseType),
    OrderByType.Asc,
    true)]
public class MainDatabaseModel : BaseTEntity, IUpdateVersion
{
    /// <summary>
    /// 主库Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主库Id", IsPrimaryKey = true)]
    public long MainId { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型")]
    public DatabaseTypeEnum DatabaseType { get; set; }

    /// <summary>
    /// 数据库类型，用于区分使用的是那个类型的数据库
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型")]
    public DbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "公网Ip地址", Length = 15)]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "内网Ip地址", Length = 15)]
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    [SugarColumn(ColumnDescription = "端口号")]
    public int Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "数据库名称", Length = 50)]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "数据库用户", Length = 10)]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "数据库密码", Length = 20)]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    [SugarColumn(ColumnDescription = "自定义连接字符串", Length = 100)]
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 超时时间，单位秒
    /// </summary>
    [SugarColumn(ColumnDescription = "超时时间，单位秒")]
    public int CommandTimeOut { get; set; }

    /// <summary>
    /// SqlSugar Sql执行最大秒数，如果超过记录警告日志
    /// </summary>
    [SugarColumn(ColumnDescription = "SqlSugar Sql执行最大秒数，如果超过记录警告日志")]
    public int SugarSqlExecMaxSeconds { get; set; }

    /// <summary>
    /// 差异日志
    /// </summary>
    [SugarColumn(ColumnDescription = "差异日志")]
    public bool DiffLog { get; set; }

    /// <summary>
    /// 禁用 SqlSugar 的 Aop
    /// </summary>
    /// <remarks>
    /// <para>如果是通过 <see cref="ISqlSugarEntityHandler"/> 进行保存日志到数据库中</para>
    /// <para>必须要将相关 AOP 中涉及到的日志表，单独进行分库设置，并且禁用 AOP</para>
    /// <para>或通过 new <see cref="SqlSugarClient"/>() 的方式进行保存。不然会存在死循环的问题</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "差异日志")]
    public bool DisableAop { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }

    /// <summary>
    /// 从库信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(SlaveDatabaseModel.MainId), nameof(MainId))]
    public List<SlaveDatabaseModel> SlaveDatabaseList { get; set; }
}