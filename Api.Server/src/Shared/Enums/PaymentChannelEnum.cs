// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="PaymentChannelEnum"/> 支付渠道枚举
/// </summary>
[Flags]
[FastEnum("支付渠道枚举")]
public enum PaymentChannelEnum : byte
{
    /// <summary>
    /// 未知
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("未知")]
    None = 0,

    /// <summary>
    /// 微信
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("微信")]
    WeChat = 1,

    /// <summary>
    /// 支付宝
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("支付宝")]
    Alipay = 2,

    /// <summary>
    /// 银行卡
    /// </summary>
    [TagType(TagTypeEnum.Warning)] [Description("银行卡")]
    BankCard = 4,

    /// <summary>
    /// Apple
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("Apple")]
    ApplePay = 8,

    /// <summary>
    /// 云闪付
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("云闪付")]
    UnionPay = 16
}