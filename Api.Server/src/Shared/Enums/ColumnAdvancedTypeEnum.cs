// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 列高级选项类型枚举
/// </summary>
[Flags]
[FastEnum("列高级选项类型枚举")]
public enum ColumnAdvancedTypeEnum : byte
{
    /// <summary>
    /// 字符串
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("字符串")]
    String = 1,

    /// <summary>
    /// 数字
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("数字")]
    Number = 2,

    /// <summary>
    /// Boolean
    /// </summary>
    [TagType(TagTypeEnum.Success)]
    [Description("Boolean")]
    Boolean = 4,

    /// <summary>
    /// 方法
    /// </summary>
    [TagType(TagTypeEnum.Warning)]
    [Description("方法")]
    Function = 8
}
