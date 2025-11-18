using Fast.Center.Entity;


namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerJobLocalLogInfo"/> Local调度作业日志信息
/// </summary>
[SuppressSniffer]
public class SchedulerJobLocalLogInfo
{
    /// <summary>
    /// 作业名称
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 信息日志
    /// </summary>
    public Func<string, string, Task> InfoLog { get; set; }

    /// <summary>
    /// 警告日志
    /// </summary>
    public Func<string, string, Task> WarnLog { get; set; }

    /// <summary>
    /// 错误日志
    /// </summary>
    public Func<string, Exception, string, Task> ErrorLog { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public string TenantNo { get; set; }

    /// <summary>
    /// 机器人信息
    /// </summary>
    public TenantUserModel RobotInfo { get; set; }
}