using SqlSugar;

namespace Fast.Scheduler.LocalJob.Test;

/// <summary>
/// <see cref="TestSimpleLocalJob"/> 测试 Simple 类型本地作业 
/// </summary>
public class TestSimpleLocalJob : ISchedulerJob
{
    /// <summary>
    /// 获取本地作业
    /// </summary>
    /// <returns></returns>
    public SchedulerLocalJobInfo GetLocalJob()
    {
        return new SchedulerLocalJobInfo
        {
            JobName = "测试 Simple 类型本地作业",
            JobGroup = SchedulerJobGroupEnum.System,
            BeginTime = new DateTime(1970, 01, 01),
            EndTime = null,
            TriggerType = TriggerTypeEnum.Simple,
            Cron = null,
            Week = null,
            DailyStartTime = null,
            DailyEndTime = null,
            IntervalSecond = 60,
            RunTimes = null,
            WarnTime = null,
            RetryTimes = null,
            RetryMillisecond = null,
            MailMessage = MailMessageEnum.None,
            Description = "测试 Simple 类型本地作业，每60秒执行一次。"
        };
    }

    /// <summary>
    /// 执行作业
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> 服务提供者（请求作用域类似于，如果存在 TenantId 则自动注入 IUser 服务）</param>
    /// <param name="db"><see cref="ISqlSugarClient"/> SqlSugar上下文</param>
    /// <param name="logInfo"><see cref="SchedulerJobLocalLogInfo"/> 日志信息</param>
    /// <returns></returns>
    public async Task<string> Execute(IServiceProvider serviceProvider, ISqlSugarClient db,
        SchedulerJobLocalLogInfo logInfo)
    {
        // 等待7秒
        await Task.Delay(7 * 1000);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Test simple local job.");
        Console.ResetColor();

        return await Task.FromResult<string>(null);
    }
}