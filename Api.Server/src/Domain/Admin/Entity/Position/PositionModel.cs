// ReSharper disable once CheckNamespace

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="PositionModel"/> 职位表Model类
/// </summary>
[SugarTable("Position", "职位表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(PositionName)}", nameof(OrgId), OrderByType.Asc, nameof(PositionName),
    OrderByType.Asc, true)]
public class PositionModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 职位Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职位Id", IsPrimaryKey = true)]
    public long PositionId { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id")]
    public long OrgId { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "职位名称", Length = 20)]
    public string PositionName { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

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