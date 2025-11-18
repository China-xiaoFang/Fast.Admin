

namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerJobUrlLogInfo"/> Url调度作业日志信息
/// </summary>
[SuppressSniffer]
internal class SchedulerJobUrlLogInfo : SchedulerJobLogInfo
{
    /// <summary>
    /// 请求Url
    /// </summary>
    public string RequestUrl { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public string RequestMethod { get; set; }

    /// <summary>
    /// 请求超时时间，单位秒（默认不超时）
    /// </summary>
    public int? RequestTimeout { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public string RequestParams { get; set; }

    /// <summary>
    /// 请求头部
    /// </summary>
    public string RequestHeader { get; set; }

    /// <summary>
    /// 响应头部
    /// </summary>
    public IDictionary<string, string> ResponseHeader { get; set; }
}