// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 应用模板Id表Model类
/// </summary>
[SugarTable("ApplicationTemplateId", "应用模板Id表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(OpenId)}", nameof(OpenId), OrderByType.Asc)]
[SugarIndex($"UX_{{table}}_{nameof(TemplateId)}", nameof(TemplateId), OrderByType.Asc, true)]
public class ApplicationTemplateIdModel : BaseEntity
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [SugarColumn(ColumnDescription = "记录Id", IsPrimaryKey = true)]
    public long RecordId { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id")]
    public long AppId { get; set; }

    /// <summary>
    /// 应用标识
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "应用标识", Length = 50)]
    public string OpenId { get; set; }

    /// <summary>
    /// 模板类型
    /// </summary>
    [SugarColumn(ColumnDescription = "模板类型")]
    public ApplicationTemplateTypeEnum TemplateType { get; set; }

    /// <summary>
    /// 模板Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "模板Id", Length = 50)]
    public string TemplateId { get; set; }
}
