// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称"软件"）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

namespace Fast.CenterLog.Entity;

/// <summary>
/// <see cref="SchedulerJobLogModel"/> 调度作业日志Model类
/// </summary>
[SugarTable("SchedulerJobLog", "调度作业日志表")]
[SugarDbType(DatabaseTypeEnum.CenterLog)]
[SugarIndex($"IX_{{table}}_{nameof(StartTime)}", nameof(StartTime), OrderByType.Desc)]
[SugarIndex($"IX_{{table}}_{nameof(JobName)}", nameof(JobName), OrderByType.Asc)]
public class SchedulerJobLogModel
{
    /// <summary>
    /// 作业日志Id
    /// </summary>
    [SugarColumn(ColumnDescription = "作业日志Id", IsPrimaryKey = true, IsIdentity = true)]
    public long JobLogId { get; set; }

    /// <summary>
    /// 作业名称
    /// </summary>
    [SugarColumn(ColumnDescription = "作业名称", Length = 200)]
    public string JobName { get; set; }

    /// <summary>
    /// 作业组
    /// </summary>
    [SugarColumn(ColumnDescription = "作业组", Length = 200)]
    public string JobGroup { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    [SugarColumn(ColumnDescription = "作业描述", Length = 500)]
    public string JobDescription { get; set; }

    /// <summary>
    /// 触发器名称
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名称", Length = 200)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器组", Length = 200)]
    public string TriggerGroup { get; set; }

    /// <summary>
    /// 触发器类型
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器类型", Length = 100)]
    public string TriggerType { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    [SugarColumn(ColumnDescription = "是否成功")]
    public bool Success { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    [SugarColumn(ColumnDescription = "错误消息", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string ErrorMessage { get; set; }

    /// <summary>
    /// 堆栈跟踪
    /// </summary>
    [SugarColumn(ColumnDescription = "堆栈跟踪", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string StackTrace { get; set; }

    /// <summary>
    /// 执行数据
    /// </summary>
    [SugarColumn(ColumnDescription = "执行数据", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string ExecuteData { get; set; }

    /// <summary>
    /// 结果数据
    /// </summary>
    [SugarColumn(ColumnDescription = "结果数据", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string ResultData { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnDescription = "耗时（毫秒）")]
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnDescription = "开始时间")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnDescription = "结束时间")]
    public DateTime? EndTime { get; set; }
}
