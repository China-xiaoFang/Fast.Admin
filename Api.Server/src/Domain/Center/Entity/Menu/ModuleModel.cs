// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="ModuleModel"/> 应用模块表Model类
/// </summary>
[SugarTable("Module", "模块表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(ModuleName)}", nameof(AppId), OrderByType.Asc, nameof(ModuleName), OrderByType.Asc,
    true)]
public class ModuleModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 模块Id
    /// </summary>
    [SugarColumn(ColumnDescription = "模块Id", IsPrimaryKey = true)]
    public long ModuleId { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id")]
    public long AppId { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "模块名称", Length = 20)]
    public string ModuleName { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "图标", Length = 200)]
    public string Icon { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    [SugarColumn(ColumnDescription = "颜色", Length = 20)]
    public string Color { get; set; }

    /// <summary>
    /// 查看类型
    /// </summary>
    [SugarColumn(ColumnDescription = "查看类型")]
    public ModuleViewTypeEnum ViewType { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}