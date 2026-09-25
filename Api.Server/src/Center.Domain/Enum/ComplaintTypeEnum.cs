// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 投诉类型枚举
/// </summary>
[Flags]
[FastEnum("投诉类型枚举")]
public enum ComplaintTypeEnum : byte
{
    /// <summary>
    /// 诱导行为
    /// </summary>
    [Description("诱导行为")]
    Inducement = 1,

    /// <summary>
    /// 不良信息
    /// </summary>
    [Description("不良信息")]
    HarmfulInformation = 2,

    /// <summary>
    /// 犯罪违法
    /// </summary>
    [Description("犯罪违法")]
    Illegal = 4,

    /// <summary>
    /// 欺诈
    /// </summary>
    [Description("欺诈")]
    Fraud = 8,

    /// <summary>
    /// 骚扰
    /// </summary>
    [Description("骚扰")]
    Harassment = 16,

    /// <summary>
    /// 侵权
    /// </summary>
    [Description("侵权")]
    Infringement = 32,

    /// <summary>
    /// 色情
    /// </summary>
    [Description("色情")]
    Pornography = 64,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")]
    Other = 128
}
