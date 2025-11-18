

namespace Fast.Shared;

/// <summary>
/// <see cref="RegexConst"/> 正则表达式常量
/// </summary>
[SuppressSniffer]
public class RegexConst
{
    /// <summary>
    /// 账号
    /// </summary>
    /// <remarks>中文、英文、数字包括下划线（6 ~ 20位）</remarks>
    public const string Account = "/^[\u4E00-\u9FA5A-Za-z0-9_]{6,20}$/";

    /// <summary>
    /// 中文
    /// </summary>
    public const string Chinese = "^[\u4e00-\u9fa5]{0,}$";

    /// <summary>
    /// Http地址判断
    /// </summary>
    public const string HttpUrl = "/(http):\\/\\/([\\w.]+\\/?)\\S*/";

    /// <summary>
    /// Https地址判断
    /// </summary>
    public const string HttpsUrl = "/(https):\\/\\/([\\w.]+\\/?)\\S*/";

    /// <summary>
    /// Http或者Https地址判断
    /// </summary>
    public const string HttpOrHttpsUrl = "/(http|https):\\/\\/([\\w.]+\\/?)\\S*/";

    /// <summary>
    /// 邮箱地址判断
    /// </summary>
    public const string EmailAddress =
        "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

    /// <summary>
    /// 手机号码判断
    /// </summary>
    public const string Mobile = @"^1[3456789]\d{9}$";
}