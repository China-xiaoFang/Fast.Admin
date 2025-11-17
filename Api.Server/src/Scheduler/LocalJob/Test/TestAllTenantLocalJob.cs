using SqlSugar;

namespace Fast.Scheduler.LocalJob.Test;

/// <summary>
/// <see cref="TestAllTenantLocalJob"/> 测试全部租户本地作业
/// </summary>
public class TestAllTenantLocalJob : ISchedulerJob
{
    /// <summary>
    /// 获取本地作业
    /// </summary>
    /// <returns></returns>
    public SchedulerLocalJobInfo GetLocalJob()
    {
        return new SchedulerLocalJobInfo
        {
            IsAllTenant = true,
            JobName = "测试全部租户本地作业",
            JobGroup = SchedulerJobGroupEnum.System,
            BeginTime = new DateTime(1970, 01, 01),
            EndTime = null,
            TriggerType = TriggerTypeEnum.Cron,
            Cron = "0 */5 * * * ?",
            Week = null,
            DailyStartTime = null,
            DailyEndTime = null,
            IntervalSecond = null,
            RunTimes = null,
            WarnTime = null,
            RetryTimes = null,
            RetryMillisecond = null,
            MailMessage = MailMessageEnum.None,
            Description = "测试全部租户本地作业，每5分钟执行一次。"
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
        // 等待3秒
        await Task.Delay(3 * 1000);

        if (logInfo.TenantId == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync("Test tenant local job error. TenantId is null.");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Test {logInfo.TenantId} tenant local job.");
        Console.ResetColor();

        return await Task.FromResult<string>(null);
    }
}