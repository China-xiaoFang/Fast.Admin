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

using System.Text;
using Fast.Center.Domain;
using Fast.Center.Service.Login.Dto;
using Fast.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using SKIT.FlurlHttpClient.Wechat.Api.Utilities;
using Yitter.IdGenerator;

namespace Fast.Center.Service.Login;

public partial class LoginService
{
    private const string DefaultClientUserAvatar =
        "https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132";

    private static ClientUserTypeEnum GetClientUserType(AppEnvironmentEnum appType)
    {
        return appType switch
        {
            AppEnvironmentEnum.WeChatMiniProgram => ClientUserTypeEnum.MiniProgram,
            AppEnvironmentEnum.WeChatOfficialAccount => ClientUserTypeEnum.OfficialAccount,
            AppEnvironmentEnum.WeChatServiceAccount => ClientUserTypeEnum.ServiceAccount,
            AppEnvironmentEnum.WeChatOpenPlatform => ClientUserTypeEnum.OpenPlatform,
            AppEnvironmentEnum.WorkWeChat => ClientUserTypeEnum.WorkWeChat,
            _ => throw new UserFriendlyException("暂不支持此类微信客户端！")
        };
    }

    private static ClientUserModel CreateClientUser(long appId, AppEnvironmentEnum appType, string openId, string unionId)
    {
        return new ClientUserModel
        {
            UserId = YitIdHelper.NextId(),
            AppId = appId,
            UserType = GetClientUserType(appType),
            OpenId = openId,
            UnionId = unionId,
            NickName = "微信用户",
            Avatar = DefaultClientUserAvatar,
            Sex = GenderEnum.Unknown
        };
    }

    private static void UpdateMobile(ClientUserModel clientUserModel, string mobile)
    {
        if (string.IsNullOrWhiteSpace(mobile) || clientUserModel.Mobile == mobile)
            return;

        clientUserModel.Mobile = mobile;
        clientUserModel.MobileUpdateTime = DateTime.Now;
    }

    private async Task<DateTime> UpdateClientUserLastLogin(ClientUserModel clientUserModel)
    {
        var dateTime = DateTime.Now;
        var userAgentInfo = _httpContext.RequestUserAgentInfo();
        var ip = _httpContext.RemoteIpv4();
        var wanNetIpInfo = await _httpContext.RemoteIpv4InfoAsync();

        clientUserModel.LastLoginDevice = userAgentInfo.Device;
        clientUserModel.LastLoginOS = userAgentInfo.OS;
        clientUserModel.LastLoginBrowser = userAgentInfo.Browser;
        clientUserModel.LastLoginProvince = wanNetIpInfo.Province;
        clientUserModel.LastLoginCity = wanNetIpInfo.City;
        clientUserModel.LastLoginIp = ip;
        clientUserModel.LastLoginTime = dateTime;
        await _repository.Updateable(clientUserModel)
            .ExecuteCommandAsync();

        return dateTime;
    }

