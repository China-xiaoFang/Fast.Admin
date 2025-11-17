// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="ButtonModel"/> 按钮表Model类
/// </summary>
[SugarTable("Button", "按钮表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(ButtonCode)}", nameof(ButtonCode), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(ButtonName)}", nameof(MenuId), OrderByType.Asc, nameof(ButtonName), OrderByType.Asc,
    true)]
public class ButtonModel : BaseEntity
{
    /// <summary>
    /// 按钮Id
    /// </summary>
    [SugarColumn(ColumnDescription = "按钮Id", IsPrimaryKey = true)]
    public long ButtonId { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [SugarColumn(ColumnDescription = "版本")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id")]
    public long AppId { get; set; }

    /// <summary>
    /// 模块Id
    /// </summary>
    [SugarColumn(ColumnDescription = "模块Id")]
    public long ModuleId { get; set; }

    /// <summary>
    /// 菜单Id
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单Id")]
    public long MenuId { get; set; }

    /// <summary>
    /// 按钮编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "按钮编码", Length = 50)]
    public string ButtonCode { get; set; }

    /// <summary>
    /// 按钮名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "按钮名称", Length = 20)]
    public string ButtonName { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否桌面端")]
    public bool HasDesktop { get; set; }

    /// <summary>
    /// 是否Web端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否Web端")]
    public bool HasWeb { get; set; }

    /// <summary>
    /// 是否移动端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否移动端")]
    public bool HasMobile { get; set; }

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
}