﻿// ------------------------------------------------------------------------
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
using Fast.Common;
using Fast.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="ChatHub"/> 集线器客户端
/// </summary>
[MapHub("/hubs/chatHub")]
public class ChatHub : Hub<IChatClient>
{
    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache _cache;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public ChatHub(ICache cache, ILogger logger)
    {
        _cache = cache;
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
            // 从 AccessToken 中读取 Mobile
            var mobile = claims?.FirstOrDefault(f => f.Type == nameof(AuthUserInfo.Mobile))
                ?.Value;
            // 获取缓存Key
            var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUserInfo, GlobalContext.DeviceType.ToString(), mobile);
            return await _cache.GetAsync<AuthUserInfo>(cacheKey);
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

        // TODO：看是存储在缓存中还是数据库中

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
                .LoginFull("连接失败，登录信息过期，请重新登录！");
            return;
        }

        // 判断设备信息是否和缓存中的一致
        if (GlobalContext.DeviceId != authUserInfo.DeviceId || GlobalContext.DeviceType != authUserInfo.DeviceType)
        {
            _hubContext?.Clients.Client(Context.ConnectionId)
                .LoginFull("连接失败，登录信息异常，请重新登录！");
            return;
        }

        // 更新连接Id
        authUserInfo.ConnectionId = Context.ConnectionId;
        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUserInfo, authUserInfo.DeviceType.ToString(), authUserInfo.Mobile);
        // 设置缓存信息
        await _cache.SetAsync(cacheKey, authUserInfo);

        // TODO：看是存储在缓存中还是数据库中
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
            // TODO：看是存储在缓存中还是数据库中
        }
    }
}