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

using System.Text.RegularExpressions;
using Fast.Center.Entity;
using Fast.Center.Service.Login.Dto;
using Fast.CenterLog.Entity;
using Fast.CenterLog.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace Fast.Center.Service.Login;

/// <summary>
/// <see cref="LoginService"/> 登录服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Auth, Name = "login", Order = 999)]
public class LoginService : ILoginService, ITransientDependency, IDynamicApplication
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
        var applicationModel = await _repository.Queryable<ApplicationOpenIdModel>()
            .Includes(e => e.Application)
            .Where(wh => wh.OpenId == GlobalContext.Origin)
            .SingleAsync();

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
                .Where(wh => wh.Account == input.Account || wh.LoginEmployeeNo == input.Account)
                .SingleAsync();
            if (tenantUserModel != null)
            {
                // 查询账号
                accountModel = await _repository.Queryable<AccountModel>()
                    .Where(wh => wh.Id == tenantUserModel.AccountId)
                    .SingleAsync();
            }
        }

        if (accountModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        // 验证账号状态
        if (accountModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("账号已被平台禁用！");
        }

        var dateTime = DateTime.Now;

        // 验证密码
        await VerifyPassword(accountModel, input.Password, dateTime);

        // 单租户自动登录
        var autoLogin = bool.Parse(await ConfigContext.GetConfig(ConfigConst.SingleTenantWhenAutoLogin));

        var tenantUserList = new List<LoginOutput.LoginTenantOutput>();
        if (tenantUserModel != null)
        {
            // 只有一个账号
            var tenantModel = await _repository.Queryable<TenantModel>()
                .Where(wh => wh.Id == tenantUserModel.TenantId)
                .SingleAsync();

            // 单租户自动登录
            if (autoLogin)
            {
                // 处理登录
                return await HandleLogin(applicationModel.Application, accountModel, tenantModel, tenantUserModel, dateTime);
            }

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
                DeptId = tenantUserModel.DeptId,
                DeptName = tenantUserModel.DeptName,
                UserType = tenantUserModel.UserType,
                Status = tenantUserModel.Status
            });
        }
        else
        {
            tenantUserList = await _repository.Queryable<TenantUserModel>()
                .InnerJoin<TenantModel>((t1, t2) => t1.TenantId == t2.Id)
                .ClearFilter<IBaseTEntity>()
                .Where(t1 => t1.AccountId == accountModel.Id)
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
                    DeptId = t1.DeptId,
                    DeptName = t1.DeptName,
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
        return new LoginOutput {Status = LoginStatusEnum.SelectTenant, Message = "请选择租户登录", TenantList = tenantUserList};
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
        // 查询应用信息
        var applicationModel = await _repository.Queryable<ApplicationOpenIdModel>()
            .Includes(e => e.Application)
            .Where(wh => wh.OpenId == GlobalContext.Origin)
            .SingleAsync();

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
            .Where(wh => wh.Id == tenantUserModel.AccountId)
            .SingleAsync();

        if (accountModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        // 验证账号状态
        if (accountModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("账号已被平台禁用！");
        }

        var dateTime = DateTime.Now;

        // 验证密码
        await VerifyPassword(accountModel, input.Password, dateTime);

        // 查询租户
        var tenantModel = await _repository.Queryable<TenantModel>()
            .Where(wh => wh.Id == tenantUserModel.TenantId)
            .SingleAsync();

        // 处理登录
        return await HandleLogin(applicationModel.Application, accountModel, tenantModel, tenantUserModel, dateTime);
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
    /// <param name="tenantModel"></param>
    /// <param name="tenantUserModel"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    private async Task<LoginOutput> HandleLogin(ApplicationModel applicationModel, AccountModel accountModel,
        TenantModel tenantModel, TenantUserModel tenantUserModel, DateTime dateTime)
    {
        if (tenantModel == null)
        {
            throw new UserFriendlyException("租户不存在！");
        }

        if (tenantModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("租户已被禁用！");
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
            accountModel.FirstLoginTenantId = tenantModel.Id;
            accountModel.FirstLoginDevice = userAgentInfo.Device;
            accountModel.FirstLoginOS = userAgentInfo.OS;
            accountModel.FirstLoginBrowser = userAgentInfo.Browser;
            accountModel.FirstLoginProvince = wanNetIpInfo.Province;
            accountModel.FirstLoginCity = wanNetIpInfo.City;
            accountModel.FirstLoginIp = ip;
            accountModel.FirstLoginTime = dateTime;
        }

        accountModel.LastLoginTenantId = tenantModel.Id;
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
            AccountId = accountModel.Id,
            Mobile = accountModel.Mobile,
            NickName = accountModel.NickName,
            Avatar = accountModel.Avatar,
            TenantId = tenantModel.Id,
            TenantNo = tenantModel.TenantNo,
            TenantName = tenantModel.TenantName,
            UserId = tenantUserModel.Id,
            UserKey = tenantUserModel.UserKey,
            Account = tenantUserModel.Account,
            LoginEmployeeNo = tenantUserModel.LoginEmployeeNo,
            EmployeeNo = tenantUserModel.EmployeeNo,
            EmployeeName = tenantUserModel.EmployeeName,
            DepartmentId = tenantUserModel.DeptId,
            DepartmentName = tenantUserModel.DeptName,
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
            Id = YitIdHelper.NextId(),
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
            TenantList =
            [
                new LoginOutput.LoginTenantOutput()
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
                    DeptId = tenantUserModel.DeptId,
                    DeptName = tenantUserModel.DeptName,
                    UserType = tenantUserModel.UserType,
                    Status = tenantUserModel.Status
                }
            ]
        };
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
                Id = YitIdHelper.NextId(),
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
}