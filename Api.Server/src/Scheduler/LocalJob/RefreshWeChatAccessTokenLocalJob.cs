using Fast.Center.Entity;
using Fast.Core;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using SqlSugar;

namespace Fast.Scheduler.LocalJob;

/// <summary>
/// <see cref="RefreshWeChatAccessTokenLocalJob"/> 刷新微信 AccessToken 本地作业
/// </summary>
public class RefreshWeChatAccessTokenLocalJob : ISchedulerJob
{
    /// <summary>
    /// 获取本地作业
    /// </summary>
    /// <returns></returns>
    public SchedulerLocalJobInfo GetLocalJob()
    {
        return new SchedulerLocalJobInfo
        {
            JobName = "刷新微信 AccessToken 本地作业",
            JobGroup = SchedulerJobGroupEnum.System,
            BeginTime = new DateTime(1970, 01, 01),
            EndTime = null,
            TriggerType = TriggerTypeEnum.Simple,
            Cron = null,
            Week = null,
            DailyStartTime = null,
            DailyEndTime = null,
            IntervalSecond = 7200,
            RunTimes = null,
            WarnTime = null,
            RetryTimes = null,
            RetryMillisecond = null,
            MailMessage = MailMessageEnum.Error,
            Description = "刷新微信 AccessToken 本地作业，每7200秒执行一次。"
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
        // 进入方法的一瞬间记录时间
        var dateTime = DateTime.Now;

        // 获取所有微信小程序信息
        var applicationOpenIdList = await db.Queryable<ApplicationOpenIdModel>()
            .Where(wh => (wh.AppType & (AppEnvironmentEnum.MiniProgram | AppEnvironmentEnum.WeChatServiceAccount)) != 0)
            .Where(wh => !string.IsNullOrWhiteSpace(wh.OpenSecret))
            .Select(sl => new { sl.RecordId, sl.AppType, sl.OpenId, sl.OpenSecret })
            .ToListAsync();

        foreach (var item in applicationOpenIdList)
        {
            var options = new WechatApiClientOptions { AppId = item.OpenId, AppSecret = item.OpenSecret };
            var client = WechatApiClientBuilder.Create(options)
                .Build();
            var response =
                await client.ExecuteCgibinStableTokenAsync(new CgibinStableTokenRequest { ForceRefresh = true });
            if (!response.IsSuccessful())
            {
                var message =
                    $"调用刷新AccessToken接口失败。ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}";
                await logInfo.ErrorLog(logInfo.JobName, null, message);
                return message;
            }

            await db.Updateable<ApplicationOpenIdModel>()
                .SetColumns(_ => new ApplicationOpenIdModel
                {
                    WeChatAccessToken = response.AccessToken,
                    WeChatAccessTokenExpiresIn = response.ExpiresIn,
                    WeChatAccessTokenRefreshTime = dateTime
                })
                .Where(wh => wh.RecordId == item.RecordId)
                .ExecuteCommandAsync();
            // 删除缓存
            await ApplicationContext.DeleteApplication(item.OpenId);

            if (item.AppType == AppEnvironmentEnum.WeChatServiceAccount)
            {
                var ticketResponse = await client.ExecuteCgibinTicketGetTicketAsync(new CgibinTicketGetTicketRequest
                {
                    AccessToken = response.AccessToken
                });
                if (!ticketResponse.IsSuccessful())
                {
                    var message =
                        $"调用获取Ticket接口失败。ErrorCode：{ticketResponse.ErrorCode}。ErrorMessage：{ticketResponse.ErrorMessage}";
                    await logInfo.ErrorLog(logInfo.JobName, null, message);
                    return message;
                }

                await db.Updateable<ApplicationOpenIdModel>()
                    .SetColumns(_ => new ApplicationOpenIdModel
                    {
                        WeChatJsApiTicket = ticketResponse.Ticket,
                        WeChatJsApiTicketExpiresIn = ticketResponse.ExpiresIn,
                        WeChatJsApiTicketRefreshTime = dateTime
                    })
                    .Where(wh => wh.RecordId == item.RecordId)
                    .ExecuteCommandAsync();
                // 删除缓存
                await ApplicationContext.DeleteApplication(item.OpenId);
            }
        }

        return $"刷新微信 AccessToken，共计：{applicationOpenIdList.Count}个。";
    }
}