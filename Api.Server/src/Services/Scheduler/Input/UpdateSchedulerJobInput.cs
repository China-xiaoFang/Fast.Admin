// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="UpdateSchedulerJobInput"/> 更新调度作业输入
/// </summary>
public class UpdateSchedulerJobInput : AddSchedulerJobInput
{
    /// <summary>
    /// 旧的作业名称
    /// </summary>
    [StringRequired(ErrorMessage = "旧的作业名称不能为空")]
    public string OldJobName { get; set; }

    /// <summary>
    /// 旧的作业分组
    /// </summary>
    [EnumRequired(ErrorMessage = "旧的作业分组不能为空")]
    public SchedulerJobGroupEnum OldJobGroup { get; set; }
}