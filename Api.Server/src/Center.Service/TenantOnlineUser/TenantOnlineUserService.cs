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
using Fast.Center.Domain;
using Fast.Center.Service.TenantOnlineUser.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Fast.Center.Service.TenantOnlineUser;

/// <summary>
/// 在线用户服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "tenantOnlineUser")]
public class TenantOnlineUserService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ICache<AuthCCL> _authCache;
    private readonly ISqlSugarRepository<TenantOnlineUserModel> _repository;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public TenantOnlineUserService(IUser user, ICache<AuthCCL> authCache, ISqlSugarRepository<TenantOnlineUserModel> repository,
        IHubContext<ChatHub, IChatClient> hubContext)
    {
        _user = user;
        _authCache = authCache;
        _repository = repository;
        _hubContext = hubContext;
    }

    /// <summary>
    /// 获取在线用户分页列表
    /// </summary>
    [HttpPost]
    [ApiInfo("获取在线用户分页列表", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.TenantOnlineUser.Paged)]
    public async Task<PagedResult<TenantOnlineUserModel>> QueryTenantOnlineUserPaged(QueryTenantOnlineUserPagedInput input)
    {
        return await _repository.Entities.WhereIF(input.DeviceType != null, wh => wh.DeviceType == input.DeviceType)
            .WhereIF(input.AccountId != null, wh => wh.AccountId == input.AccountId)
            .WhereIF(input.EmployeeId != null, wh => wh.EmployeeId == input.EmployeeId)
            .OrderByIF(input.IsOrderBy, ob => ob.IsOnline, OrderByType.Desc)
            .OrderByIF(input.IsOrderBy, ob => ob.LastLoginTime, OrderByType.Desc)
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 强制下线
    /// </summary>
    [HttpPost]
    [ApiInfo("强制下线", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.TenantOnlineUser.ForceOffline)]
    public async Task ForceOffline(ForceOfflineInput input)
    {
        var onlineUser = await _repository.Entities.Where(wh => wh.ConnectionId == input.ConnectionId)
            .SingleAsync();
        if (onlineUser == null)
            throw new UserFriendlyException("在线会话不存在或已下线！");

        // 同一次登录可能建立多个 SignalR 连接，按会话统一下线，不影响关闭单点登录后的其他独立会话。
        var onlineUsers = await _repository.Entities.Where(wh => wh.IsOnline)
            .WhereIF(!string.IsNullOrWhiteSpace(onlineUser.SessionId), wh => wh.SessionId == onlineUser.SessionId)
            .WhereIF(string.IsNullOrWhiteSpace(onlineUser.SessionId), wh => wh.ConnectionId == input.ConnectionId)
            .ToListAsync();
        if (onlineUsers.Count == 0)
            throw new UserFriendlyException("在线会话不存在或已下线！");

        var offlineTime = DateTime.Now;
        var connectionIds = onlineUsers.Select(sl => sl.ConnectionId)
            .Distinct()
            .ToList();
        await _hubContext.Clients.Clients(connectionIds)
            .ForceOffline(new ForceOfflineOutput
            {
                IsAdmin = _user.IsSuperAdmin || _user.IsAdmin,
                NickName = _user.NickName,
                EmployeeNo = _user.EmployeeNo,
                OfflineTime = offlineTime
            });

        if (!string.IsNullOrWhiteSpace(onlineUser.SessionId))
        {
            var cacheKey = CacheConst.GetCacheKey(CacheConst.AuthUser, onlineUser.AppNo, _user.TenantNo, onlineUser.DeviceType,
                onlineUser.EmployeeNo, onlineUser.SessionId);
            await _authCache.DelAsync(cacheKey);
        }

        onlineUsers.ForEach(item =>
        {
            item.IsOnline = false;
            item.OfflineTime = offlineTime;
        });
        await _repository.Updateable(onlineUsers)
            .ExecuteCommandAsync();
    }
}