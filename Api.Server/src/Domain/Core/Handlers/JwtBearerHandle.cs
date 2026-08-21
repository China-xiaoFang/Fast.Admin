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

using Fast.JwtBearer;
using Fast.UnifyResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.Core;

/// <summary>
/// JWT 验证提供器
/// </summary>
public class JwtBearerHandle : IJwtBearerHandle
{
    /// <inheritdoc />
    public async Task<bool> AuthorizeHandle(AuthorizationHandlerContext context, HttpContext httpContext)
    {
        if (httpContext.User.Identity?.IsAuthenticated != true)
            return false;

        // 获取 IUser，当前请求生命周期，只会解析一次
        var _user = httpContext.RequestServices.GetService<IUser>();

        // 从 AccessToken 中读取 Data
        var data = httpContext.User.FindFirst("Data")
            ?.Value;
        if (string.IsNullOrWhiteSpace(data) || _user == null)
            return false;

        Dictionary<string, string> payload;
        try
        {
            payload = data.Base64ToString()
                .ToObject<Dictionary<string, string>>();
        }
        catch
        {
            return false;
        }

        if (payload == null)
            return false;

        // 从 payload 中读取 DeviceType,SessionId,AppNo,TenantNo,EmployeeNo
        if (payload.TryGetValue(nameof(AuthUserInfo.DeviceType), out var deviceTypeValue)
            && Enum.TryParse<AppEnvironmentEnum>(deviceTypeValue, true, out var deviceType)
            && payload.TryGetValue(nameof(AuthUserInfo.SessionId), out var sessionId)
            && payload.TryGetValue(nameof(AuthUserInfo.AppNo), out var appNo)
            && payload.TryGetValue(nameof(AuthUserInfo.TenantNo), out var tenantNo)
            && payload.TryGetValue(nameof(AuthUserInfo.EmployeeNo), out var employeeNo))
        {
            // 获取授权用户信息
            var authUserInfo = await _user.GetAuthUserInfo(deviceType, appNo, tenantNo, employeeNo, sessionId);

            if (authUserInfo == null)
                return false;

            // 判断设备信息是否和缓存中的一致
            if (GlobalContext.DeviceId != authUserInfo.DeviceId || GlobalContext.DeviceType != authUserInfo.DeviceType)
                return false;

            // 设置授权用户
            _user.SetAuthUser(authUserInfo);

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<object> AuthorizeFailHandle(AuthorizationHandlerContext context, HttpContext httpContext,
        Exception exception)
    {
        return await Task.FromResult(UnifyContext.GetRestfulResult(StatusCodes.Status401Unauthorized, false, null, "401 未经授权",
            httpContext));
    }

    /// <inheritdoc />
    public async Task<bool> PermissionHandle(AuthorizationHandlerContext context, IAuthorizationRequirement requirement,
        HttpContext httpContext)
    {
        // 获取 IUser
        var _user = httpContext.RequestServices.GetService<IUser>();

        // 超级管理员有所有的权限
        if (_user.IsSuperAdmin)
            return true;

        // 获取权限标识
        var permissionAttribute = httpContext.GetEndpoint()
            ?.Metadata.GetMetadata<PermissionAttribute>();

        if (permissionAttribute?.TagList == null || permissionAttribute.TagList.Count == 0)
            return true;

        // 输出权限标识
        httpContext.Response.Headers.TryAdd("Auth-Permission", string.Join(",", permissionAttribute.TagList));

        if (_user.ButtonCodeList == null || _user.ButtonCodeList?.Count == 0)
            return false;

        // 满足一个即可
        if (_user.ButtonCodeList.Intersect(permissionAttribute.TagList)
            .Any())
            return true;

        return await Task.FromResult(false);
    }

    /// <inheritdoc />
    public async Task<object> PermissionFailHandle(AuthorizationHandlerContext context, IAuthorizationRequirement requirement,
        HttpContext httpContext, Exception exception)
    {
        return await Task.FromResult<object>(null);
    }
}