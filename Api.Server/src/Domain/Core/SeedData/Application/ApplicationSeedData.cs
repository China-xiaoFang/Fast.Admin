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
    /// <param name="userId"><see cref="long"/> 用户Id</param>
    /// <param name="userName"><see cref="string"/> 用户名称</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    public static async Task<ApplicationModel> SeedData(ISqlSugarClient db, long userId, string userName, DateTime dateTime)
    {
        var applicationModel = new ApplicationModel
        {
            Id = YitIdHelper.NextId(),
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
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        applicationModel = await db.Insertable(applicationModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ApplicationOpenIdModel>
            {
                new()
                {
                    AppId = applicationModel.Id,
                    OpenId = FastContext.HostEnvironment.IsDevelopment() ? "127.0.0.1:2001" : "admin.fastdotnet.com",
                    AppType = AppEnvironmentEnum.Web,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "ClassicLogin",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.Id,
                    OpenId = "DesktopOpenId",
                    AppType = AppEnvironmentEnum.Desktop,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = false,
                    Remark = null,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.Id,
                    OpenId = "wx1e4b5c59fb34d91f",
                    AppType = AppEnvironmentEnum.WeChatMiniProgram,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.Id,
                    OpenId = "AndroidOpenId",
                    AppType = AppEnvironmentEnum.Android,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    AppId = applicationModel.Id,
                    OpenId = "IOSOpenId",
                    AppType = AppEnvironmentEnum.IOS,
                    EnvironmentType = EnvironmentTypeEnum.Production,
                    LoginComponent = "",
                    WebSocketUrl = "/hubs/chatHub",
                    RequestTimeout = 60000,
                    RequestEncipher = true,
                    Remark = null,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        return applicationModel;
    }
}