using Fast.Core;
using Fast.DynamicApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Calendar;
using Quartz.Impl.Triggers;

namespace Fast.Scheduler.Applications;

/// <summary>
/// <see cref="SchedulerApplication"/> 调度作业
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Scheduler, Name = "scheduler")]
public class SchedulerApplication : IDynamicApplication
{
    /// <summary>
    /// <see cref="ISchedulerCenter"/> 调度中心
    /// </summary>
    private readonly ISchedulerCenter _schedulerCenter;

    /// <summary>
    /// <see cref="SchedulerApplication"/> 调度作业
    /// </summary>
    /// <param name="schedulerCenter"><see cref="ISchedulerCenter"/> 调度中心</param>
    public SchedulerApplication(ISchedulerCenter schedulerCenter)
    {
        _schedulerCenter = schedulerCenter;
    }

    /// <summary>
    /// 运行并验证Cron表达式
    /// </summary>
    /// <param name="cron"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("运行并验证Cron表达式", HttpRequestActionEnum.Other)]
    public List<string> RunVerifyCron(string cron)
    {
        // 验证表达式是否正确
        if (!CronExpression.IsValidExpression(cron))
        {
            return ["请检查Cron表达式是否拼写正确！"];
        }

        var result = new List<string>();

        var cronTrigger = new CronTriggerImpl("TestName", "TestGroup", cron);
        var calendar = new BaseCalendar(TimeZoneInfo.Local);
        // 默认获取10条
        var list = TriggerUtils.ComputeFireTimes(cronTrigger, calendar, 10);

        foreach (var item in list)
        {
            result.Add(item.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        return result;
    }

    /// <summary>
    /// 获取调度器详情
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取调度器详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Scheduler.Detail)]
    public async Task<QuerySchedulerDetailOutput> QuerySchedulerDetail(long? tenantId = null)
    {
        return await _schedulerCenter.QuerySchedulerDetail(tenantId);
    }

    /// <summary>
    /// 启动调度器
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("启动调度器", HttpRequestActionEnum.Other)]
    [Permission(PermissionConst.Scheduler.Start)]
    public async Task StartScheduler(long? tenantId = null)
    {
        await _schedulerCenter.StartScheduler(tenantId);
    }

    /// <summary>
    /// 停止调度器
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("停止调度器", HttpRequestActionEnum.Other)]
    [Permission(PermissionConst.Scheduler.Stop)]
    public async Task StopScheduler(long? tenantId = null)
    {
        await _schedulerCenter.StopScheduler(tenantId);
    }

    /// <summary>
    /// 暂停调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("暂停调度作业", HttpRequestActionEnum.Other)]
    [Permission(PermissionConst.Scheduler.StopJob)]
    public async Task StopSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null)
    {
        await _schedulerCenter.StopSchedulerJob(input, tenantId);
    }

    /// <summary>
    /// 恢复调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("恢复调度作业", HttpRequestActionEnum.Other)]
    [Permission(PermissionConst.Scheduler.ResumeJob)]
    public async Task ResumeSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null)
    {
        await _schedulerCenter.ResumeSchedulerJob(input, tenantId);
    }

    /// <summary>
    /// 立即执行调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("立即执行调度作业", HttpRequestActionEnum.Other)]
    [Permission(PermissionConst.Scheduler.Trigger)]
    public async Task TriggerSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null)
    {
        await _schedulerCenter.TriggerSchedulerJob(input, tenantId);
    }

    /// <summary>
    /// 获取调度作业日志
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取调度作业日志", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Scheduler.Detail)]
    public async Task<List<string>> QuerySchedulerJobLogs(SchedulerJobKeyInput input, long? tenantId = null)
    {
        return await _schedulerCenter.QuerySchedulerJobLogs(input, tenantId);
    }

    /// <summary>
    /// 获取调度作业运行次数
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取调度作业运行次数", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Scheduler.Detail)]
    public async Task<long> QuerySchedulerJobRunNumber(SchedulerJobKeyInput input, long? tenantId = null)
    {
        return await _schedulerCenter.QuerySchedulerJobRunNumber(input, tenantId);
    }

    /// <summary>
    /// 获取全部调度作业
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="jobGroup"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取全部调度作业", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Scheduler.Paged)]
    public async Task<List<QueryAllSchedulerJobOutput>> QueryAllSchedulerJob(SchedulerJobGroupEnum? jobGroup = null,
        long? tenantId = null)
    {
        return await _schedulerCenter.QueryAllSchedulerJob(jobGroup, tenantId);
    }

    /// <summary>
    /// 获取调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取调度作业", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Scheduler.Detail)]
    public async Task<SchedulerJobInfo> QuerySchedulerJob(SchedulerJobKeyInput input, long? tenantId = null)
    {
        return await _schedulerCenter.QuerySchedulerJob(input, tenantId);
    }

    /// <summary>
    /// 添加调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加调度作业", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Scheduler.Add)]
    public async Task AddSchedulerJob(AddSchedulerJobInput input)
    {
        await _schedulerCenter.AddSchedulerJob(input);
    }

    /// <summary>
    /// 编辑调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑调度作业", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Scheduler.Edit)]
    public async Task EditSchedulerJob(UpdateSchedulerJobInput input)
    {
        await _schedulerCenter.EditSchedulerJob(input);
    }

    /// <summary>
    /// 删除调度作业
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除调度作业", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Scheduler.Delete)]
    public async Task DeleteSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null)
    {
        await _schedulerCenter.DeleteSchedulerJob(input, tenantId);
    }

    /// <summary>
    /// 移除调度作业异常信息
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("移除调度作业异常信息", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Scheduler.Delete)]
    public async Task DeleteSchedulerJobException(SchedulerJobKeyInput input, long? tenantId = null)
    {
        await _schedulerCenter.DeleteSchedulerJobException(input, tenantId);
    }
}