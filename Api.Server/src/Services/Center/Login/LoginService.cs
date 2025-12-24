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
using System.Text.RegularExpressions;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.Center.Service.Login.Dto;
using Fast.CenterLog.Entity;
using Fast.CenterLog.Enum;
using Fast.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using SKIT.FlurlHttpClient.Wechat.Api.Utilities;
using Yitter.IdGenerator;

namespace Fast.Center.Service.Login;

/// <summary>
/// <see cref="LoginService"/> 登录服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Auth, Name = "login", Order = 999)]
public class LoginService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly HttpContext _httpContext;
    private readonly ISqlSugarClient _repository;

    public LoginService(IUser user, IHttpContextAccessor httpContextAccessor, ISqlSugarClient repository)
    {
        _user = user;
        _httpContext = httpContextAccessor.HttpContext;
        _repository = repository;
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="accountModel"></param>
    /// <param name="password"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    private async Task VerifyPassword(AccountModel accountModel, string password, DateTime dateTime)
    {
        if (string.IsNullOrWhiteSpace(accountModel.Password))
        {
            throw new UserFriendlyException("未设定密码，请重置密码后重试！");
        }

        /*
         * 连续错误3次，锁定1分钟
         * 连续错误5次，锁定5分钟
         * 连续错误10次，锁定账号
         * 登录成功后清除锁定信息
         */
        if (!string.Equals(password, accountModel.Password, StringComparison.OrdinalIgnoreCase))
        {
            // 判断是否存在锁定时间
            if (accountModel.LockEndTime != null && accountModel.LockEndTime > dateTime)
            {
                var unLockTimeSpan = accountModel.LockEndTime.Value - dateTime;
                throw new UserFriendlyException($"账号已被锁定，请 {unLockTimeSpan.ToDescription()} 后再重试！");
            }

            accountModel.PasswordErrorTime ??= 0;
            // 错误次数+1
            accountModel.PasswordErrorTime++;

            switch (accountModel.PasswordErrorTime)
            {
                // 错误3次，锁定1分钟
                case 3:
                    accountModel.LockStartTime ??= dateTime;
                    accountModel.LockEndTime = accountModel.LockStartTime.Value.AddMinutes(1);
                    await _repository.Updateable(accountModel)
                        .ExecuteCommandAsync();
                    break;
                // 错误5次，锁定5分钟
                case 5:
                    accountModel.LockStartTime ??= dateTime;
                    accountModel.LockEndTime = dateTime.AddMinutes(5);
                    await _repository.Updateable(accountModel)
                        .ExecuteCommandAsync();
                    break;
                // 判断是否连续错误10次以上
                case >= 10:
                    // 错误10次，直接禁用账号
                    accountModel.Status = CommonStatusEnum.Disable;
                    await _repository.Updateable(accountModel)
                        .ExecuteCommandAsync();
                    throw new UserFriendlyException("密码连续输入错误10次，账号已被禁用，请联系管理员！");
            }

            await _repository.Updateable(accountModel)
                .ExecuteCommandAsync();

            throw new UserFriendlyException("密码不正确！");
        }

        // 清除锁定信息
        if (accountModel.PasswordErrorTime != null)
        {
            accountModel.PasswordErrorTime = null;
            accountModel.LockStartTime = null;
            accountModel.LockEndTime = null;
        }
    }

    /// <summary>
    /// 处理登录
    /// </summary>
    /// <param name="applicationModel"></param>
    /// <param name="accountModel"></param>
    /// <param name="tenantUserModel"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    private async Task<LoginOutput> HandleLogin(ApplicationModel applicationModel, AccountModel accountModel,
        TenantUserModel tenantUserModel, DateTime dateTime)
    {
        // 验证账号状态
        if (accountModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("账号已被平台禁用！");
        }

        if (tenantUserModel == null)
        {
            throw new UserFriendlyException("用户不存在！");
        }

        // 验证租户用户状态
        if (tenantUserModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("用户已被禁用！");
        }

        // 验证是否为机器人
        if (tenantUserModel.UserType == UserTypeEnum.Robot)
        {
            throw new UserFriendlyException("无效用户！");
        }

        // 查询租户
        var tenantModel = await _repository.Queryable<TenantModel>()
            .Where(wh => wh.TenantId == tenantUserModel.TenantId)
            .SingleAsync();

        if (tenantModel == null)
        {
            throw new UserFriendlyException("租户不存在！");
        }

        if (tenantModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("租户已被禁用！");
        }

        // 验证版本
        if (tenantModel.Edition < applicationModel.Edition)
        {
            throw new UserFriendlyException(
                $"当前租户版本【{tenantModel.Edition.GetDescription()}】不支持访问该应用，请升级至【{applicationModel.Edition.GetDescription()}】或更高版本。");
        }

        // 获取设备信息
        var userAgentInfo = _httpContext.RequestUserAgentInfo();
        // 获取Ip信息
        var ip = _httpContext.RemoteIpv4();
        // 获取万网信息
        var wanNetIpInfo = await _httpContext.RemoteIpv4InfoAsync();

        if (accountModel.FirstLoginTime == null)
        {
            accountModel.FirstLoginTenantId = tenantModel.TenantId;
            accountModel.FirstLoginDevice = userAgentInfo.Device;
            accountModel.FirstLoginOS = userAgentInfo.OS;
            accountModel.FirstLoginBrowser = userAgentInfo.Browser;
            accountModel.FirstLoginProvince = wanNetIpInfo.Province;
            accountModel.FirstLoginCity = wanNetIpInfo.City;
            accountModel.FirstLoginIp = ip;
            accountModel.FirstLoginTime = dateTime;
        }

        accountModel.LastLoginTenantId = tenantModel.TenantId;
        accountModel.LastLoginDevice = userAgentInfo.Device;
        accountModel.LastLoginOS = userAgentInfo.OS;
        accountModel.LastLoginBrowser = userAgentInfo.Browser;
        accountModel.LastLoginProvince = wanNetIpInfo.Province;
        accountModel.LastLoginCity = wanNetIpInfo.City;
        accountModel.LastLoginIp = ip;
        accountModel.LastLoginTime = dateTime;
        await _repository.Updateable(accountModel)
            .ExecuteCommandAsync();

        // 登录
        await _user.Login(new AuthUserInfo
        {
            DeviceType = GlobalContext.DeviceType,
            DeviceId = GlobalContext.DeviceId,
            AppNo = applicationModel.AppNo,
            AppName = applicationModel.AppName,
            AccountId = accountModel.AccountId,
            AccountKey = accountModel.AccountKey,
            Mobile = accountModel.Mobile,
            NickName = accountModel.NickName,
            Avatar = accountModel.Avatar,
            TenantId = tenantModel.TenantId,
            TenantNo = tenantModel.TenantNo,
            TenantName = tenantModel.TenantName,
            TenantCode = tenantModel.TenantCode,
            UserId = tenantUserModel.UserId,
            UserKey = tenantUserModel.UserKey,
            Account = tenantUserModel.Account,
            EmployeeNo = tenantUserModel.EmployeeNo,
            EmployeeName = tenantUserModel.EmployeeName,
            DepartmentId = tenantUserModel.DepartmentId,
            DepartmentName = tenantUserModel.DepartmentName,
            IsSuperAdmin = tenantUserModel.UserType == UserTypeEnum.SuperAdmin,
            IsAdmin = tenantUserModel.UserType == UserTypeEnum.Admin,
            LastLoginDevice = accountModel.LastLoginDevice,
            LastLoginOS = accountModel.LastLoginOS,
            LastLoginBrowser = accountModel.LastLoginBrowser,
            LastLoginProvince = accountModel.LastLoginProvince,
            LastLoginCity = accountModel.LastLoginCity,
            LastLoginIp = accountModel.LastLoginIp,
            LastLoginTime = accountModel.LastLoginTime.Value
        });

        // 添加访问日志
        var visitLogModel = new VisitLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user.AccountId,
            Account = _user.Account,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            VisitType = VisitTypeEnum.Login,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            CreatedUserId = _user.UserId,
            CreatedUserName = _user.EmployeeName,
            CreatedTime = DateTime.Now,
            TenantId = _user.TenantId
        };
        visitLogModel.RecordCreate(_httpContext);
        await _httpContext.RequestServices.GetService<ISqlSugarRepository<VisitLogModel>>()
            .InsertAsync(visitLogModel);

        return new LoginOutput
        {
            Status = LoginStatusEnum.Success,
            Message = "登录成功",
            AccountKey = accountModel.AccountKey,
            NickName = accountModel.NickName,
            Avatar = accountModel.Avatar,
            TenantList =
            [
                new LoginOutput.LoginTenantOutput
                {
                    UserKey = tenantUserModel.UserKey,
                    TenantName = tenantModel.TenantName,
                    ShortName = tenantModel.ShortName,
                    SpellName = tenantModel.SpellName,
                    Edition = tenantModel.Edition,
                    LogoUrl = tenantModel.LogoUrl,
                    EmployeeNo = tenantUserModel.EmployeeNo,
                    EmployeeName = tenantUserModel.EmployeeName,
                    IdPhoto = tenantUserModel.IdPhoto,
                    DepartmentId = tenantUserModel.DepartmentId,
                    DepartmentName = tenantUserModel.DepartmentName,
                    UserType = tenantUserModel.UserType,
                    Status = tenantUserModel.Status
                }
            ]
        };
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/login")]
    [ApiInfo("登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    public async Task<LoginOutput> Login(LoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel == null)
        {
            throw new UserFriendlyException("未知的应用！");
        }

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 判断账号是否为手机号
        var isMobile = new Regex(RegexConst.Mobile).IsMatch(input.Account);

        AccountModel accountModel = null;
        TenantUserModel tenantUserModel = null;

        if (isMobile)
        {
            // 根据手机号，查询账号
            accountModel = await _repository.Queryable<AccountModel>()
                .Where(wh => wh.Mobile == input.Account)
                .SingleAsync();
        }
        else
        {
            // 根据账号或登录工号查询租户用户信息
            tenantUserModel = await _repository.Queryable<TenantUserModel>()
                .ClearFilter<IBaseTEntity>()
                .Where(wh => (!string.IsNullOrWhiteSpace(wh.Account) && wh.Account == input.Account)
                             || wh.EmployeeNo == input.Account)
                .SingleAsync();
            if (tenantUserModel != null)
            {
                // 查询账号
                accountModel = await _repository.Queryable<AccountModel>()
                    .Where(wh => wh.AccountId == tenantUserModel.AccountId)
                    .SingleAsync();
            }
        }

        if (accountModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        var dateTime = DateTime.Now;

        // 验证密码
        await VerifyPassword(accountModel, input.Password, dateTime);

        // 单租户自动登录
        var autoLogin = bool.Parse(await ConfigContext.GetConfig(ConfigConst.SingleTenantWhenAutoLogin));

        var tenantUserList = new List<LoginOutput.LoginTenantOutput>();
        if (tenantUserModel != null)
        {
            // 单租户自动登录
            if (autoLogin)
            {
                // 处理登录
                return await HandleLogin(applicationModel.Application, accountModel, tenantUserModel, dateTime);
            }

            // 查询租户
            var tenantModel = await _repository.Queryable<TenantModel>()
                .Where(wh => wh.TenantId == tenantUserModel.TenantId)
                .SingleAsync();
            tenantUserList.Add(new LoginOutput.LoginTenantOutput
            {
                UserKey = tenantUserModel.UserKey,
                TenantName = tenantModel.TenantName,
                ShortName = tenantModel.ShortName,
                SpellName = tenantModel.SpellName,
                Edition = tenantModel.Edition,
                LogoUrl = tenantModel.LogoUrl,
                EmployeeNo = tenantUserModel.EmployeeNo,
                EmployeeName = tenantUserModel.EmployeeName,
                IdPhoto = tenantUserModel.IdPhoto,
                DepartmentId = tenantUserModel.DepartmentId,
                DepartmentName = tenantUserModel.DepartmentName,
                UserType = tenantUserModel.UserType,
                Status = tenantUserModel.Status
            });
        }
        else
        {
            tenantUserList = await _repository.Queryable<TenantUserModel>()
                .InnerJoin<TenantModel>((t1, t2) => t1.TenantId == t2.TenantId)
                .ClearFilter<IBaseTEntity>()
                .Where(t1 => t1.AccountId == accountModel.AccountId)
                .Select((t1, t2) => new LoginOutput.LoginTenantOutput
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
                .ToListAsync();
        }

        if (tenantUserList.Count == 0)
        {
            throw new UserFriendlyException("账号未绑定任何租户！");
        }

        // 多个账号，或未开启单租户自动登录
        return new LoginOutput
        {
            Status = LoginStatusEnum.SelectTenant,
            Message = "请选择租户登录",
            AccountKey = accountModel.AccountKey,
            NickName = accountModel.NickName,
            Avatar = accountModel.Avatar,
            TenantList = tenantUserList
        };
    }

    /// <summary>
    /// 获取登录用户根据账号
    /// </summary>
    /// <param name="accountKey"></param>
    /// <returns></returns>
    [HttpGet("/queryLoginUserByAccount")]
    [ApiInfo("获取登录用户根据账号", HttpRequestActionEnum.Query)]
    [AllowAnonymous]
    public async Task<List<LoginOutput.LoginTenantOutput>> QueryLoginUserByAccount(
        [Required(ErrorMessage = "账号Key不能为空")] string accountKey)
    {
        return await _repository.Queryable<AccountModel>()
            .InnerJoin<TenantUserModel>((t1, t2) => t1.AccountId == t2.AccountId)
            .InnerJoin<TenantModel>((t1, t2, t3) => t2.TenantId == t3.TenantId)
            .ClearFilter<IBaseTEntity>()
            .Where(t1 => t1.AccountKey == accountKey)
            .Select((t1, t2, t3) => new LoginOutput.LoginTenantOutput
            {
                UserKey = t2.UserKey,
                TenantName = t3.TenantName,
                ShortName = t3.ShortName,
                SpellName = t3.SpellName,
                Edition = t3.Edition,
                LogoUrl = t3.LogoUrl,
                EmployeeNo = t2.EmployeeNo,
                EmployeeName = t2.EmployeeName,
                IdPhoto = t2.IdPhoto,
                DepartmentId = t2.DepartmentId,
                DepartmentName = t2.DepartmentName,
                UserType = t2.UserType,
                Status = t2.Status
            })
            .ToListAsync();
    }

    /// <summary>
    /// 租户登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/tenantLogin")]
    [ApiInfo("租户登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    public async Task<LoginOutput> TenantLogin(TenantLoginInput input)
    {
        if (string.IsNullOrWhiteSpace(input.AccountKey) && string.IsNullOrWhiteSpace(input.Password))
        {
            throw new UserFriendlyException("账号Key和密码必须二选一！");
        }

        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel == null)
        {
            throw new UserFriendlyException("未知的应用！");
        }

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 查询租户用户
        var tenantUserModel = await _repository.Queryable<TenantUserModel>()
            .ClearFilter<IBaseTEntity>()
            .Where(wh => wh.UserKey == input.UserKey)
            .SingleAsync();

        if (tenantUserModel == null)
        {
            throw new UserFriendlyException("用户不存在！");
        }

        // 查询账号
        var accountModel = await _repository.Queryable<AccountModel>()
            .Where(wh => wh.AccountId == tenantUserModel.AccountId)
            .SingleAsync();

        if (accountModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        var dateTime = DateTime.Now;

        if (!string.IsNullOrWhiteSpace(input.AccountKey))
        {
            if (accountModel.AccountKey != input.AccountKey)
            {
                throw new UserFriendlyException("账号不存在！");
            }
        }
        else
        {
            // 验证密码
            await VerifyPassword(accountModel, input.Password, dateTime);
        }

        // 处理登录
        return await HandleLogin(applicationModel.Application, accountModel, tenantUserModel, dateTime);
    }

    /// <summary>
    /// 处理微信登录
    /// </summary>
    /// <param name="applicationModel"></param>
    /// <param name="weChatUserModel"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    private async Task<LoginOutput> HandleWeChatLogin(ApplicationModel applicationModel, WeChatUserModel weChatUserModel)
    {
        var dateTime = DateTime.Now;

        // 获取设备信息
        var userAgentInfo = _httpContext.RequestUserAgentInfo();
        // 获取Ip信息
        var ip = _httpContext.RemoteIpv4();
        // 获取万网信息
        var wanNetIpInfo = await _httpContext.RemoteIpv4InfoAsync();

        weChatUserModel.LastLoginDevice = userAgentInfo.Device;
        weChatUserModel.LastLoginOS = userAgentInfo.OS;
        weChatUserModel.LastLoginBrowser = userAgentInfo.Browser;
        weChatUserModel.LastLoginProvince = wanNetIpInfo.Province;
        weChatUserModel.LastLoginCity = wanNetIpInfo.City;
        weChatUserModel.LastLoginIp = ip;
        weChatUserModel.LastLoginTime = dateTime;
        await _repository.Updateable(weChatUserModel)
            .ExecuteCommandAsync();

        // 判断微信用户是否授权手机号码
        if (string.IsNullOrWhiteSpace(weChatUserModel.PurePhoneNumber))
        {
            return new LoginOutput {Status = LoginStatusEnum.NotAccount, Message = "未找到微信用户信息，请先授权登录！"};
        }

        var accountModel = await _repository.Queryable<AccountModel>()
            .Where(wh => wh.Mobile == weChatUserModel.PurePhoneNumber)
            .SingleAsync();

        if (accountModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        // 处理账号绑定的问题
        accountModel.WeChatId ??= weChatUserModel.WeChatId;
        if (accountModel.WeChatId != weChatUserModel.WeChatId)
        {
            // 删除旧账号绑定
            await _repository.Updateable<AccountModel>()
                .SetColumns(e => e.WeChatId == null)
                .Where(wh => wh.WeChatId == weChatUserModel.WeChatId)
                .ExecuteCommandAsync();
            // 更新账号绑定
            accountModel.WeChatId = weChatUserModel.WeChatId;
            await _repository.Updateable(accountModel)
                .ExecuteCommandAsync();
        }

        var tenantUserList = await _repository.Queryable<TenantUserModel>()
            .ClearFilter<IBaseTEntity>()
            .Where(t1 => t1.AccountId == accountModel.AccountId)
            .ToListAsync();
        if (tenantUserList.Count == 0)
        {
            throw new UserFriendlyException("账号未绑定任何租户！");
        }

        if (tenantUserList.Count == 1)
        {
            // 单租户自动登录
            var autoLogin = bool.Parse(await ConfigContext.GetConfig(ConfigConst.SingleTenantWhenAutoLogin));

            // 单租户自动登录
            if (autoLogin)
            {
                var tenantUserModel = tenantUserList.First();
                // 处理登录
                return await HandleLogin(applicationModel, accountModel, tenantUserModel, dateTime);
            }
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

        // 多个账号，或未开启单租户自动登录
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
                .Select((t1, t2) => new LoginOutput.LoginTenantOutput
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
    /// <param name="input"></param>
    [HttpPost("/weChatLogin")]
    [ApiInfo("微信登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    public async Task<LoginOutput> WeChatLogin(WeChatLoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel == null)
        {
            throw new UserFriendlyException("未知的应用！");
        }

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 解析微信Code，获取OpenId
        var client = WechatApiClientBuilder
            .Create(new WechatApiClientOptions {AppId = applicationModel.OpenId, AppSecret = applicationModel.OpenSecret})
            .Build();
        var response = await client.ExecuteSnsJsCode2SessionAsync(new SnsJsCode2SessionRequest {JsCode = input.WeChatCode});
        if (!response.IsSuccessful())
        {
            throw new UserFriendlyException(
                $"解析Code失败，获取微信登录信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
        }

        // 根据 OpenId 获取微信用户信息
        var weChatUserModel = await _repository.Queryable<WeChatUserModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.OpenId == response.OpenId)
            .SingleAsync();
        if (weChatUserModel == null)
        {
            // 这里的 IV 和 EncryptedData 在没有授权的情况下是为空的
            if (string.IsNullOrWhiteSpace(input.IV) || string.IsNullOrWhiteSpace(input.EncryptedData))
            {
                return new LoginOutput {Status = LoginStatusEnum.AuthExpired, Message = "授权已过期，请重新授权登录！"};
            }

            // 尝试解析加密数据
            var decryptBytes = AESUtility.DecryptWithCBC(Convert.FromBase64String(response.SessionKey),
                Convert.FromBase64String(input.IV), Convert.FromBase64String(input.EncryptedData));
            var decryptStr = Encoding.Default.GetString(decryptBytes);
            var decryptData = decryptStr.ToObject<DecryptWeChatUserInfo>();
            if (decryptData == null)
            {
                throw new UserFriendlyException("解析加密用户信息失败！");
            }

            // 保存微信用户
            weChatUserModel = new WeChatUserModel
            {
                WeChatId = YitIdHelper.NextId(),
                AppId = applicationModel.AppId,
                UserType = GlobalContext.DeviceType switch
                {
                    AppEnvironmentEnum.WeChatMiniProgram => WeChatUserTypeEnum.MiniProgram,
                    AppEnvironmentEnum.WeChatOfficialAccount => WeChatUserTypeEnum.OfficialAccount,
                    AppEnvironmentEnum.WeChatServiceAccount => WeChatUserTypeEnum.ServiceAccount,
                    AppEnvironmentEnum.WeChatOpenPlatform => WeChatUserTypeEnum.OpenPlatform,
                    AppEnvironmentEnum.WorkWeChat => WeChatUserTypeEnum.WorkWeChat,
                    _ => WeChatUserTypeEnum.MiniProgram
                },
                OpenId = response.OpenId,
                UnionId = response.UnionId,
                SessionKey = response.SessionKey,
                NickName = decryptData.NickName,
                Avatar =
                    "https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132",
                Sex = decryptData.Gender,
                Country = decryptData.Country,
                Province = decryptData.Province,
                City = decryptData.City,
                Language = decryptData.Language
            };
            await _repository.Insertable(weChatUserModel)
                .ExecuteCommandAsync();
        }

        return await HandleWeChatLogin(applicationModel.Application, weChatUserModel);
    }

    /// <summary>
    /// 微信授权登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/weChatAuthLogin")]
    [ApiInfo("微信授权登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    public async Task<LoginOutput> WeChatAuthLogin(WeChatAuthLoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel == null)
        {
            throw new UserFriendlyException("未知的应用！");
        }

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 解析微信Code，获取OpenId
        var client = WechatApiClientBuilder
            .Create(new WechatApiClientOptions {AppId = applicationModel.OpenId, AppSecret = applicationModel.OpenSecret})
            .Build();
        var response = await client.ExecuteSnsJsCode2SessionAsync(new SnsJsCode2SessionRequest {JsCode = input.WeChatCode});
        if (!response.IsSuccessful())
        {
            throw new UserFriendlyException(
                $"解析Code失败，获取微信登录信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
        }

        // 根据 OpenId 获取微信用户信息
        var weChatUserModel = await _repository.Queryable<WeChatUserModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.OpenId == response.OpenId)
            .SingleAsync();
        if (weChatUserModel == null)
        {
            return new LoginOutput {Status = LoginStatusEnum.NotAccount, Message = "未找到微信用户信息，请先授权登录！"};
        }

        // 换取用户手机号
        var phoneNumberResponse = await client.ExecuteWxaBusinessGetUserPhoneNumberAsync(
            new WxaBusinessGetUserPhoneNumberRequest {AccessToken = applicationModel.WeChatAccessToken, Code = input.Code});
        if (!phoneNumberResponse.IsSuccessful())
        {
            throw new UserFriendlyException(
                $"解析Code失败，获取用户手机号失败：ErrorCode：{phoneNumberResponse.ErrorCode}。ErrorMessage：{phoneNumberResponse.ErrorMessage}");
        }

        if (weChatUserModel.PurePhoneNumber != phoneNumberResponse.PhoneInfo.PurePhoneNumber)
        {
            weChatUserModel.PurePhoneNumber = phoneNumberResponse.PhoneInfo.PurePhoneNumber;
            weChatUserModel.PhoneNumber = phoneNumberResponse.PhoneInfo.PhoneNumber;
            weChatUserModel.CountryCode = phoneNumberResponse.PhoneInfo.CountryCode;
            weChatUserModel.MobileUpdateTime = DateTime.Now;
        }

        return await HandleWeChatLogin(applicationModel.Application, weChatUserModel);
    }

    /// <summary>
    /// 尝试登录
    /// </summary>
    /// <returns></returns>
    [HttpPost("/tryLogin")]
    [ApiInfo("尝试登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    public async Task<LoginOutput> TryLogin(TryLoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel == null)
        {
            throw new UserFriendlyException("未知的应用！");
        }

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        var tenantUserModel = await _repository.Queryable<TenantUserModel>()
            .ClearFilter<IBaseTEntity>()
            .Where(wh => wh.UserKey == input.UserKey)
            .SingleAsync();

        if (tenantUserModel == null)
        {
            return new LoginOutput {Status = LoginStatusEnum.NotAccount, Message = "未找到用户信息，请先授权登录！"};
        }

        var accountModel = await _repository.Queryable<AccountModel>()
            .Where(wh => wh.AccountId == tenantUserModel.AccountId)
            .SingleAsync();

        if (accountModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        // 处理登录
        return await HandleLogin(applicationModel.Application, accountModel, tenantUserModel, DateTime.Now);
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns></returns>
    [HttpPost("/logout")]
    [ApiInfo("退出登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    public async Task Logout()
    {
        if (!string.IsNullOrWhiteSpace(_user.Account) && !string.IsNullOrWhiteSpace(_user.Mobile))
        {
            // 添加登出日志
            var visitLogModel = new VisitLogModel
            {
                RecordId = YitIdHelper.NextId(),
                AccountId = _user.AccountId,
                Account = _user.Account,
                Mobile = _user.Mobile,
                NickName = _user.NickName,
                VisitType = VisitTypeEnum.Logout,
                DepartmentId = _user.DepartmentId,
                DepartmentName = _user.DepartmentName,
                CreatedUserId = _user.UserId,
                CreatedUserName = _user.EmployeeName,
                CreatedTime = DateTime.Now,
                TenantId = _user.TenantId
            };
            visitLogModel.RecordCreate(_httpContext);
            await _httpContext.RequestServices.GetService<ISqlSugarRepository<VisitLogModel>>()
                .InsertAsync(visitLogModel);
        }

        await _user.Logout();
    }

    /// <summary>
    /// 微信客户端登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/weChatClientLogin")]
    [ApiInfo("微信客户端登录", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    public async Task<WeChatClientLoginOutput> WeChatClientLogin(WeChatClientLoginInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel == null)
        {
            throw new UserFriendlyException("未知的应用！");
        }

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        var client = WechatApiClientBuilder
            .Create(new WechatApiClientOptions {AppId = applicationModel.OpenId, AppSecret = applicationModel.OpenSecret})
            .Build();

        WeChatUserModel weChatUserModel = null;

        // 微信小程序
        if (applicationModel.AppType == AppEnvironmentEnum.WeChatMiniProgram)
        {
            // 解析微信Code，获取OpenId
            var response = await client.ExecuteSnsJsCode2SessionAsync(new SnsJsCode2SessionRequest {JsCode = input.WeChatCode});
            if (!response.IsSuccessful())
            {
                throw new UserFriendlyException(
                    $"解析Code失败，获取微信登录信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
            }

            // 根据 OpenId 获取微信用户信息
            weChatUserModel = await _repository.Queryable<WeChatUserModel>()
                .Where(wh => wh.AppId == applicationModel.AppId)
                .Where(wh => wh.OpenId == response.OpenId)
                .SingleAsync();
            if (weChatUserModel == null)
            {
                // 保存微信用户
                weChatUserModel = new WeChatUserModel
                {
                    WeChatId = YitIdHelper.NextId(),
                    AppId = applicationModel.AppId,
                    UserType = GlobalContext.DeviceType switch
                    {
                        AppEnvironmentEnum.WeChatMiniProgram => WeChatUserTypeEnum.MiniProgram,
                        AppEnvironmentEnum.WeChatOfficialAccount => WeChatUserTypeEnum.OfficialAccount,
                        AppEnvironmentEnum.WeChatServiceAccount => WeChatUserTypeEnum.ServiceAccount,
                        AppEnvironmentEnum.WeChatOpenPlatform => WeChatUserTypeEnum.OpenPlatform,
                        AppEnvironmentEnum.WorkWeChat => WeChatUserTypeEnum.WorkWeChat,
                        _ => WeChatUserTypeEnum.MiniProgram
                    },
                    OpenId = response.OpenId,
                    UnionId = response.UnionId,
                    SessionKey = response.SessionKey,
                    NickName = "微信用户",
                    Avatar =
                        "https://thirdwx.qlogo.cn/mmopen/vi_32/POgEwh4mIHO4nibH0KlMECNjjGxQUq24ZEaGT4poC6icRiccVGKSyXwibcPq4BWmiaIGuG1icwxaQX6grC9VemZoJ8rg/132",
                    Sex = GenderEnum.Unknown
                };

                // 这里的 IV 和 EncryptedData 在没有授权的情况下是为空的
                if (!string.IsNullOrWhiteSpace(input.IV) || !string.IsNullOrWhiteSpace(input.EncryptedData))
                {
                    // 尝试解析加密数据
                    var decryptBytes = AESUtility.DecryptWithCBC(Convert.FromBase64String(response.SessionKey),
                        Convert.FromBase64String(input.IV), Convert.FromBase64String(input.EncryptedData));
                    var decryptStr = Encoding.Default.GetString(decryptBytes);
                    var decryptData = decryptStr.ToObject<DecryptWeChatUserInfo>();
                    if (decryptData == null)
                    {
                        throw new UserFriendlyException("解析加密用户信息失败！");
                    }

                    weChatUserModel.NickName = decryptData.NickName;
                    weChatUserModel.Sex = decryptData.Gender;
                    weChatUserModel.Country = decryptData.Country;
                    weChatUserModel.Province = decryptData.Province;
                    weChatUserModel.City = decryptData.City;
                    weChatUserModel.Language = decryptData.Language;
                }

                await _repository.Insertable(weChatUserModel)
                    .ExecuteCommandAsync();
            }

            if (!string.IsNullOrWhiteSpace(input.Code))
            {
                // 换取用户手机号
                var phoneNumberResponse = await client.ExecuteWxaBusinessGetUserPhoneNumberAsync(
                    new WxaBusinessGetUserPhoneNumberRequest
                    {
                        AccessToken = applicationModel.WeChatAccessToken, Code = input.Code
                    });

                if (!phoneNumberResponse.IsSuccessful())
                {
                    throw new UserFriendlyException(
                        $"解析Code失败，获取用户手机号失败：ErrorCode：{phoneNumberResponse.ErrorCode}。ErrorMessage：{phoneNumberResponse.ErrorMessage}");
                }

                if (weChatUserModel.PurePhoneNumber != phoneNumberResponse.PhoneInfo.PurePhoneNumber)
                {
                    weChatUserModel.PurePhoneNumber = phoneNumberResponse.PhoneInfo.PurePhoneNumber;
                    weChatUserModel.PhoneNumber = phoneNumberResponse.PhoneInfo.PhoneNumber;
                    weChatUserModel.CountryCode = phoneNumberResponse.PhoneInfo.CountryCode;
                    weChatUserModel.MobileUpdateTime = DateTime.Now;
                }
            }
        }
        // 微信服务号
        else if (applicationModel.AppType == AppEnvironmentEnum.WeChatServiceAccount)
        {
            // 根据 Code 换取用户 AccessToken
            var tokenResponse =
                await client.ExecuteSnsOAuth2AccessTokenAsync(new SnsOAuth2AccessTokenRequest {Code = input.WeChatCode});
            if (!tokenResponse.IsSuccessful())
            {
                return new WeChatClientLoginOutput
                {
                    Status = LoginStatusEnum.AuthExpired,
                    Message =
                        $"解析Code失败，获取用户微信 AccessToken 失败：ErrorCode：{tokenResponse.ErrorCode}。ErrorMessage：{tokenResponse.ErrorMessage}"
                };
            }

            var response = await client.ExecuteSnsUserInfoAsync(new SnsUserInfoRequest
            {
                AccessToken = tokenResponse.AccessToken, OpenId = tokenResponse.OpenId
            });
            if (!response.IsSuccessful())
            {
                throw new UserFriendlyException(
                    $"获取微信用户信息失败：ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
            }

            // 根据 OpenId 获取微信用户信息
            weChatUserModel = await _repository.Queryable<WeChatUserModel>()
                .Where(wh => wh.AppId == applicationModel.AppId)
                .Where(wh => wh.OpenId == response.OpenId)
                .SingleAsync();
            if (weChatUserModel == null)
            {
                // 保存微信用户
                weChatUserModel = new WeChatUserModel
                {
                    WeChatId = YitIdHelper.NextId(),
                    AppId = applicationModel.AppId,
                    UserType = GlobalContext.DeviceType switch
                    {
                        AppEnvironmentEnum.WeChatMiniProgram => WeChatUserTypeEnum.MiniProgram,
                        AppEnvironmentEnum.WeChatOfficialAccount => WeChatUserTypeEnum.OfficialAccount,
                        AppEnvironmentEnum.WeChatServiceAccount => WeChatUserTypeEnum.ServiceAccount,
                        AppEnvironmentEnum.WeChatOpenPlatform => WeChatUserTypeEnum.OpenPlatform,
                        AppEnvironmentEnum.WorkWeChat => WeChatUserTypeEnum.WorkWeChat,
                        _ => WeChatUserTypeEnum.MiniProgram
                    },
                    OpenId = response.OpenId,
                    UnionId = response.UnionId,
                    NickName = response.Nickname,
                    Avatar = response.HeadImageUrl,
                    Sex = GenderEnum.Unknown
                };
                await _repository.Insertable(weChatUserModel)
                    .ExecuteCommandAsync();
            }
            else
            {
                weChatUserModel.NickName = response.Nickname;
                weChatUserModel.Avatar = response.HeadImageUrl;
            }
        }

        if (weChatUserModel == null)
        {
            throw new UserFriendlyException("暂不支持此类客户端！");
        }

        var dateTime = DateTime.Now;

        // 获取设备信息
        var userAgentInfo = _httpContext.RequestUserAgentInfo();
        // 获取Ip信息
        var ip = _httpContext.RemoteIpv4();
        // 获取万网信息
        var wanNetIpInfo = await _httpContext.RemoteIpv4InfoAsync();

        weChatUserModel.LastLoginDevice = userAgentInfo.Device;
        weChatUserModel.LastLoginOS = userAgentInfo.OS;
        weChatUserModel.LastLoginBrowser = userAgentInfo.Browser;
        weChatUserModel.LastLoginProvince = wanNetIpInfo.Province;
        weChatUserModel.LastLoginCity = wanNetIpInfo.City;
        weChatUserModel.LastLoginIp = ip;
        weChatUserModel.LastLoginTime = dateTime;
        await _repository.Updateable(weChatUserModel)
            .ExecuteCommandAsync();

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
            AccountId = weChatUserModel.WeChatId,
            Mobile = weChatUserModel.PurePhoneNumber,
            NickName = weChatUserModel.NickName,
            Avatar = weChatUserModel.Avatar,
            TenantId = applicationModel.Application.TenantId ?? 0,
            TenantNo = tenantMode?.TenantNo ?? applicationModel.Application.AppNo,
            TenantName = applicationModel.Application.TenantName,
            TenantCode = tenantMode?.TenantCode ?? "",
            UserId = weChatUserModel.WeChatId,
            Account = weChatUserModel.PurePhoneNumber,
            EmployeeNo = weChatUserModel.OpenId,
            EmployeeName = weChatUserModel.NickName,
            IsSuperAdmin = false,
            IsAdmin = false,
            LastLoginDevice = weChatUserModel.LastLoginDevice,
            LastLoginOS = weChatUserModel.LastLoginOS,
            LastLoginBrowser = weChatUserModel.LastLoginBrowser,
            LastLoginProvince = weChatUserModel.LastLoginProvince,
            LastLoginCity = weChatUserModel.LastLoginCity,
            LastLoginIp = weChatUserModel.LastLoginIp,
            LastLoginTime = weChatUserModel.LastLoginTime.Value,
            ButtonCodeList = [PermissionConst.ClientService]
        });

        return new WeChatClientLoginOutput
        {
            OpenId = weChatUserModel.OpenId,
            UnionId = weChatUserModel.UnionId,
            Mobile = weChatUserModel.PurePhoneNumber,
            NickName = weChatUserModel.NickName,
            Avatar = weChatUserModel.Avatar
        };
    }
}