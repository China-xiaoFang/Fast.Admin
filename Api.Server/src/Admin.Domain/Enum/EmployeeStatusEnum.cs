// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 职员状态枚举
/// </summary>
[Flags]
[FastEnum("职员状态枚举")]
public enum EmployeeStatusEnum : byte
{
    /// <summary>
    /// 临时工
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("临时")]
    Temporary = 1,

    /// <summary>
    /// 试用期
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("试用")]
    Probation = 2,

    /// <summary>
    /// 实习生
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("实习生")]
    Intern = 4,

    /// <summary>
    /// 外包
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("外包")]
    Outsourcing = 8,

    /// <summary>
    /// 挂职
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("挂职")]
    Secondment = 16,

    /// <summary>
    /// 正式
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("正式")]
    Formal = 32,

    /// <summary>
    /// 离职
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("离职")]
    Resigned = 64
}
