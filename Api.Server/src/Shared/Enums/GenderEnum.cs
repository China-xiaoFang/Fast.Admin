// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 性别枚举
/// </summary>
[Flags]
[FastEnum("性别枚举")]
public enum GenderEnum : byte
{
    /// <summary>
    /// 未知
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 男
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("男")]
    Man = 1,

    /// <summary>
    /// 女
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("女")]
    Woman = 2
}
