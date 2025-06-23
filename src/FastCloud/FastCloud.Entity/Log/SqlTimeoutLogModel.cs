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

// ReSharper disable once CheckNamespace

namespace Fast.FastCloud.Entity;

/// <summary>
/// <see cref="SqlTimeoutLogModel"/> Sql超时日志Model类
/// </summary>
[SugarTable("SqlTimeoutLog", "Sql超时日志表")]
[SugarDbType(DatabaseTypeEnum.FastCloud)]
public class SqlTimeoutLogModel : BaseIdentityRecordEntity
{
    /// <summary>
    /// 平台Id
    /// </summary>
    [SugarColumn(ColumnDescription = "平台Id")]
    public long PlatformId { get; set; }

    /// <summary>
    /// 平台名称
    /// </summary>
    [SugarColumn(ColumnDescription = "平台名称", Length = 20, IsNullable = false)]
    public string PlatformName { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id")]
    public long AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [SugarColumn(ColumnDescription = "应用名称", Length = 20, IsNullable = false)]
    public string AppName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id")]
    public long? TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "租户名称", Length = 30, IsNullable = true)]
    public string TenantName { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)", IsNullable = true)]
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", Length = 20, IsNullable = false)]
    public string NickName { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    [SugarColumn(ColumnDescription = "文件名称", Length = 200, IsNullable = true)]
    public string FileName { get; set; }

    /// <summary>
    /// 文件行数
    /// </summary>
    [SugarColumn(ColumnDescription = "文件行数")]
    public int FileLine { get; set; }

    /// <summary>
    /// 方法名
    /// </summary>
    [SugarColumn(ColumnDescription = "方法名", Length = 200, IsNullable = true)]
    public string MethodName { get; set; }

    /// <summary>
    /// 超时秒数
    /// </summary>
    [SugarColumn(ColumnDescription = "超时秒数")]
    public double TimeoutSeconds { get; set; }

    /// <summary>
    /// 原始Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "原始Sql", ColumnDataType = StaticConfig.CodeFirst_BigString, IsNullable = true)]
    public string RawSql { get; set; }

    /// <summary>
    /// Sql参数
    /// </summary>
    [SugarColumn(ColumnDescription = "Sql参数",
        ColumnDataType = StaticConfig.CodeFirst_BigString,
        IsNullable = true,
        IsJson = true)]
    public SugarParameter[] Parameters { get; set; }

    /// <summary>
    /// 纯Sql，参数化之后的Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "纯Sql，参数化之后的Sql", ColumnDataType = StaticConfig.CodeFirst_BigString, IsNullable = true)]
    public string PureSql { get; set; }

    /// <summary>
    /// 超时时间
    /// </summary>
    [SugarSearchTime]
    public override DateTime? CreatedTime { get; set; }
}