    /// <summary>
    /// 处理微信登录
    /// </summary>
    /// <returns>微信登录结果</returns>
    private async Task<LoginOutput> HandleWeChatLogin(ApplicationModel applicationModel, ClientUserModel clientUserModel)
    {
        var dateTime = await UpdateClientUserLastLogin(clientUserModel);

        // 判断客户端用户是否已绑定手机号
        if (string.IsNullOrWhiteSpace(clientUserModel.Mobile))
        {
            return new LoginOutput {Status = LoginStatusEnum.NotAccount, Message = "客户端用户未绑定手机号，请先授权手机号！"};
        }

        var accountModel = await _repository.Queryable<AccountModel>()
            .Where(wh => wh.Mobile == clientUserModel.Mobile)
            .SingleAsync();

        if (accountModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        // 保证一个客户端用户只绑定一个账号，并持久化首次绑定关系
        if (accountModel.ClientUserId != clientUserModel.UserId)
        {
            await _repository.Ado.UseTranAsync(async () =>
            {
                await _repository.Updateable<AccountModel>()
                    .SetColumns(e => e.ClientUserId == null)
                    .Where(wh => wh.ClientUserId == clientUserModel.UserId)
                    .ExecuteCommandAsync();

                accountModel.ClientUserId = clientUserModel.UserId;
                await _repository.Updateable(accountModel)
                    .ExecuteCommandAsync();
            }, ex => throw ex);
        }

        var tenantUserList = await _repository.Queryable<TenantUserModel>()
            .ClearFilter<IBaseTEntity>()
            .Where(t1 => t1.AccountId == accountModel.AccountId)
            .ToListAsync();
        if (tenantUserList.Count == 0)
        {
            throw new UserFriendlyException("账号未绑定任何租户！");
        }

        // 单租户自动登录
        var autoLogin = bool.Parse(await ConfigContext.GetConfig(ConfigConst.SingleTenantWhenAutoLogin));

        if (tenantUserList.Count == 1 && autoLogin)
        {
            var tenantUserModel = tenantUserList.First();
            // 处理登录
            return await HandleLogin(applicationModel, accountModel, tenantUserModel, dateTime);
        }

        // 微信登录自动选择最后一次登录租户
        if (accountModel.LastLoginTenantId != null)
        {
            var tenantUserModel = tenantUserList.FirstOrDefault(f => f.TenantId == accountModel.LastLoginTenantId);
            if (tenantUserModel != null)
            {
                // 处理登录
                return await HandleLogin(applicationModel, accountModel, tenantUserModel, dateTime);
            }
        }

        // 多个租户，或未开启单租户自动登录
        return new LoginOutput
        {
            Status = LoginStatusEnum.SelectTenant,
            Message = "请选择租户登录",
            AccountKey = accountModel.AccountKey,
            NickName = accountModel.NickName,
            Avatar = accountModel.Avatar,
            TenantList = await _repository.Queryable<TenantUserModel>()
                .InnerJoin<TenantModel>((t1, t2) => t1.TenantId == t2.TenantId)
                .ClearFilter<IBaseTEntity>()
                .Where(t1 => t1.AccountId == accountModel.AccountId)
                .Select((t1, t2) => new LoginTenantOutput
                {
                    UserKey = t1.UserKey,
                    TenantName = t2.TenantName,
                    ShortName = t2.ShortName,
                    SpellName = t2.SpellName,
                    Edition = t2.Edition,
                    LogoUrl = t2.LogoUrl,
                    EmployeeNo = t1.EmployeeNo,
                    EmployeeName = t1.EmployeeName,
                    IdPhoto = t1.IdPhoto,
                    DepartmentId = t1.DepartmentId,
                    DepartmentName = t1.DepartmentName,
                    UserType = t1.UserType,
                    Status = t1.Status
                })
                .ToListAsync()
        };
    }

    /// <summary>
    /// 微信登录
    /// </summary>
    [HttpPost("/weChatLogin")]
    [ApiInfo("微信登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    [EnableRateLimiting(CommonConst.LoginApiRateLimit)]
    public async Task<LoginOutput> WeChatLogin(WeChatLoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 解析微信Code，获取OpenId
        var apiClient = WechatApiClientBuilder
            .Create(new WechatApiClientOptions {AppId = applicationModel.OpenId, AppSecret = applicationModel.OpenSecret})
            .Build();
        var response = await apiClient.ExecuteSnsJsCode2SessionAsync(new SnsJsCode2SessionRequest {JsCode = input.WeChatCode});
        if (!response.IsSuccessful())
        {
            throw new UserFriendlyException(
                $"解析Code失败，获取微信登录信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
        }

        // 根据 OpenId 获取微信用户信息
        var clientUserModel = await _repository.Queryable<ClientUserModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.OpenId == response.OpenId)
            .SingleAsync();
        if (clientUserModel == null)
        {
            // 这里的 IV 和 EncryptedData 在没有授权的情况下是为空的
            if (string.IsNullOrWhiteSpace(input.IV) || string.IsNullOrWhiteSpace(input.EncryptedData))
            {
                return new LoginOutput {Status = LoginStatusEnum.AuthExpired, Message = "授权已过期，请重新授权登录！"};
            }

            // 尝试解析加密数据
            var decryptBytes = AESUtility.DecryptWithCBC(Convert.FromBase64String(response.SessionKey),
                Convert.FromBase64String(input.IV), Convert.FromBase64String(input.EncryptedData));
            var decryptStr = Encoding.UTF8.GetString(decryptBytes);
            var decryptData = decryptStr.ToObject<DecryptWeChatUserInfo>();
            if (decryptData == null)
            {
                throw new UserFriendlyException("解析加密用户信息失败！");
            }

            // 保存微信用户
            clientUserModel = CreateClientUser(applicationModel.AppId, GlobalContext.DeviceType, response.OpenId,
                response.UnionId);
            clientUserModel.SessionKey = response.SessionKey;
            clientUserModel.NickName = decryptData.NickName;
            clientUserModel.Sex = decryptData.Gender;
            await _repository.Insertable(clientUserModel)
                .ExecuteCommandAsync();
        }

        return await HandleWeChatLogin(applicationModel.Application, clientUserModel);
    }

    /// <summary>
    /// 微信授权登录
    /// </summary>
    [HttpPost("/weChatAuthLogin")]
    [ApiInfo("微信授权登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    [EnableRateLimiting(CommonConst.LoginApiRateLimit)]
    public async Task<LoginOutput> WeChatAuthLogin(WeChatAuthLoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 解析微信Code，获取OpenId
        var apiClient = WechatApiClientBuilder
            .Create(new WechatApiClientOptions {AppId = applicationModel.OpenId, AppSecret = applicationModel.OpenSecret})
            .Build();
        var response = await apiClient.ExecuteSnsJsCode2SessionAsync(new SnsJsCode2SessionRequest {JsCode = input.WeChatCode});
        if (!response.IsSuccessful())
        {
            throw new UserFriendlyException(
                $"解析Code失败，获取微信登录信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
        }

        // 根据 OpenId 获取微信用户信息
        var clientUserModel = await _repository.Queryable<ClientUserModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.OpenId == response.OpenId)
            .SingleAsync();
        if (clientUserModel == null)
        {
            return new LoginOutput {Status = LoginStatusEnum.NotAccount, Message = "未找到微信用户信息，请先授权登录！"};
        }

        // 换取用户手机号
        var phoneNumberResponse = await apiClient.ExecuteWxaBusinessGetUserPhoneNumberAsync(
            new WxaBusinessGetUserPhoneNumberRequest {AccessToken = applicationModel.WeChatAccessToken, Code = input.Code});
        if (!phoneNumberResponse.IsSuccessful())
        {
            throw new UserFriendlyException(
                $"解析Code失败，获取用户手机号失败：ErrorCode：{phoneNumberResponse.ErrorCode}。ErrorMessage：{phoneNumberResponse.ErrorMessage}");
        }

        UpdateMobile(clientUserModel, phoneNumberResponse.PhoneInfo.PurePhoneNumber);

        return await HandleWeChatLogin(applicationModel.Application, clientUserModel);
    }

    /// <summary>
    /// 微信客户端登录
    /// </summary>
    [HttpPost("/weChatClientLogin")]
    [ApiInfo("微信客户端登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    [EnableRateLimiting(CommonConst.LoginApiRateLimit)]
    public async Task<WeChatClientLoginOutput> WeChatClientLogin(WeChatClientLoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        var apiClient = WechatApiClientBuilder
            .Create(new WechatApiClientOptions {AppId = applicationModel.OpenId, AppSecret = applicationModel.OpenSecret})
            .Build();

        ClientUserModel clientUserModel = null;

        // 微信小程序
        if (applicationModel.AppType == AppEnvironmentEnum.WeChatMiniProgram)
        {
            // 解析微信Code，获取OpenId
            var response =
                await apiClient.ExecuteSnsJsCode2SessionAsync(new SnsJsCode2SessionRequest {JsCode = input.WeChatCode});
            if (!response.IsSuccessful())
            {
                throw new UserFriendlyException(
                    $"解析Code失败，获取微信登录信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
            }

            // 根据 OpenId 获取微信用户信息
            clientUserModel = await _repository.Queryable<ClientUserModel>()
                .Where(wh => wh.AppId == applicationModel.AppId)
                .Where(wh => wh.OpenId == response.OpenId)
                .SingleAsync();
            if (clientUserModel == null)
            {
                // 保存微信用户
                clientUserModel = CreateClientUser(applicationModel.AppId, GlobalContext.DeviceType, response.OpenId,
                    response.UnionId);
                clientUserModel.SessionKey = response.SessionKey;

                // 这里的 IV 和 EncryptedData 在没有授权的情况下是为空的
                if (string.IsNullOrWhiteSpace(input.IV) != string.IsNullOrWhiteSpace(input.EncryptedData))
                    throw new UserFriendlyException("IV 和加密用户数据必须同时提供！");

                if (!string.IsNullOrWhiteSpace(input.IV) && !string.IsNullOrWhiteSpace(input.EncryptedData))
                {
                    // 尝试解析加密数据
                    var decryptBytes = AESUtility.DecryptWithCBC(Convert.FromBase64String(response.SessionKey),
                        Convert.FromBase64String(input.IV), Convert.FromBase64String(input.EncryptedData));
                    var decryptStr = Encoding.UTF8.GetString(decryptBytes);
                    var decryptData = decryptStr.ToObject<DecryptWeChatUserInfo>();
                    if (decryptData == null)
                    {
                        throw new UserFriendlyException("解析加密用户信息失败！");
                    }

                    clientUserModel.NickName = decryptData.NickName;
                    clientUserModel.Sex = decryptData.Gender;
                }

                await _repository.Insertable(clientUserModel)
                    .ExecuteCommandAsync();
            }

            if (!string.IsNullOrWhiteSpace(input.Code))
            {
                // 换取用户手机号
                var phoneNumberResponse = await apiClient.ExecuteWxaBusinessGetUserPhoneNumberAsync(
                    new WxaBusinessGetUserPhoneNumberRequest
                    {
                        AccessToken = applicationModel.WeChatAccessToken, Code = input.Code
                    });

                if (!phoneNumberResponse.IsSuccessful())
                {
                    throw new UserFriendlyException(
                        $"解析Code失败，获取用户手机号失败：ErrorCode：{phoneNumberResponse.ErrorCode}。ErrorMessage：{phoneNumberResponse.ErrorMessage}");
                }

                UpdateMobile(clientUserModel, phoneNumberResponse.PhoneInfo.PurePhoneNumber);
            }
        }
        // 微信服务号
        else if (applicationModel.AppType == AppEnvironmentEnum.WeChatServiceAccount)
        {
            // 根据 Code 换取用户 AccessToken
            var tokenResponse =
                await apiClient.ExecuteSnsOAuth2AccessTokenAsync(new SnsOAuth2AccessTokenRequest {Code = input.WeChatCode});
            if (!tokenResponse.IsSuccessful())
            {
                return new WeChatClientLoginOutput
                {
                    Status = LoginStatusEnum.AuthExpired,
                    Message =
                        $"解析Code失败，获取用户微信 AccessToken 失败：ErrorCode：{tokenResponse.ErrorCode}。ErrorMessage：{tokenResponse.ErrorMessage}"
                };
            }

            var response = await apiClient.ExecuteSnsUserInfoAsync(new SnsUserInfoRequest
            {
                AccessToken = tokenResponse.AccessToken, OpenId = tokenResponse.OpenId
            });
            if (!response.IsSuccessful())
            {
                throw new UserFriendlyException(
                    $"获取微信用户信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
            }

            // 根据 OpenId 获取微信用户信息
            clientUserModel = await _repository.Queryable<ClientUserModel>()
                .Where(wh => wh.AppId == applicationModel.AppId)
                .Where(wh => wh.OpenId == response.OpenId)
                .SingleAsync();
            if (clientUserModel == null)
            {
                // 保存微信用户
                clientUserModel = CreateClientUser(applicationModel.AppId, GlobalContext.DeviceType, response.OpenId,
                    response.UnionId);
                clientUserModel.NickName = response.Nickname;
                clientUserModel.Avatar = response.HeadImageUrl;
                await _repository.Insertable(clientUserModel)
                    .ExecuteCommandAsync();
            }
            else
            {
                clientUserModel.NickName = response.Nickname;
                clientUserModel.Avatar = response.HeadImageUrl;
            }
        }

        if (clientUserModel == null)
        {
            throw new UserFriendlyException("暂不支持此类客户端！");
        }

        await UpdateClientUserLastLogin(clientUserModel);

        TenantModel tenantMode = null;
        if (applicationModel.Application.TenantId != null)
        {
            tenantMode = await _repository.Queryable<TenantModel>()
                .InSingleAsync(applicationModel.Application.TenantId);
        }

        // 客户端登录
        await _user.ClientLogin(new AuthUserInfo
        {
            DeviceType = GlobalContext.DeviceType,
            DeviceId = GlobalContext.DeviceId,
            AppNo = applicationModel.Application.AppNo,
            AppName = applicationModel.Application.AppName,
            AccountId = clientUserModel.UserId,
            Mobile = clientUserModel.Mobile,
            NickName = clientUserModel.NickName,
            Avatar = clientUserModel.Avatar,
            TenantId = applicationModel.Application.TenantId ?? 0,
            TenantNo = tenantMode?.TenantNo ?? applicationModel.Application.AppNo,
            TenantName = applicationModel.Application.TenantName,
            TenantCode = tenantMode?.TenantCode ?? "",
            IsSystemTenant = false,
            EmployeeId = clientUserModel.UserId,
            EmployeeName = clientUserModel.NickName,
            ClientUserId = clientUserModel.UserId,
            ClientUserOpenId = clientUserModel.OpenId,
            IsSuperAdmin = false,
            IsAdmin = false,
            LastLoginDevice = clientUserModel.LastLoginDevice,
            LastLoginOS = clientUserModel.LastLoginOS,
            LastLoginBrowser = clientUserModel.LastLoginBrowser,
            LastLoginProvince = clientUserModel.LastLoginProvince,
            LastLoginCity = clientUserModel.LastLoginCity,
            LastLoginIp = clientUserModel.LastLoginIp,
            LastLoginTime = clientUserModel.LastLoginTime.Value,
            ButtonCodeList = [PermissionConst.ClientService]
        });

        return new WeChatClientLoginOutput
        {
            OpenId = clientUserModel.OpenId,
            UnionId = clientUserModel.UnionId,
            Mobile = clientUserModel.Mobile,
            NickName = clientUserModel.NickName,
            Avatar = clientUserModel.Avatar
        };
    }
}