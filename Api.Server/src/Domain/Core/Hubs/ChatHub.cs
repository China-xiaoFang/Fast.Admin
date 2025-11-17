using Fast.Center.Entity;
using Fast.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="ChatHub"/> 集线器客户端
/// </summary>
[MapHub("/hubs/chatHub")]
public class ChatHub : Hub<IChatClient>
{
    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache<AuthCCL> _authCache;

    /// <summary>
    /// 仓储
    /// </summary>
    private readonly ISqlSugarClient _repository;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="ChatHub"/> 集线器客户端
    /// </summary>
    /// <param name="authCache"><see cref="ICache"/> 缓存</param>
    /// <param name="repository"><see cref="ISqlSugarClient"/> 仓储</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public ChatHub(ICache<AuthCCL> authCache, ISqlSugarClient repository, ILogger<ChatHub> logger)
    {
        _authCache = authCache;
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 从AccessToken中获取授权用户信息
    /// </summary>
    /// <returns></returns>
    private async Task<AuthUserInfo> GetAuthUserInfo()
    {
        try
        {
            // 读取 Token
            var token = Context.GetHttpContext()
                ?.Request.Query["access_token"]
                .ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            // 读取 Token 不验证
            var claims = JwtBearerUtil.ReadJwtToken(token)
                ?.Claims;
            // 从 AccessToken 中读取 Data
            var data = claims?.FirstOrDefault(f => f.Type == "Data")!.Value;
            var payload = data?.Base64ToString()
                .ToObject<Dictionary<string, string>>();
            // 从 payload 中读取 DeviceType,DeviceId,AppNo,TenantNo,EmployeeNo
            if (payload != null
                && payload.TryGetValue(nameof(AuthUserInfo.DeviceType), out var deviceType)
                && payload.TryGetValue(nameof(AuthUserInfo.AppNo), out var appNo)
                && payload.TryGetValue(nameof(AuthUserInfo.TenantNo), out var tenantNo)
                && payload.TryGetValue(nameof(AuthUserInfo.EmployeeNo), out var employeeNo))
            {
                // 尝试获取缓存
                var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, appNo, tenantNo, deviceType, employeeNo);
                var authUserInfo = await _authCache.GetAsync<AuthUserInfo>(cacheKey);

                return authUserInfo;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocket，获取授权用户信息错误。");
            return null;
        }
    }

    /// <summary>
    /// 集线器连接
    /// Called when a new connection is established with the hub.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous connect.</returns>
    public override async Task OnConnectedAsync()
    {
        if (string.IsNullOrWhiteSpace(Context.ConnectionId))
        {
            // 关闭连接
            Context.Abort();
            return;
        }

        // 这里如果是连接，则直接连接
        await base.OnConnectedAsync();

        // 由于 WebSocket 并不会阻塞连接，所以这里连接成功之后回调
        var _hubContext = Context.GetHttpContext()
            ?.RequestServices.GetService<IHubContext<ChatHub, IChatClient>>();
        _hubContext?.Clients.Client(Context.ConnectionId)
            .ConnectSuccess();
    }

    /// <summary>
    /// 集线器断开
    /// Called when a connection with the hub is terminated.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous disconnect.</returns>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (string.IsNullOrWhiteSpace(Context.ConnectionId))
        {
            return;
        }

        var authUserInfo = await GetAuthUserInfo();

        if (authUserInfo == null)
        {
            // 关闭链接
            Context.Abort();
            return;
        }

        // 获取在线用户信息
        var tenantOnlineUserModel = await _repository.Queryable<TenantOnlineUserModel>()
            .Where(wh => wh.DeviceId == authUserInfo.DeviceId)
            .Where(wh => wh.AppNo == authUserInfo.AppNo)
            .Where(wh => wh.UserId == authUserInfo.UserId)
            .Where(wh => wh.TenantId == authUserInfo.TenantId)
            .SingleAsync();

        if (tenantOnlineUserModel != null)
        {
            // 断开连接，修改在线状态
            tenantOnlineUserModel.IsOnline = false;
            tenantOnlineUserModel.OfflineTime = DateTime.Now;
            await _repository.Updateable(tenantOnlineUserModel)
                .ExecuteCommandAsync();
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// 前端调用登录
    /// </summary>
    /// <returns></returns>
    public async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Context.ConnectionId))
        {
            return;
        }

        var authUserInfo = await GetAuthUserInfo();

        var httpContext = Context.GetHttpContext();

        var _hubContext = httpContext?.RequestServices.GetService<IHubContext<ChatHub, IChatClient>>();

        if (authUserInfo == null)
        {
            _hubContext?.Clients.Client(Context.ConnectionId)
                .LoginFail("连接失败，登录信息过期，请重新登录！");
            return;
        }

        // 判断设备信息是否和缓存中的一致
        if (GlobalContext.DeviceId != authUserInfo.DeviceId || GlobalContext.DeviceType != authUserInfo.DeviceType)
        {
            _hubContext?.Clients.Client(Context.ConnectionId)
                .LoginFail("连接失败，登录信息异常，请重新登录！");
            return;
        }

        var dateTime = DateTime.Now;

        // 获取设备信息
        var userAgentInfo = httpContext.RequestUserAgentInfo();
        // 获取万网信息
        var wanNetIpInfo = await httpContext.RemoteIpv4InfoAsync();

