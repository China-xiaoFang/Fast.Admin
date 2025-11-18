

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="JobLevelModel"/> 职级表Model类
/// </summary>
[SugarTable("JobLevel", "职级表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(JobLevelName)}", nameof(JobLevelName), OrderByType.Asc, true)]
public class JobLevelModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 职级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职级Id", IsPrimaryKey = true)]
    public long JobLevelId { get; set; }

    /// <summary>
    /// 职级名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "职级名称", Length = 20)]
    public string JobLevelName { get; set; }

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