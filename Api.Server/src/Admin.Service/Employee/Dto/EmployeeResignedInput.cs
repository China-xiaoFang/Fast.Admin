// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Service.Employee.Dto;

/// <summary>
/// 职员离职输入
/// </summary>
public class EmployeeResignedInput : UpdateVersionInput
{
    /// <summary>
    /// 职员Id
    /// </summary>
    [LongRequired(ErrorMessage = "职员Id不能为空")]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 离职日期
    /// </summary>
    [DateTimeRequired(ErrorMessage = "离职日期不能为空")]
    public DateTime ResignDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    [StringRequired(ErrorMessage = "离职原因不能为空")]
    public string ResignReason { get; set; }
}
