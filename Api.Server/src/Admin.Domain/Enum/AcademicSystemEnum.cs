// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 学制枚举
/// </summary>
[Flags]
[FastEnum("学制枚举")]
public enum AcademicSystemEnum : byte
{
    /// <summary>
    /// 两年制
    /// </summary>
    [Description("两年制")]
    TwoYears = 1,

    /// <summary>
    /// 三年制
    /// </summary>
    [Description("三年制")]
    ThreeYears = 2,

    /// <summary>
    /// 四年制
    /// </summary>
    [Description("四年制")]
    FourYears = 4,

    /// <summary>
    /// 五年制
    /// </summary>
    [Description("五年制")]
    FiveYears = 8,

    /// <summary>
    /// 六年制
    /// </summary>
    [Description("六年制")]
    SixYears = 16,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")]
    Other = 32
}
