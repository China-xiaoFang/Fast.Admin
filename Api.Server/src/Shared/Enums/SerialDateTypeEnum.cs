// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 序号时间类型枚举
/// </summary>
[Flags]
[FastEnum("序号时间类型枚举")]
public enum SerialDateTypeEnum : byte
{
    /// <summary>
    /// 年(yyyy)
    /// </summary>
    [Description("年(yyyy)")]
    Year = 1,

    /// <summary>
    /// 年月(yyyyMM)
    /// </summary>
    [Description("年月(yyyyMM)")]
    Month = 2,

    /// <summary>
    /// 年月日(yyyyMMdd)
    /// </summary>
    [Description("年月日(yyyyMMdd)")]
    Day = 4,

    /// <summary>
    /// 年月日时(yyyyMMddHH)
    /// </summary>
    [Description("年月日时(yyyyMMddHH)")]
    Hour = 8
}
