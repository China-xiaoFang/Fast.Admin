// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 支付渠道枚举
/// </summary>
[Flags]
[FastEnum("支付渠道枚举")]
public enum PaymentChannelEnum : byte
{
    /// <summary>
    /// 微信
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("微信")]
    WeChat = 1,

    /// <summary>
    /// 支付宝
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("支付宝")]
    Alipay = 2,

    /// <summary>
    /// 银行卡
    /// </summary>
    [TagType(TagTypeEnum.Warning)]
    [Description("银行卡")]
    BankCard = 4,

    /// <summary>
    /// Apple
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("Apple")]
    ApplePay = 8,

    /// <summary>
    /// 云闪付
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("云闪付")]
    UnionPay = 16
}
