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

using Fast.AdminLog.Enum;

namespace Fast.AdminLog.Entity;

/// <summary>
/// <see cref="OperateLogModel"/> 操作日志Model类
/// </summary>
[SugarTable("OperateLog_{year}{month}{day}", "操作日志表")]
[SplitTable(SplitType.Month)]
[SugarDbType(DatabaseTypeEnum.AdminLog)]
[SugarIndex($"IX_{{table}}_{nameof(CreatedUserId)}", nameof(CreatedUserId), OrderByType.Asc)]
[SugarIndex($"IX_{{table}}_{nameof(CreatedTime)}", nameof(CreatedTime), OrderByType.Asc)]
public class OperateLogModel : BaseRecordEntity
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [SugarColumn(ColumnDescription = "记录Id", IsPrimaryKey = true)]
    public long RecordId { get; set; }

    /// <summary>
    /// 职员Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职员Id")]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "工号", Length = 20)]
    public string EmployeeNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "姓名", Length = 20)]
    public string EmployeeName { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)")]
    public string Mobile { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "操作名称", Length = 50)]
    public string Title { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    [SugarColumn(ColumnDescription = "操作类型")]
    public OperateLogTypeEnum OperateType { get; set; }

    /// <summary>
    /// 业务Id
    /// </summary>
    [SugarColumn(ColumnDescription = "业务Id")]
    public long? BizId { get; set; }

    /// <summary>
    /// 业务编码
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "业务编码", Length = 30)]
    public string BizNo { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(ColumnDescription = "描述", Length = 500)]
    public string Description { get; set; }

    /// <summary>
    /// 操作者用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "操作者用户Id", CreateTableFieldSort = 991)]
    public override long? CreatedUserId { get; set; }

    /// <summary>
    /// 操作者用户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "操作者用户名称", Length = 20, CreateTableFieldSort = 992)]
    public override string CreatedUserName { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    [SplitField]
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "操作时间", CreateTableFieldSort = 993)]
    public override DateTime? CreatedTime { get; set; }
}