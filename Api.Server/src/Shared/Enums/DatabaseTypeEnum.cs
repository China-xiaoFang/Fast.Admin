// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 数据库类型枚举
/// </summary>
[Flags]
[FastEnum("数据库类型枚举")]
public enum DatabaseTypeEnum
{
    /// <summary>
    /// 系统核心库
    /// </summary>
    [Description("系统核心库")]
    Center = 1,

    /// <summary>
    /// 系统核心日志库
    /// </summary>
    [Description("系统核心日志库")]
    CenterLog = 2,

    /// <summary>
    /// 系统业务库
    /// </summary>
    [Description("系统业务库")]
    Admin = 4,

    /// <summary>
    /// 系统业务日志库
    /// </summary>
    [Description("系统业务日志库")]
    AdminLog = 8,

    /// <summary>
    /// 网关系统库
    /// </summary>
    [Description("网关系统库")]
    Gateway = 16,

    /// <summary>
    /// 部署系统库
    /// </summary>
    [Description("部署系统库")]
    Deploy = 32
}
