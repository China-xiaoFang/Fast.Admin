// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 表格配置表Model类
/// </summary>
[SugarTable("TableConfig", "表格配置表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"UX_{{table}}_{nameof(TableKey)}", nameof(TableKey), OrderByType.Asc, true)]
public class TableConfigModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 表格Id
    /// </summary>
    [SugarColumn(ColumnDescription = "表格Id", IsPrimaryKey = true)]
    public long TableId { get; set; }

    /// <summary>
    /// 表格Key
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "表格Key", Length = 32)]
    public string TableKey { get; set; }

    /// <summary>
    /// 表格名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "表格名称", Length = 50)]
    public string TableName { get; set; }

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

    /// <summary>
    /// 表格列配置信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TableColumnConfigModel.TableId), nameof(TableId))]
    public List<TableColumnConfigModel> TableColumnConfigList { get; set; }
}
