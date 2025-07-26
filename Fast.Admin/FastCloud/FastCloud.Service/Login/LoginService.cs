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
using Fast.FastCloud.Service.Login.Dto;
using Microsoft.AspNetCore.Http;

namespace Fast.FastCloud.Service.Login;

/// <summary>
/// <see cref="LoginService"/> 登录服务
/// </summary>
public class LoginService : ILoginService, ITransientDependency
{
    private readonly IUser _user;
    private readonly HttpContext _httpContext;
    private readonly ISqlSugarClient _repository;
    private readonly ISqlSugarRepository<VisitLogModel> _visitLogRepository;

    public LoginService(IUser user, IHttpContextAccessor httpContextAccessor, ISqlSugarClient repository,
        ISqlSugarRepository<VisitLogModel> visitLogRepository)
    {
        _user = user;
        _httpContext = httpContextAccessor.HttpContext;
        _repository = repository;
        _visitLogRepository = visitLogRepository;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="deviceType"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task Login(AppEnvironmentEnum deviceType, LoginInput input)
    {
        // 获取设备类型和设备Id
        var deviceTypeStr = _httpContext.Request.Headers[HttpHeaderConst.DeviceType]
            .ToString()
            .UrlDecode();
        var deviceId = _httpContext.Request.Headers[HttpHeaderConst.DeviceId]
            .ToString()
            .UrlDecode();

        if (string.IsNullOrWhiteSpace(deviceTypeStr)
            || string.IsNullOrWhiteSpace(deviceId)
            || !deviceTypeStr.Equals(deviceType.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            throw new UserFriendlyException("非法访问！");
        }

        // 判断是否为一个有效的手机号
        var mobileRegex = new Regex(RegexConst.Mobile);
        if (!mobileRegex.IsMatch(input.Mobile))
        {
            throw new UserFriendlyException("不是一个有效的手机号码！");
        }

        var userModel = await _repository.Queryable<UserModel>()
            .Where(wh => wh.Mobile == input.Mobile)
            .SingleAsync();

        // 判断账号是否存在
        if (userModel == null)
        {
            throw new UserFriendlyException("账号不存在！");
        }

        // 验证是否为机器人
        if (userModel.UserType == UserTypeEnum.Robot)
        {
            throw new UserFriendlyException("无效账号！");
        }

        // 验证账号状态
        if (userModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("账号已被禁用！");
        }

        var dateTime = DateTime.Now;

        /*
         * 连续错误3次，锁定1分钟
         * 连续错误5次，锁定5分钟
         * 连续错误10次，锁定账号
         * 登录成功后清除锁定信息
         */
        if (input.Password != userModel.Password)
        {
            // 判断是否存在锁定时间
            if (userModel.LockEndTime != null && userModel.LockEndTime > dateTime)
            {
                var unLockTimeSpan = userModel.LockEndTime.Value - dateTime;
                throw new UserFriendlyException($"账号已被锁定，请 {unLockTimeSpan.ToDescription()} 后再重试！");
            }

            userModel.PasswordErrorTime ??= 0;
            // 错误次数+1
            userModel.PasswordErrorTime++;

            switch (userModel.PasswordErrorTime)
            {
                // 错误3次，锁定1分钟
                case 3:
                    userModel.LockStartTime ??= dateTime;
                    userModel.LockEndTime = userModel.LockStartTime.Value.AddMinutes(1);
                    await _repository.Updateable(userModel)
                        .ExecuteCommandAsync();
                    break;
                // 错误5次，锁定5分钟
                case 5:
                    userModel.LockStartTime ??= dateTime;
                    userModel.LockEndTime = dateTime.AddMinutes(5);
                    await _repository.Updateable(userModel)
                        .ExecuteCommandAsync();
                    break;
                // 判断是否连续错误10次以上
                case >= 10:
                    // 错误10此，直接禁用账号
                    userModel.Status = CommonStatusEnum.Disable;
                    await _repository.Updateable(userModel)
                        .ExecuteCommandAsync();
                    throw new UserFriendlyException("密码连续输入错误10次，账号已被禁用，请联系管理员！");
            }

            await _repository.Updateable(userModel)
                .ExecuteCommandAsync();

            throw new UserFriendlyException("密码不正确！");
        }

        // 清除锁定信息
        if (userModel.PasswordErrorTime != null)
        {
            userModel.PasswordErrorTime = null;
            userModel.LockStartTime = null;
            userModel.LockEndTime = null;
        }

        // 获取设备信息
        var userAgentInfo = _httpContext.RequestUserAgentInfo();
        // 获取Ip信息
        var ip = _httpContext.RemoteIpv4();
        // 获取万网信息
        var wanNetIpInfo = await _httpContext.RemoteIpv4InfoAsync();

        if (userModel.FirstLoginTime == null)
        {
            userModel.FirstLoginDevice = userAgentInfo.Device;
            userModel.FirstLoginOS = userAgentInfo.OS;
            userModel.FirstLoginBrowser = userAgentInfo.Browser;
            userModel.FirstLoginProvince = wanNetIpInfo.Province;
            userModel.FirstLoginCity = wanNetIpInfo.City;
            userModel.FirstLoginIp = ip;
            userModel.FirstLoginTime = dateTime;
        }

        userModel.LastLoginDevice = userAgentInfo.Device;
        userModel.LastLoginOS = userAgentInfo.OS;
        userModel.LastLoginBrowser = userAgentInfo.Browser;
        userModel.LastLoginProvince = wanNetIpInfo.Province;
        userModel.LastLoginCity = wanNetIpInfo.City;
        userModel.LastLoginIp = ip;
        userModel.LastLoginTime = dateTime;
        await _repository.Updateable(userModel)
            .ExecuteCommandAsync();

        // 登录
        await _user.Login(new AuthUserInfo
        {
            DeviceType = deviceType,
            DeviceId = GlobalContext.DeviceId,
            UserId = userModel.Id,
            Mobile = userModel.Mobile,
            NickName = userModel.NickName,
            IsAdmin = userModel.UserType == UserTypeEnum.Admin,
            LastLoginDevice = userModel.LastLoginDevice,
            LastLoginOS = userModel.LastLoginOS,
            LastLoginBrowser = userModel.LastLoginBrowser,
            LastLoginProvince = userModel.LastLoginProvince,
            LastLoginCity = userModel.LastLoginCity,
            LastLoginIp = userModel.LastLoginIp,
            LastLoginTime = userModel.LastLoginTime.Value
        });

        // 添加访问日志
        var visitLogModel = new VisitLogModel
        {
            Id = YitIdHelper.NextId(),
            CreatedUserId = userModel.Id,
            CreatedUserName = userModel.NickName,
            CreatedTime = dateTime,
            Mobile = userModel.Mobile,
            VisitType = VisitTypeEnum.Login
        };
        visitLogModel.RecordCreate(_httpContext);
        await _visitLogRepository.InsertAsync(visitLogModel);
    }
}