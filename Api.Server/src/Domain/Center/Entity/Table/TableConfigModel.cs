// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="TableConfigModel"/> 表格配置表Model类
/// </summary>
[SugarTable("TableConfig", "表格配置表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(TableKey)}", nameof(TableKey), OrderByType.Asc, true)]
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
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "表格Key", Length = 32)]
    public string TableKey { get; set; }

    /// <summary>
    /// 表格名称
    /// </summary>
    [SugarSearchValue]
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