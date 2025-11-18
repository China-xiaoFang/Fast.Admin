

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="WeChatUserTypeEnum"/> 微信用户类型枚举
/// </summary>
[Flags]
[FastEnum("微信用户类型枚举")]
public enum WeChatUserTypeEnum : byte
{
    /// <summary>
    /// 小程序
    /// </summary>
    [Description("小程序")] MiniProgram = 1,

    /// <summary>
    /// 公众号
    /// </summary>
    [Description("公众号")] OfficialAccount = 2,

    /// <summary>
    /// 服务号
    /// </summary>
    [Description("服务号")] ServiceAccount = 4,

    /// <summary>
    /// 开放平台
    /// </summary>
    [Description("开放平台")] OpenPlatform = 8,

    /// <summary>
    /// 企业微信
    /// </summary>
    [Description("企业微信")] WorkWeChat = 16
}