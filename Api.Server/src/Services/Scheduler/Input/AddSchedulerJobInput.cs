

namespace Fast.Scheduler;

/// <summary>
/// <see cref="AddSchedulerJobInput"/> 添加调度作业输入
/// </summary>
public class AddSchedulerJobInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

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

    /// <summary>
    /// 作业类型
    /// </summary>
    [EnumRequired(ErrorMessage = "作业类型不能为空")]
    public SchedulerJobTypeEnum JobType { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime BeginTime { get; set; } = DateTime.Now;

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
    /// 执行间隔时间，单位秒
    /// </summary>
    /// <remarks>如果有Cron，则IntervalSecond失效</remarks>
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
    [EnumRequired(ErrorMessage = "邮件消息不能为空", AllowZero = true)]
    public MailMessageEnum MailMessage { get; set; } = MailMessageEnum.Error;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    #region Url

    /// <summary>
    /// 请求Url
    /// </summary>
    public string RequestUrl { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public HttpRequestMethodEnum? RequestMethod { get; set; }

    /// <summary>
    /// 请求超时时间，单位秒（默认不超时）
    /// </summary>
    public int? RequestTimeout { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public IDictionary<string, object> RequestParams { get; set; }

    /// <summary>
    /// 请求头部
    /// </summary>
    public IDictionary<string, string> RequestHeader { get; set; }

    #endregion
}