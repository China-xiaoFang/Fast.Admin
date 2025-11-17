using Quartz;

// ReSharper disable once CheckNamespace
namespace Fast.Scheduler;

/// <summary>
/// <see cref="QueryAllSchedulerJobOutput"/> 获取全部调度作业输出
/// </summary>
public class QueryAllSchedulerJobOutput
{
    /// <summary>
    /// 作业分组
    /// </summary>
    public SchedulerJobGroupEnum JobGroup { get; set; }

    /// <summary>
    /// 作业信息
    /// </summary>
    public List<SchedulerJobInfoDto> JobInfoList { get; set; }

    /// <summary>
    /// 调度作业信息
    /// </summary>
    public class SchedulerJobInfoDto
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? PreviousFireTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 作业类型
        /// </summary>
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
        /// 时间间隔
        /// </summary>
        /// <remarks>Cron/执行间隔时间</remarks>
        public string Interval { get; set; }

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

        #endregion

        /// <summary>
        /// 触发器状态
        /// </summary>
        public TriggerState TriggerState { get; set; }

        /// <summary>
        /// 运行次数
        /// </summary>
        /// <remarks>指作业一共运行多少次</remarks>
        public long RunNumber { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public string Exception { get; set; }
    }
}