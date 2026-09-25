// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 租户类型枚举
/// </summary>
[FastEnum("租户类型枚举")]
public enum TenantTypeEnum : byte
{
    /// <summary>
    /// 系统租户
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("系统租户")]
    System = 1,

    /// <summary>
    /// 普通租户
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("普通租户")]
    Common = 2
}
