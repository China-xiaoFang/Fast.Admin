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

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fast.Core;

/// <summary>
/// 全局 API 限流规则
/// </summary>
internal sealed class GlobalApiRateLimiterPolicy : ApiRateLimiterPolicy
{
    /// <summary>
    /// 全局 API 限流规则
    /// </summary>
    public GlobalApiRateLimiterPolicy(IOptions<ApiRateLimitSettingsOptions> options) : base("global-client",
        options.Value.PermitLimit.GetValueOrDefault(120), options.Value.WindowSeconds.GetValueOrDefault(60))
    {
    }

    /// <inheritdoc />
    protected override string GetPartitionKey(HttpContext httpContext)
    {
        var sessionId = GetSessionId(httpContext);
        if (!string.IsNullOrWhiteSpace(sessionId))
        {
            // 已登录请求按服务端生成的会话分区，避免依赖客户端可修改的设备Id
            return $"{PartitionPrefix}:session:{GetFingerprint(sessionId)}";
        }

        return base.GetPartitionKey(httpContext);
    }

    /// <summary>
    /// 从已通过 JWT 签名验证的授权信息中获取会话Id
    /// </summary>
    private static string GetSessionId(HttpContext httpContext)
    {
        if (httpContext.User.Identity?.IsAuthenticated != true)
            return null;

        var data = httpContext.User.FindFirst("Data")
            ?.Value;
        if (string.IsNullOrWhiteSpace(data))
            return null;

        try
        {
            var payload = data.Base64ToString()
                .ToObject<Dictionary<string, string>>();
            return payload != null && payload.TryGetValue(nameof(AuthUserInfo.SessionId), out var sessionId)
                ? sessionId?.Trim()
                : null;
        }
        catch
        {
            // 无法解析的授权信息交由后续授权处理器拒绝，此处回退到匿名分区
            return null;
        }
    }
}