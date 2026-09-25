// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.AdminLog.Domain;

/// <summary>
/// 操作日志类型枚举
/// </summary>
[Flags]
[FastEnum("操作日志类型枚举")]
public enum OperateLogTypeEnum : long
{
    /// <summary>
    /// 配置管理
    /// </summary>
    [Description("配置管理")]
    Config = 1 << 0,

    /// <summary>
    /// 组织架构
    /// </summary>
    [Description("组织架构")]
    Organization = 1 << 1,

    /// <summary>
    /// 财务管理
    /// </summary>
    [Description("财务管理")]
    Finance = 1 << 2
}
