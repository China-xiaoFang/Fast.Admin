// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 系统序号规则类型枚举
/// </summary>
[FastEnum("系统序号规则类型枚举")]
public enum SysSerialRuleTypeEnum : byte
{
    /// <summary>
    /// 应用编号
    /// </summary>
    [Description("应用编号")]
    AppNo = 1,

    /// <summary>
    /// 租户编号
    /// </summary>
    [Description("租户编号")]
    TenantNo = 2
}
