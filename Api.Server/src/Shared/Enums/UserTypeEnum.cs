// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 用户类型枚举
/// </summary>
[Flags]
[FastEnum("用户类型枚举")]
public enum UserTypeEnum : byte
{
    /// <summary>
    /// 超级管理员
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("超级管理员")]
    SuperAdmin = 1,

    /// <summary>
    /// 管理员
    /// </summary>
    /// <remarks>每个租户只有一个管理员账号</remarks>
    [TagType(TagTypeEnum.Warning)]
    [Description("管理员")]
    Admin = 2,

    /// <summary>
    /// 机器人
    /// </summary>
    /// <remarks>每个租户只有一个机器人账号</remarks>
    [TagType(TagTypeEnum.Primary)]
    [Description("机器人")]
    Robot = 4,

    /// <summary>
    /// 普通账号
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("普通账号")]
    None = 8
}
