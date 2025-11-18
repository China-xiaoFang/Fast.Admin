

namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerJobKeyInput"/> 调度作业Key输入
/// </summary>
public class SchedulerJobKeyInput
{
    /// <summary>
    /// 作业名称
    /// </summary>
    [StringRequired(ErrorMessage = "作业名称不能为空")]
    public string JobName { get; set; }

    /// <summary>
    /// 作业分组
    /// </summary>
    [EnumRequired(ErrorMessage = "作业分组不能为空")]
    public SchedulerJobGroupEnum JobGroup { get; set; }
}