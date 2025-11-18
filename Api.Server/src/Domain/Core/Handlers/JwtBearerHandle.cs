using Fast.JwtBearer;
using Fast.UnifyResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Fast.Core;

/// <summary>
/// <see cref="JwtBearerHandle"/> Jwt验证提供器
/// </summary>
public class JwtBearerHandle : IJwtBearerHandle
{
    /// <summary>授权处理</summary>
    /// <remarks>这里已经判断了 Token 是否有效，并且已经处理了自动刷新 Token。只需要处理其余的逻辑即可。如果返回 false或抛出异常，搭配 AuthorizeFailHandle 则抛出 HttpStatusCode = 401 状态码，否则走默认处理 AuthorizationHandlerContext.Fail() 会返回 HttpStatusCode = 403 状态码</remarks>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext" /></param>
    /// <param name="httpContext"><see cref="T:Microsoft.AspNetCore.Http.HttpContext" /></param>
    /// <returns><see cref="T:System.Boolean" /></returns>
    public async Task<bool> AuthorizeHandle(AuthorizationHandlerContext context, HttpContext httpContext)
    {
        if (httpContext.User.Identity?.IsAuthenticated == false)
            return false;

        // 获取 IUser，当前请求生命周期，只会解析一次
        var _user = httpContext.RequestServices.GetService<IUser>();

        // 从 AccessToken 中读取 Data
        var data = httpContext.User.Claims.FirstOrDefault(f => f.Type == "Data")!.Value;
        var payload = data.Base64ToString()
            .ToObject<Dictionary<string, string>>();
        // 从 payload 中读取 DeviceType,AppNo,TenantNo,EmployeeNo
        if (payload.TryGetValue(nameof(AuthUserInfo.DeviceType), out var deviceType)
            && payload.TryGetValue(nameof(AuthUserInfo.AppNo), out var appNo)
            && payload.TryGetValue(nameof(AuthUserInfo.TenantNo), out var tenantNo)
            && payload.TryGetValue(nameof(AuthUserInfo.EmployeeNo), out var employeeNo))
        {
            // 获取授权用户信息
            var authUserInfo =
                await _user.GetAuthUserInfo(Enum.Parse<AppEnvironmentEnum>(deviceType, true), appNo, tenantNo,
                    employeeNo);

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

    /// <summary>授权失败处理</summary>
    /// <remarks>如果返回 null，则走默认处理 AuthorizationHandlerContext.Fail()</remarks>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext" /></param>
    /// <param name="httpContext"><see cref="T:Microsoft.AspNetCore.Http.HttpContext" /></param>
    /// <param name="exception"><see cref="T:System.Exception" /></param>
    /// <returns></returns>
    public async Task<object> AuthorizeFailHandle(AuthorizationHandlerContext context, HttpContext httpContext,
        Exception exception)
    {
        return await Task.FromResult(UnifyContext.GetRestfulResult(StatusCodes.Status401Unauthorized, false, null,
            exception?.Message ?? "401 未经授权", httpContext));
    }

    /// <summary>权限判断处理</summary>
    /// <remarks>这里只需要判断你的权限逻辑即可，如果返回 false或抛出异常 则抛出 HttpStatusCode = 403 状态码</remarks>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext" /></param>
    /// <param name="requirement"><see cref="T:Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" /></param>
    /// <param name="httpContext"><see cref="T:Microsoft.AspNetCore.Http.HttpContext" /></param>
    /// <returns></returns>
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

    /// <summary>权限判断失败处理</summary>
    /// <remarks>如果返回 null，则走默认处理 AuthorizationHandlerContext.Fail()</remarks>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext" /></param>
    /// <param name="requirement"><see cref="T:Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" /></param>
    /// <param name="httpContext"><see cref="T:Microsoft.AspNetCore.Http.HttpContext" /></param>
    /// <param name="exception"><see cref="T:System.Exception" /></param>
    /// <returns></returns>
    public async Task<object> PermissionFailHandle(AuthorizationHandlerContext context,
        IAuthorizationRequirement requirement,
        HttpContext httpContext, Exception exception)
    {
        return await Task.FromResult<object>(null);
    }
}