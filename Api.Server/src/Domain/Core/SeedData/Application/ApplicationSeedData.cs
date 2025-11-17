using Fast.Center.Entity;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="ApplicationSeedData"/> 应用种子数据
/// </summary>
internal static class ApplicationSeedData
{
    /// <summary>
    /// 应用种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    public static async Task<ApplicationModel> SeedData(ISqlSugarClient db, DateTime dateTime)
    {
        var applicationModel = new ApplicationModel
        {
            AppId = YitIdHelper.NextId(),
            Edition = EditionEnum.None,
            AppNo = "App201801",
            AppName = "Fast.Admin",
            LogoUrl = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
            ThemeColor = "#409EFF",
            ICPSecurityCode = "陇ICP备2020003856号",
            PublicSecurityCode = "甘公网安备 62090202000584号",
            UserAgreement = null,
            PrivacyAgreement = null,
            ServiceAgreement = null,
            Remark = null,
            CreatedTime = dateTime
        };
        applicationModel = await db.Insertable(applicationModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ApplicationOpenIdModel>
            {
                new()
                {
                    AppId = applicationModel.AppId,
                    OpenId = FastContext.HostEnvironment.IsDevelopment() ? "127.0.0.1:2001" : "admin.fastdotnet.com",
                    AppType = AppEnvironmentEnum.Web,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "ClassicLogin",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.AppId,
                    OpenId = "DesktopOpenId",
                    AppType = AppEnvironmentEnum.Desktop,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = false,
                    Remark = null,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.AppId,
                    OpenId = "WeChatMiniProgramOpenId",
                    AppType = AppEnvironmentEnum.WeChatMiniProgram,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.AppId,
                    OpenId = "AndroidOpenId",
                    AppType = AppEnvironmentEnum.Android,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.AppId,
                    OpenId = "IOSOpenId",
                    AppType = AppEnvironmentEnum.IOS,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        return applicationModel;
    }
}