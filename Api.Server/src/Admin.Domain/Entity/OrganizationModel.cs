// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 机构表Model类
/// </summary>
[SugarTable("Organization", "机构表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(OrgName)}", nameof(OrgName), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(OrgCode)}", nameof(OrgCode), OrderByType.Asc, true)]
public class OrganizationModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id", IsPrimaryKey = true)]
    public long OrgId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id")]
    public long ParentId { get; set; }

    /// <summary>
    /// 父级名称
    /// </summary>
    [SugarColumn(ColumnDescription = "父级名称", Length = 20)]
    public string ParentName { get; set; }

    /// <summary>
    /// 父级Id集合
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id集合", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<long> ParentIds { get; set; }

    /// <summary>
    /// 父级名称集合
    /// </summary>
    [SugarColumn(ColumnDescription = "父级名称集合", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<string> ParentNames { get; set; }

    /// <summary>
    /// 机构名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "机构名称", Length = 20)]
    public string OrgName { get; set; }

    /// <summary>
    /// 机构编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "机构编码", Length = 30)]
    public string OrgCode { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnDescription = "联系人", Length = 20)]
    public string Contacts { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [SugarColumn(ColumnDescription = "电话", Length = 20)]
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(ColumnDescription = "邮箱", Length = 50)]
    public string Email { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 数据公开
    /// </summary>
    /// <remarks><see cref="DataScopeTypeEnum.OrgWithChild"/> 时生效</remarks>
    [SugarColumn(ColumnDescription = "数据公开")]
    public bool DataPublic { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string Remark { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}
