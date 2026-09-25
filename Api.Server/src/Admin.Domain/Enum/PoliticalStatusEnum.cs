// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 政治面貌枚举
/// </summary>
[Flags]
[FastEnum("政治面貌枚举")]
public enum PoliticalStatusEnum : byte
{
    /// <summary>
    /// 群众
    /// </summary>
    [Description("群众")]
    Masses = 1,

    /// <summary>
    /// 共青团员
    /// </summary>
    [Description("共青团员")]
    YouthLeague = 2,

    /// <summary>
    /// 中共党员
    /// </summary>
    [Description("中共党员")]
    PartyMember = 4,

    /// <summary>
    /// 民主党派
    /// </summary>
    [Description("民主党派")]
    Democratic = 8,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")]
    Other = 16
}
