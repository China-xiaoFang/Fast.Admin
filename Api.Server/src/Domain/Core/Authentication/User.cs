using Fast.JwtBearer;
using Microsoft.AspNetCore.Http;


namespace Fast.Core;

/// <summary>
/// <see cref="User"/> 授权用户信息
/// </summary>
/// <remarks>作用域注册，保证当前请求管道中是唯一的，并且只会加载一次</remarks>
public sealed class User : AuthUserInfo, IUser, IScopedDependency
{
    /// <summary>
    /// 是否存在用户信息
    /// </summary>
    private bool _hasUserInfo { get; set; }

    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache<AuthCCL> _authCache;

    /// <summary>
    /// 请求上下文
    /// </summary>
    private readonly HttpContext _httpContext;

    /// <summary>
    /// <see cref="User"/> 授权用户信息
    /// </summary>
    /// <param name="authCache"><see cref="ICache"/> 缓存</param>
    /// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/> 请求上下文</param>
    /// <remarks>作用域注册，保证当前请求管道中是唯一的，并且只会加载一次</remarks>
    public User(ICache<AuthCCL> authCache, IHttpContextAccessor httpContextAccessor)
    {
        _authCache = authCache;
        _httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    /// 设置授权用户
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <param name="forceUserInfo"><see cref="bool"/> 强制覆盖用户信息，默认 <c>false</c></param>
    /// <remarks>只会赋值一次</remarks>
    public void SetAuthUser(AuthUserInfo authUserInfo, bool forceUserInfo = false)
    {
        if (_hasUserInfo && !forceUserInfo)
            return;

        if (authUserInfo == null)
        {
            throw new UnauthorizedAccessException("授权用户信息不存在！");
        }

        // 设置授权用户信息
        DeviceType = authUserInfo.DeviceType;
        DeviceId = authUserInfo.DeviceId;
        AppNo = authUserInfo.AppNo;
        AppName = authUserInfo.AppName;
        AccountId = authUserInfo.AccountId;
        AccountKey = authUserInfo.AccountKey;
        Mobile = authUserInfo.Mobile;
        NickName = authUserInfo.NickName;
        Avatar = authUserInfo.Avatar;
        TenantId = authUserInfo.TenantId;
        TenantNo = authUserInfo.TenantNo;
        TenantName = authUserInfo.TenantName;
        UserId = authUserInfo.UserId;
        UserKey = authUserInfo.UserKey;
        Account = authUserInfo.Account;
        EmployeeNo = authUserInfo.EmployeeNo;
        EmployeeName = authUserInfo.EmployeeName;
        DepartmentId = authUserInfo.DepartmentId;
        DepartmentName = authUserInfo.DepartmentName;
        IsSuperAdmin = authUserInfo.IsSuperAdmin;
        IsAdmin = authUserInfo.IsAdmin;
        LastLoginDevice = authUserInfo.LastLoginDevice;
        LastLoginOS = authUserInfo.LastLoginOS;
        LastLoginBrowser = authUserInfo.LastLoginBrowser;
        LastLoginProvince = authUserInfo.LastLoginProvince;
        LastLoginCity = authUserInfo.LastLoginCity;
        LastLoginIp = authUserInfo.LastLoginIp;
        LastLoginTime = authUserInfo.LastLoginTime;
        RoleIdList = authUserInfo.RoleIdList;
        RoleNameList = authUserInfo.RoleNameList;
        MenuCodeList = authUserInfo.MenuCodeList;
        ButtonCodeList = authUserInfo.ButtonCodeList;
        _hasUserInfo = true;
    }

    /// <summary>
    /// 从缓存中获取授权用户信息
    /// </summary>
    /// <param name="deviceType"><see cref="AppEnvironmentEnum"/> 设备类型</param>
    /// <param name="appNo"><see cref="string"/> 应用编号</param>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <param name="employeeNo"><see cref="string"/> 工号</param>
    /// <returns></returns>
    public async Task<AuthUserInfo> GetAuthUserInfo(AppEnvironmentEnum deviceType, string appNo, string tenantNo,
        string employeeNo)
    {
        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, appNo, tenantNo, deviceType.ToString(), employeeNo);

        return await _authCache.GetAsync<AuthUserInfo>(cacheKey);
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

        if (string.IsNullOrWhiteSpace(authUserInfo.TenantNo))
        {
            throw new UnauthorizedAccessException("租户信息不存在！");
        }

        if (!authUserInfo.IsSuperAdmin && !authUserInfo.IsAdmin)
        {
            if (string.IsNullOrWhiteSpace(authUserInfo.EmployeeNo))
            {
                throw new UnauthorizedAccessException("员工信息不存在！");
            }
        }

        try
        {
            // 设置授权用户信息
            SetAuthUser(authUserInfo, true);

            var payload = new Dictionary<string, string>
            {
                { nameof(DeviceType), authUserInfo.DeviceType.ToString() },
                { nameof(DeviceId), authUserInfo.DeviceId },
                { nameof(AppNo), authUserInfo.AppNo },
                { nameof(TenantNo), authUserInfo.TenantNo },
                { nameof(EmployeeNo), authUserInfo.EmployeeNo },
                { nameof(LastLoginIp), authUserInfo.LastLoginIp },
                { nameof(LastLoginTime), authUserInfo.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss") }
            };

            var data = payload.ToJsonString()
                .ToBase64();

            // 生成 AccessToken
            var accessToken = JwtBearerUtil.GenerateToken(new Dictionary<string, object> { { "Data", data } });

            // 生成 RefreshToken
            var refreshToken = JwtBearerUtil.GenerateRefreshToken(accessToken);

            // 获取缓存Key
            var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, authUserInfo.AppNo, authUserInfo.TenantNo,
                authUserInfo.DeviceType.ToString(), authUserInfo.EmployeeNo);

            // 设置缓存信息
            await _authCache.SetAsync(cacheKey, authUserInfo);

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
    /// 客户端统一登录
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    public async Task ClientLogin(AuthUserInfo authUserInfo)
    {
        if (authUserInfo == null)
        {
            throw new UnauthorizedAccessException("用户信息不存在！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.DeviceId))
        {
            throw new UnauthorizedAccessException("未知的设备！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.TenantNo))
        {
            throw new UnauthorizedAccessException("租户信息不存在！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.EmployeeNo))
        {
            throw new UnauthorizedAccessException("用户信息不存在！");
        }

        try
        {
            // 设置授权用户信息
            SetAuthUser(authUserInfo, true);

            var payload = new Dictionary<string, string>
            {
                { nameof(DeviceType), authUserInfo.DeviceType.ToString() },
                { nameof(DeviceId), authUserInfo.DeviceId },
                { nameof(AppNo), authUserInfo.AppNo },
                { nameof(TenantNo), authUserInfo.TenantNo },
                { nameof(EmployeeNo), authUserInfo.EmployeeNo },
                { nameof(LastLoginIp), authUserInfo.LastLoginIp },
                { nameof(LastLoginTime), authUserInfo.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss") }
            };

            var data = payload.ToJsonString()
                .ToBase64();

            // 生成 AccessToken
            var accessToken = JwtBearerUtil.GenerateToken(new Dictionary<string, object> { { "Data", data } });

            // 生成 RefreshToken
            var refreshToken = JwtBearerUtil.GenerateRefreshToken(accessToken);

            // 获取缓存Key
            var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, authUserInfo.AppNo, authUserInfo.TenantNo,
                authUserInfo.DeviceType.ToString(), authUserInfo.EmployeeNo);

            // 设置缓存信息
            await _authCache.SetAsync(cacheKey, authUserInfo);

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
    /// 机器人登录
    /// </summary>
    /// <remarks>非调度作业请勿使用</remarks>
    /// <returns></returns>
    public async Task<string> RobotLogin()
    {
        var payload = new Dictionary<string, string>
        {
            { nameof(DeviceType), DeviceType.ToString() },
            { nameof(DeviceId), DeviceId },
            { nameof(AppNo), "Scheduler" },
            { nameof(TenantNo), TenantNo },
            { nameof(EmployeeNo), EmployeeNo },
            { nameof(LastLoginIp), LastLoginIp },
            { nameof(LastLoginTime), LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss") }
        };

        var data = payload.ToJsonString()
            .ToBase64();

        // 生成 AccessToken，机器人使用默认1分钟过期
        var accessToken = JwtBearerUtil.GenerateToken(new Dictionary<string, object> { { "Data", data } }, 1);

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, AppNo, TenantNo, DeviceType.ToString(), EmployeeNo);

        // 设置缓存信息
        await _authCache.SetAsync(cacheKey, this);

        return accessToken;
    }

    /// <summary>
    /// 刷新登录信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    public async Task RefreshAuth(AuthUserInfo authUserInfo)
    {
        if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.Mobile))
        {
            throw new UnauthorizedAccessException("账号信息不存在！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.DeviceId))
        {
            throw new UnauthorizedAccessException("未知的设备！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.TenantNo))
        {
            throw new UnauthorizedAccessException("租户信息不存在！");
        }

        if (!authUserInfo.IsSuperAdmin && !authUserInfo.IsAdmin)
        {
            if (string.IsNullOrWhiteSpace(authUserInfo.EmployeeNo))
            {
                throw new UnauthorizedAccessException("员工信息不存在！");
            }
        }

        // 设置授权用户信息
        RoleIdList = authUserInfo.RoleIdList;
        RoleNameList = authUserInfo.RoleNameList;
        MenuCodeList = authUserInfo.MenuCodeList;
        ButtonCodeList = authUserInfo.ButtonCodeList;

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, authUserInfo.AppNo, authUserInfo.TenantNo,
            authUserInfo.DeviceType.ToString(), authUserInfo.EmployeeNo);

        // 设置缓存信息
        await _authCache.SetAsync(cacheKey, this);
    }

    /// <summary>
    /// 刷新账号信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    public async Task RefreshAccount(AuthUserInfo authUserInfo)
    {
        if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.Mobile))
        {
            throw new UnauthorizedAccessException("账号信息不存在！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.DeviceId))
        {
            throw new UnauthorizedAccessException("未知的设备！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.TenantNo))
        {
            throw new UnauthorizedAccessException("租户信息不存在！");
        }

        if (!authUserInfo.IsSuperAdmin && !authUserInfo.IsAdmin)
        {
            if (string.IsNullOrWhiteSpace(authUserInfo.EmployeeNo))
            {
                throw new UnauthorizedAccessException("员工信息不存在！");
            }
        }

        // 设置授权用户信息
        NickName = authUserInfo.NickName;
        Avatar = authUserInfo.Avatar;

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, authUserInfo.AppNo, authUserInfo.TenantNo,
            authUserInfo.DeviceType.ToString(), authUserInfo.EmployeeNo);

        // 设置缓存信息
        await _authCache.SetAsync(cacheKey, this);
    }

    /// <summary>
    /// 刷新职员信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    public async Task RefreshEmployee(AuthUserInfo authUserInfo)
    {
        if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.Mobile))
        {
            throw new UnauthorizedAccessException("账号信息不存在！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.DeviceId))
        {
            throw new UnauthorizedAccessException("未知的设备！");
        }

        if (string.IsNullOrWhiteSpace(authUserInfo.TenantNo))
        {
            throw new UnauthorizedAccessException("租户信息不存在！");
        }

        if (!authUserInfo.IsSuperAdmin && !authUserInfo.IsAdmin)
        {
            if (string.IsNullOrWhiteSpace(authUserInfo.EmployeeNo))
            {
                throw new UnauthorizedAccessException("员工信息不存在！");
            }
        }

        // 设置授权用户信息
        EmployeeName = authUserInfo.EmployeeName;
        DepartmentId = authUserInfo.DepartmentId;
        DepartmentName = authUserInfo.DepartmentName;
        RoleIdList = authUserInfo.RoleIdList;
        RoleNameList = authUserInfo.RoleNameList;

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, authUserInfo.AppNo, authUserInfo.TenantNo,
            authUserInfo.DeviceType.ToString(), authUserInfo.EmployeeNo);

        // 设置缓存信息
        await _authCache.SetAsync(cacheKey, this);
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

            try
            {
                // 读取 AccessToken，不验证
                var accessTokenIdentity = JwtBearerUtil.ReadJwtToken(accessToken);
                // 从 AccessToken 中读取 Data
                var data = accessTokenIdentity.Claims.FirstOrDefault(f => f.Type == "Data")!.Value;
                var payload = data.Base64ToString()
                    .ToObject<Dictionary<string, string>>();
                // 从 payload 中读取 DeviceType,DeviceId,AppNo,TenantNo,EmployeeNo
                if (payload.TryGetValue(nameof(DeviceType), out var deviceType)
                    && payload.TryGetValue(nameof(DeviceId), out var deviceId)
                    && payload.TryGetValue(nameof(AppNo), out var appNo)
                    && payload.TryGetValue(nameof(TenantNo), out var tenantNo)
                    && payload.TryGetValue(nameof(EmployeeNo), out var employeeNo))
                {
                    // 尝试获取缓存
                    var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, appNo, tenantNo, deviceType, employeeNo);
                    var authUserInfo = await _authCache.GetAsync<AuthUserInfo>(cacheKey);
                    if (authUserInfo != null)
                    {
                        // 判断缓存中的设备信息是否和当前 AccessToken 中的相同
                        if (authUserInfo.DeviceId == deviceId && authUserInfo.DeviceType.ToString() == deviceType)
                        {
                            // 清除缓存用户信息
                            await _authCache.DelAsync(cacheKey);
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        // 设置Swagger退出登录
        _httpContext.SignOutToSwagger();
    }
}