// ReSharper disable once CheckNamespace

namespace Fast.Core;

/// <summary>
/// <see cref="ConfigConst"/> 配置常量
/// </summary>
[SuppressSniffer]
public class ConfigConst
{
    /// <summary>
    /// 单租户自动登录
    /// </summary>
    public const string SingleTenantWhenAutoLogin = "SINGLE_TENANT_WHEN_AUTO_LOGIN";

    /// <summary>
    /// 单点登录
    /// </summary>
    public const string SingleLogin = "SINGLE_LOGIN";

    /// <summary>
    /// 登录验证码开关
    /// </summary>
    public const string LoginCaptchaOpen = "LOGIN_CAPTCHA_OPEN";
}