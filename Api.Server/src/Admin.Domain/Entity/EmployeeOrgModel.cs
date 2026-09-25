// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 职员机构表Model类
/// </summary>
[SugarTable("EmployeeOrg", "职员机构表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
public class EmployeeOrgModel : IDatabaseEntity
{
    /// <summary>
    /// 职员Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职员Id", IsPrimaryKey = true)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id")]
    public long OrgId { get; set; }

    /// <summary>
    /// 机构名称
    /// </summary>
    [SugarColumn(ColumnDescription = "机构名称", Length = 20)]
    public string OrgName { get; set; }

    /// <summary>
    /// 机构名称
    /// </summary>
    [SugarColumn(ColumnDescription = "机构名称", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<string> OrgNames { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [SugarColumn(ColumnDescription = "部门Id", IsPrimaryKey = true)]
    public long DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [SugarColumn(ColumnDescription = "部门名称", Length = 20)]
    public string DepartmentName { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [SugarColumn(ColumnDescription = "部门名称", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<string> DepartmentNames { get; set; }

    /// <summary>
    /// 是否为主部门
    /// </summary>
    [SugarColumn(ColumnDescription = "是否为主部门")]
    public bool IsPrimary { get; set; }

    /// <summary>
    /// 职位Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职位Id", IsPrimaryKey = true)]
    public long PositionId { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    [SugarColumn(ColumnDescription = "职位名称", Length = 20)]
    public string PositionName { get; set; }

    /// <summary>
    /// 职级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职级Id")]
    public long JobLevelId { get; set; }

    /// <summary>
    /// 职级名称
    /// </summary>
    [SugarColumn(ColumnDescription = "职级名称", Length = 20)]
    public string JobLevelName { get; set; }

    /// <summary>
    /// 是否为负责人
    /// </summary>
    [SugarColumn(ColumnDescription = "是否为负责人")]
    public bool IsPrincipal { get; set; }
}
