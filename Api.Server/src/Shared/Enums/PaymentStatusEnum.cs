// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 支付状态枚举
/// </summary>
[Flags]
[FastEnum("支付状态枚举")]
public enum PaymentStatusEnum : byte
{
    /// <summary>
    /// 待支付
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("待支付")]
    Unpaid = 1,

    /// <summary>
    /// 已支付
    /// </summary>
    [TagType(TagTypeEnum.Success)]
    [Description("已支付")]
    Paid = 2,

    /// <summary>
    /// 已关闭
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("已关闭")]
    Closed = 4
}
