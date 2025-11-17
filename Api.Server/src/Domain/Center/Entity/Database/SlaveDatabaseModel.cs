// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="SlaveDatabaseModel"/> 从数据库表Model类
/// </summary>
[SugarTable("DatabaseSlave", "从数据库表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class SlaveDatabaseModel : BaseTEntity, IUpdateVersion
{
    /// <summary>
    /// 从库Id
    /// </summary>
    [SugarColumn(ColumnDescription = "从库Id", IsPrimaryKey = true)]
    public long SlaveId { get; set; }

    /// <summary>
    /// 主库Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主库Id")]
    public long MainId { get; set; }

    /// <summary>
    /// 数据库类型，用于区分使用的是那个类型的数据库
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型")]
    public DbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "公网Ip地址", Length = 15)]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "内网Ip地址", Length = 15)]
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "端口号")]
    public int? Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "数据库名称", Length = 50)]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "数据库用户", Length = 10)]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "数据库密码", Length = 20)]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    [SugarColumn(ColumnDescription = "自定义连接字符串", Length = 100)]
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 从库命中率
    /// </summary>
    /// <remarks>
    /// <para>为 0 则不命中</para>
    /// <para>建议相加不超过10</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "从库命中率")]
    public int HitRate { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}