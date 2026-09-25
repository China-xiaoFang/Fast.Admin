// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 退款状态枚举
/// </summary>
[Flags]
[FastEnum("退款状态枚举")]
public enum RefundStatusEnum : byte
{
    /// <summary>
    /// 待审核
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("待审核")]
    PendingReview = 1,

    /// <summary>
    /// 已拒绝
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("已拒绝")]
    Rejected = 2,

    /// <summary>
    /// 退款中
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("退款中")]
    Refunding = 4,

    /// <summary>
    /// 已退款
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("已退款")]
    Refunded = 8,

    /// <summary>
    /// 部分退款
    /// </summary>
    [TagType(TagTypeEnum.Warning)]
    [Description("部分退款")]
    PartRefunded = 16,

    /// <summary>
    /// 退款失败
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("退款失败")]
    RefundFailed = 32
}
