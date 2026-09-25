// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 数据范围类型枚举
/// </summary>
[FastEnum("数据范围类型枚举")]
public enum DataScopeTypeEnum : byte
{
    /// <summary>
    /// 全部数据
    /// </summary>
    [Description("全部数据")]
    All = 1,

    /// <summary>
    /// 本机构及以下数据
    /// </summary>
    [Description("本机构及以下数据")]
    OrgWithChild = 2,

    /// <summary>
    /// 本部门及以下数据
    /// </summary>
    [Description("本部门及以下数据")]
    DeptWithChild = 4,

    /// <summary>
    /// 本部门数据
    /// </summary>
    [Description("本部门数据")]
    Dept = 8,

    /// <summary>
    /// 仅本人数据
    /// </summary>
    [Description("仅本人数据")]
    Self = 16,

    /// <summary>
    /// 自定义部门数据
    /// </summary>
    [Description("自定义部门数据")]
    CustomDept = 32
}
