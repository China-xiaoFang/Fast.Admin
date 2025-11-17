using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

// ReSharper disable once CheckNamespace
namespace Fast.Scheduler;

/// <summary>
/// <see cref="LocalJob"/> Local调度作业
/// </summary>
internal class LocalJob : JobBase<SchedulerJobLogInfo>
{
    public LocalJob(IServiceProvider serviceProvider, ISchedulerCenter schedulerCenter, IMailService mailService,
        IOptions<MvcNewtonsoftJsonOptions> jsonOptions, ILogger<IJob> logger) : base(serviceProvider, mailService,
        jsonOptions,
        logger, new SchedulerJobUrlLogInfo())
    {
    }

    /// <summary>
    /// 执行调度作业
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> 服务提供者</param>
    /// <param name="db"><see cref="ISqlSugarClient"/> SqlSugar上下文</param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override async Task JobExecute(IServiceProvider serviceProvider, ISqlSugarClient db,
        IJobExecutionContext context)
    {
        // 作业类型
        var jobType = context.JobDetail.JobDataMap.GetEnum<SchedulerJobTypeEnum>(nameof(SchedulerJobInfo.JobType));

        if (jobType != SchedulerJobTypeEnum.Local)
        {
            throw new UserFriendlyException("当前调度作业只支持【本地】！");
        }

        var jobKey = new JobKey(context.JobDetail.Key.Name, context.JobDetail.Key.Group);

        // 尝试从缓存中获取本地调度作业的实现类
        var localSchedulerJobType = SchedulerContext.LocalSchedulerJobTypes.GetValueOrDefault(jobKey.ToString());

        if (localSchedulerJobType == null)
        {
            throw new SchedulerException("未能在缓存中找到【本地】调度作业！");
        }

        // 解析服务
        if (serviceProvider.GetService(localSchedulerJobType) is not ISchedulerJob _schedulerJob)
        {
            throw new UserFriendlyException("解析【本地】调度作业服务异常，服务不存在！");
        }

        // 执行本地调度作业
        var result = await _schedulerJob.Execute(serviceProvider, db,
            new SchedulerJobLocalLogInfo
            {
                JobName = _logInfo.JobName,
                InfoLog = InfoLog,
                WarnLog = WarnLog,
                ErrorLog = ErrorLog,
                TenantId = _logInfo.TenantId,
                TenantName = _logInfo.TenantName,
                TenantNo = _logInfo.TenantNo,
                RobotInfo = _logInfo.RobotInfo
            });

        if (!string.IsNullOrWhiteSpace(result))
        {
            _logInfo.Result =
                $"<span class='result'>{HttpUtility.HtmlEncode(result).GetSubStringWithEllipsis(1000, true)}</span>";
        }
    }
}