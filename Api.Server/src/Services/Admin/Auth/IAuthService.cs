using Fast.Admin.Service.Auth.Dto;

namespace Fast.Admin.Service.Auth;

/// <summary>
/// <see cref="IAuthService"/> 鉴权服务
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// 获取登录用户信息
    /// </summary>
    /// <returns></returns>
    Task<GetLoginUserInfoOutput> GetLoginUserInfo();
}