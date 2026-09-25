// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Admin.Domain;

namespace Fast.Admin.Service.Employee.Dto;

/// <summary>
/// 编辑职员输入
/// </summary>
public class EditEmployeeInput : UpdateVersionInput
{
    /// <summary>
    /// 职员Id
    /// </summary>
    [LongRequired(ErrorMessage = "职员Id不能为空")]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [StringRequired(ErrorMessage = "姓名不能为空")]
    public string EmployeeName { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [StringRequired(ErrorMessage = "手机不能为空")]
    [RegularExpression(RegexConst.Mobile, ErrorMessage = "手机格式不正确")]
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [StringRequired(ErrorMessage = "邮箱不能为空")]
    [RegularExpression(RegexConst.EmailAddress, ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [EnumRequired(ErrorMessage = "性别不能为空", AllowZero = true)]
    public GenderEnum Sex { get; set; }

    /// <summary>
    /// 证件照
    /// </summary>
    public string IdPhoto { get; set; }

    /// <summary>
    /// 入职日期
    /// </summary>
    [DateTimeRequired(ErrorMessage = "入职日期不能为空")]
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 机构信息
    /// </summary>
    public List<EmployeeOrgModel> OrgList { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    public List<EmployeeRoleModel> RoleList { get; set; }
}