        // 更新连接Id
        authUserInfo.ConnectionId = Context.ConnectionId;
        authUserInfo.LastLoginDevice = userAgentInfo.Device;
        authUserInfo.LastLoginOS = userAgentInfo.OS;
        authUserInfo.LastLoginBrowser = userAgentInfo.Browser;
        authUserInfo.LastLoginProvince = wanNetIpInfo.Province;
        authUserInfo.LastLoginCity = wanNetIpInfo.City;
        authUserInfo.LastLoginIp = httpContext.RemoteIpv4();
        authUserInfo.LastLoginTime = dateTime;
        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, authUserInfo.AppNo, authUserInfo.TenantNo,
            authUserInfo.DeviceType, authUserInfo.EmployeeNo);
        // 设置缓存信息
        await _authCache.SetAsync(cacheKey, authUserInfo);

        // 获取在线用户信息
        var tenantOnlineUserModel = await _repository.Queryable<TenantOnlineUserModel>()
            .Where(wh => wh.ConnectionId == Context.ConnectionId)
            .SingleAsync();

        if (tenantOnlineUserModel == null)
        {
            tenantOnlineUserModel = new TenantOnlineUserModel
            {
                ConnectionId = Context.ConnectionId,
                DeviceType = authUserInfo.DeviceType,
                DeviceId = authUserInfo.DeviceId,
                AppNo = authUserInfo.AppNo,
                AppName = authUserInfo.AppName,
                AccountId = authUserInfo.AccountId,
                AccountKey = authUserInfo.AccountKey,
                Mobile = authUserInfo.Mobile,
                NickName = authUserInfo.NickName,
                Avatar = authUserInfo.Avatar,
                UserId = authUserInfo.UserId,
                UserKey = authUserInfo.UserKey,
                Account = authUserInfo.Account,
                EmployeeNo = authUserInfo.EmployeeNo,
                EmployeeName = authUserInfo.EmployeeName,
                DepartmentId = authUserInfo.DepartmentId,
                DepartmentName = authUserInfo.DepartmentName,
                IsSuperAdmin = authUserInfo.IsSuperAdmin,
                IsAdmin = authUserInfo.IsAdmin,
                LastLoginDevice = authUserInfo.LastLoginDevice,
                LastLoginOS = authUserInfo.LastLoginOS,
                LastLoginBrowser = authUserInfo.LastLoginBrowser,
                LastLoginProvince = authUserInfo.LastLoginProvince,
                LastLoginCity = authUserInfo.LastLoginCity,
                LastLoginIp = authUserInfo.LastLoginIp,
                LastLoginTime = authUserInfo.LastLoginTime,
                IsOnline = true,
                OfflineTime = null,
                TenantId = authUserInfo.TenantId
            };
            tenantOnlineUserModel = await _repository.Insertable(tenantOnlineUserModel)
                .ExecuteReturnEntityAsync();
        }
        else
        {
            // 修改在线状态
            tenantOnlineUserModel.LastLoginDevice = authUserInfo.LastLoginDevice;
            tenantOnlineUserModel.LastLoginOS = authUserInfo.LastLoginOS;
            tenantOnlineUserModel.LastLoginBrowser = authUserInfo.LastLoginBrowser;
            tenantOnlineUserModel.LastLoginProvince = authUserInfo.LastLoginProvince;
            tenantOnlineUserModel.LastLoginCity = authUserInfo.LastLoginCity;
            tenantOnlineUserModel.LastLoginIp = authUserInfo.LastLoginIp;
            tenantOnlineUserModel.LastLoginTime = authUserInfo.LastLoginTime;
            tenantOnlineUserModel.IsOnline = true;
            tenantOnlineUserModel.OfflineTime = null;
            await _repository.Updateable(tenantOnlineUserModel)
                .ExecuteCommandAsync();
        }

        // 单点登录
        var singleLogin = bool.Parse(await ConfigContext.GetConfig(ConfigConst.SingleLogin));

        if (singleLogin)
        {
            var connectionIdList = await _repository.Queryable<TenantOnlineUserModel>()
                .Where(wh => wh.AppNo == authUserInfo.AppNo)
                .Where(wh => wh.UserId == authUserInfo.UserId)
                .Where(wh => wh.TenantId == authUserInfo.TenantId)
                .Where(wh => wh.ConnectionId != Context.ConnectionId)
                .Select(sl => sl.ConnectionId)
                .ToListAsync();
            if (connectionIdList.Any())
            {
                // 踢下线
                await _hubContext?.Clients?.Clients(connectionIdList)
                    .ElsewhereLogin(tenantOnlineUserModel);

                // 删除所有的死连接
                await _repository.Deleteable<TenantOnlineUserModel>()
                    .Where(wh => connectionIdList.Contains(wh.ConnectionId))
                    .ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 前端调用退出登录
    /// </summary>
    /// <returns></returns>
    public async Task Logout()
    {
        if (string.IsNullOrWhiteSpace(Context.ConnectionId))
        {
            return;
        }

        var authUserInfo = await GetAuthUserInfo();

        if (authUserInfo != null)
        {
            // 删除当前连接所在的在线信息
            await _repository.Deleteable<TenantOnlineUserModel>()
                .Where(wh => wh.ConnectionId == Context.ConnectionId)
                .ExecuteCommandAsync();
        }
    }
}