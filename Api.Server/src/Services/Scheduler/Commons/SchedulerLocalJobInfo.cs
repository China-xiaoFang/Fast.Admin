// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerLocalJobInfo"/> 本地调度作业信息
/// </summary>
[SuppressSniffer]
public class SchedulerLocalJobInfo
{
    /// <summary>
    /// 是否全部租户作业
    /// </summary>
    /// <remarks>如果为 True 则所有调度器都会创建此作业</remarks>
    public bool IsAllTenant { get; set; } = false;

    /// <summary>
    /// 作业名称
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 作业分组
    /// </summary>
    public SchedulerJobGroupEnum JobGroup { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 触发器类型
    /// </summary>
    public TriggerTypeEnum TriggerType { get; set; }

    /// <summary>
    /// Cron表达式
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// 星期（Flag）
    /// </summary>
    public WeekEnum? Week { get; set; }

    /// <summary>
    /// 每天开始时间
    /// </summary>
    public TimeSpan? DailyStartTime { get; set; }

    /// <summary>
    /// 每天结束时间
    /// </summary>
    public TimeSpan? DailyEndTime { get; set; }

    /// <summary>
    /// 执行间隔时间，单位秒（如果有Cron，则IntervalSecond失效）
    /// </summary>
    public int? IntervalSecond { get; set; }

    /// <summary>
    /// 执行次数（默认无限循环）
    /// </summary>
    public int? RunTimes { get; set; }

    /// <summary>
    /// 警告秒数
    /// </summary>
    /// <remarks>优先级最高</remarks>
    public int? WarnTime { get; set; }

    /// <summary>
    /// 重试次数
    /// </summary>
    public int? RetryTimes { get; set; }

    /// <summary>
    /// 重试间隔，单位毫秒
    /// </summary>
    public int? RetryMillisecond { get; set; }

    /// <summary>
    /// 邮件消息
    /// </summary>
    public MailMessageEnum MailMessage { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}