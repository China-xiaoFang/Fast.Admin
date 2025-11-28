// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using Fast.Center.Enum;
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
    public async Task<string> Execute(IServiceProvider serviceProvider, ISqlSugarClient db, SchedulerJobLocalLogInfo logInfo)
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