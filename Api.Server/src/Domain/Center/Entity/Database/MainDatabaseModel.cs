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

namespace Fast.Center.Entity;

/// <summary>
/// 主数据库表Model类
/// </summary>
[SugarTable("DatabaseMain", "主数据库表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(DatabaseType)}", nameof(DatabaseType), OrderByType.Asc, nameof(TenantId), OrderByType.Asc,
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
    /// 数据库类型，用于区分所使用的数据库引擎
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型，用于区分所使用的数据库引擎")]
    public SugarDbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
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
    /// <remarks>或 SQLite 文件路径</remarks>
    [Required]
    [SugarColumn(ColumnDescription = "数据库名称", Length = 500)]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库用户", Length = 128)]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库密码", Length = 512)]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    [SugarColumn(ColumnDescription = "自定义连接字符串", Length = 2000)]
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 超时时间，单位秒
    /// </summary>
    [SugarColumn(ColumnDescription = "超时时间，单位秒")]
    public int CommandTimeOut { get; set; }

    /// <summary>
    /// SqlSugar SQL执行警告阈值（秒）
    /// </summary>
    [SugarColumn(ColumnDescription = "SqlSugar SQL执行警告阈值（秒）")]
    public int SugarSqlExecMaxSeconds { get; set; }

    /// <summary>
    /// 差异日志
    /// </summary>
    [SugarColumn(ColumnDescription = "差异日志")]
    public bool DiffLog { get; set; }

    /// <summary>
    /// 是否禁用 SqlSugar AOP
    /// </summary>
    /// <remarks>
    /// <para>使用 <see cref="ISqlSugarEntityHandler"/> 将日志保存到数据库时，必须为 AOP 涉及的日志表单独配置分库，并禁用 AOP</para>
    /// <para>也可以通过 <c>new</c> <see cref="SqlSugarClient"/> 的方式保存日志，否则可能产生递归调用</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "是否禁用 SqlSugar AOP")]
    public bool DisableAop { get; set; }

    /// <summary>
    /// 是否初始化
    /// </summary>
    [SugarColumn(ColumnDescription = "是否初始化")]
    public bool IsInitialized { get; set; }

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