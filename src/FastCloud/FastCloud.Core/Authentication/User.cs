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

using Fast.Cache;
using Fast.JwtBearer;
using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="User"/> 授权用户信息
/// </summary>
/// <remarks>作用域注册，保证当前请求管道中是唯一的，并且只会加载一次</remarks>
internal sealed class User : AuthUserInfo, IUser, IScopedDependency
{
    /// <summary>
    /// 是否存在用户信息
    /// </summary>
    private bool _hasUserInfo { get; set; }

    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache _cache;

    /// <summary>
    /// 请求上下文
    /// </summary>
    private readonly HttpContext _httpContext;

    public User(ICache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    /// 设置授权用户
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <remarks>只会赋值一次</remarks>
    public void SetAuthUser(AuthUserInfo authUserInfo)
    {
        if (_hasUserInfo)
            return;

        if (authUserInfo == null)
        {
            throw new UnauthorizedAccessException("授权用户信息不存在！");
        }

        // 设置授权用户信息
        authUserInfo.Adapt(this);
        _hasUserInfo = true;
    }

    /// <summary>
    /// 从缓存中获取授权用户信息
    /// </summary>
    /// <param name="deviceType"><see cref="AppEnvironmentEnum"/> 设备类型</param>
    /// <param name="mobile"><see cref="string"/> 手机号</param>
    /// <returns></returns>
    public async Task<AuthUserInfo> GetAuthUserInfo(AppEnvironmentEnum deviceType, string mobile)
    {
        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUserInfo, deviceType.ToString(), mobile);

        return await _cache.GetAsync<AuthUserInfo>(cacheKey);
    }

    /// <summary>
    /// 统一登录
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    public async Task Login(AuthUserInfo authUserInfo)
    {
        if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.Mobile))
        {
            throw new UnauthorizedAccessException("账号信息不存在！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.DeviceId))
        {
            throw new UnauthorizedAccessException("未知的设备！");
        }

        try
        {
            // 设置授权用户信息
            authUserInfo.Adapt(this);

            // 生成 AccessToken
            var accessToken = JwtBearerUtil.GenerateToken(new Dictionary<string, object>
            {
                {nameof(DeviceType), authUserInfo.DeviceType.ToString()},
                {nameof(DeviceId), authUserInfo.DeviceId},
                {nameof(Mobile), authUserInfo.Mobile},
                {nameof(NickName), authUserInfo.NickName},
                {nameof(LastLoginIp), authUserInfo.LastLoginIp},
                {nameof(LastLoginTime), authUserInfo.LastLoginTime}
            });

            // 生成 RefreshToken
            var refreshToken = JwtBearerUtil.GenerateRefreshToken(accessToken);

            // 获取缓存Key
            var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUserInfo,
                authUserInfo.DeviceType.ToString(),
                authUserInfo.Mobile);

            // 设置缓存信息
            await _cache.SetAsync(cacheKey, authUserInfo);

            // 设置 AccessToken
            _httpContext.Response.Headers["access-token"] = accessToken;

            // 设置 RefreshToken
            _httpContext.Response.Headers["x-access-token"] = refreshToken;

            // 设置Swagger自动登录
            _httpContext.SignInToSwagger(accessToken);
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException($"401 登录鉴权失败：{ex.Message}");
        }
    }

    /// <summary>
    /// 刷新登录信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    public async Task Refresh(AuthUserInfo authUserInfo)
    {
        // 设置授权用户信息
        authUserInfo.Adapt(this);

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUserInfo, authUserInfo.DeviceType.ToString(), authUserInfo.Mobile);

        // 设置缓存信息
        await _cache.SetAsync(cacheKey, authUserInfo);
    }

    /// <summary>
    /// 统一退出登录
    /// </summary>
    /// <returns></returns>
    public async Task Logout()
    {
        /*
         * 首先确定，退出登录有两种情况，
         *  1.正常情况，点击退出登录，这个时候的Token是存在的，且没有过期的。
         *  2.401的情况下，系统调用退出登录的接口，这个时候虽然存在Token，但是Token肯定是过期的。
         */

        // 这里直接从请求头中获取 AccessToken
        var accessToken = JwtBearerUtil.GetJwtBearerToken(_httpContext);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            // 标记过期
            JwtBearerUtil.SetExpiredToken(_httpContext, accessToken);

            // 读取 AccessToken，不验证
            var accessTokenIdentity = JwtBearerUtil.ReadJwtToken(accessToken);
            // 从 AccessToken 中读取 DeviceType,DeviceId,Mobile
            var deviceType = accessTokenIdentity.Claims.FirstOrDefault(f => f.Type == nameof(DeviceType))!.Value;
            var deviceId = accessTokenIdentity.Claims.FirstOrDefault(f => f.Type == nameof(DeviceId))!.Value;
            var mobile = accessTokenIdentity.Claims.FirstOrDefault(f => f.Type == nameof(Mobile))!.Value;

            // 尝试获取缓存
            var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUserInfo, deviceType, mobile);
            var authUserInfo = await _cache.GetAsync<AuthUserInfo>(cacheKey);
            if (authUserInfo != null)
            {
                // 判断缓存中的设备信息是否和当前 AccessToken 中的相同
                if (authUserInfo.DeviceId == deviceId && authUserInfo.DeviceType.ToString() == deviceType)
                {
                    // 清除缓存用户信息
                    await _cache.DelAsync(cacheKey);
                }
            }
        }

        // 设置Swagger退出登录
        _httpContext.SignOutToSwagger();
    }
}