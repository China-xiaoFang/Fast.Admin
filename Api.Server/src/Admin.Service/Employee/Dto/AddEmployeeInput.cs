// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Admin.Domain;

namespace Fast.Admin.Service.Employee.Dto;

/// <summary>
/// 添加职员输入
/// </summary>
public class AddEmployeeInput
{
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
    /// 机构Id
    /// </summary>
    [LongRequired(ErrorMessage = "机构Id不能为空")]
    public long OrgId { get; set; }

    /// <summary>
    /// 机构名称
    /// </summary>
    public string OrgName { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [LongRequired(ErrorMessage = "部门Id不能为空")]
    public long DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DepartmentName { get; set; }

    /// <summary>
    /// 职位Id
    /// </summary>
    [LongRequired(ErrorMessage = "职位Id不能为空")]
    public long PositionId { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    public string PositionName { get; set; }

    /// <summary>
    /// 职级Id
    /// </summary>
    public long? JobLevelId { get; set; }

    /// <summary>
    /// 职级名称
    /// </summary>
    public string JobLevelName { get; set; }

    /// <summary>
    /// 是否为负责人
    /// </summary>
    [Required(ErrorMessage = "是否为负责人不能为空")]
    public bool IsPrincipal { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    public List<EmployeeRoleModel> RoleList { get; set; }
}
