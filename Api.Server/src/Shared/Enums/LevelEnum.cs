// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 级别枚举
/// </summary>
[Flags]
[FastEnum("级别枚举")]
public enum LevelEnum : byte
{
    /// <summary>
    /// 默认级
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("默认级")]
    Default = 0,

    /// <summary>
    /// 系统级
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("系统级")]
    System = 1,

    /// <summary>
    /// 租户级
    /// </summary>
    [TagType(TagTypeEnum.Warning)]
    [Description("租户级")]
    Tenant = 2,

    /// <summary>
    /// 自定义级
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("自定义级")]
    Custom = 32
}
