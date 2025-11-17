// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="ISchedulerCenter"/> 调度中心
/// </summary>
public interface ISchedulerCenter
{
    /// <summary>
    /// 初始化调度程序
    /// </summary>
    /// <returns></returns>
    Task InitializeScheduler();

    /// <summary>
    /// 同步调度程序
    /// </summary>
    /// <returns></returns>
    Task SyncScheduler();

    /// <summary>
    /// 获取调度器详情
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<QuerySchedulerDetailOutput> QuerySchedulerDetail(long? tenantId = null);

    /// <summary>
    /// 启动调度器
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<bool> StartScheduler(long? tenantId = null);

    /// <summary>
    /// 停止调度器
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<bool> StopScheduler(long? tenantId = null);

    /// <summary>
    /// 暂停调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task StopSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 恢复调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task ResumeSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 立即执行调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task TriggerSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 是否存在调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<bool> ExistsSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 获取调度作业日志
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<List<string>> QuerySchedulerJobLogs(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 获取调度作业运行次数
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<long> QuerySchedulerJobRunNumber(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 获取全部调度作业
    /// </summary>
    /// <param name="jobGroup"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<List<QueryAllSchedulerJobOutput>> QueryAllSchedulerJob(SchedulerJobGroupEnum? jobGroup = null,
        long? tenantId = null);

    /// <summary>
    /// 获取调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<SchedulerJobInfo> QuerySchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 添加调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task AddSchedulerJob(AddSchedulerJobInput input);

    /// <summary>
    /// 编辑调度作业
    /// </summary>
    /// <remarks>注：这里更新作业会导致触发器的执行记录被清空。所以会导致更新后可能会立即执行一次。</remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    Task EditSchedulerJob(UpdateSchedulerJobInput input);

    /// <summary>
    /// 删除调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task DeleteSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 移除调度作业异常信息
    /// </summary>
    /// <remarks>因为只能在 IJob 持久化操作 JobDataMap，所以这里直接暴力操作数据库</remarks>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task DeleteSchedulerJobException(SchedulerJobKeyInput input, long? tenantId = null);
}