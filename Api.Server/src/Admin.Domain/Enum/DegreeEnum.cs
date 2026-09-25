// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 学位枚举
/// </summary>
[Flags]
[FastEnum("学位枚举")]
public enum DegreeEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("无")]
    None = 1,

    /// <summary>
    /// 学士
    /// </summary>
    [Description("学士")]
    Bachelor = 2,

    /// <summary>
    /// 硕士
    /// </summary>
    [Description("硕士")]
    Master = 4,

    /// <summary>
    /// 博士
    /// </summary>
    [Description("博士")]
    Doctor = 8,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")]
    Other = 16
}
