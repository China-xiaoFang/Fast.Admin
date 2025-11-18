using Fast.Center.Entity;


namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerJobLogInfo"/> 调度作业日志信息
/// </summary>
[SuppressSniffer]
public class SchedulerJobLogInfo
{
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

    /// <summary>
    /// 开始执行时间
    /// </summary>
    public DateTime BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    public double ExecuteTime { get; set; }

    /// <summary>
    /// 作业名称
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// 异常消息
    /// </summary>
    public string ErrorMsg { get; set; }
}