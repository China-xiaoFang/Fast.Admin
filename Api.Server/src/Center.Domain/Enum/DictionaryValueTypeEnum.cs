// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 字典值类型枚举
/// </summary>
[Flags]
[FastEnum("字典值类型枚举")]
public enum DictionaryValueTypeEnum : byte
{
    /// <summary>
    /// 字符串
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("字符串")]
    String = 1,

    /// <summary>
    /// Int
    /// </summary>
    [TagType(TagTypeEnum.Success)]
    [Description("Int")]
    Int = 2,

    /// <summary>
    /// Long
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("Long")]
    Long = 4,

    /// <summary>
    /// Boolean
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("Boolean")]
    Boolean = 8
}
