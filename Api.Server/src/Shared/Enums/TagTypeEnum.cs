// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 标签类型枚举
/// </summary>
[Flags]
[FastEnum("标签类型枚举")]
public enum TagTypeEnum : byte
{
    /// <summary>
    /// Primary
    /// </summary>
    [Description("primary")]
    Primary = 1,

    /// <summary>
    /// Success
    /// </summary>
    [Description("success")]
    Success = 2,

    /// <summary>
    /// Info
    /// </summary>
    [Description("info")]
    Info = 4,

    /// <summary>
    /// Warning
    /// </summary>
    [Description("warning")]
    Warning = 8,

    /// <summary>
    /// Danger
    /// </summary>
    [Description("danger")]
    Danger = 16
}
