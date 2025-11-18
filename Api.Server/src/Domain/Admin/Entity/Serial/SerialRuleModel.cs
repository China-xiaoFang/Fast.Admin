

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="SerialRuleModel"/> 序号规则表Model类
/// </summary>
[SugarTable("SerialRule", "序号规则表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(RuleType)}", nameof(RuleType), OrderByType.Asc, true)]
public class SerialRuleModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 序号规则Id
    /// </summary>
    [SugarColumn(ColumnDescription = "序号规则Id", IsPrimaryKey = true)]
    public long SerialRuleId { get; set; }

    /// <summary>
    /// 规则类型
    /// </summary>
    [SugarColumn(ColumnDescription = "规则类型")]
    public SerialRuleTypeEnum RuleType { get; set; }

    /// <summary>
    /// 序号类型
    /// </summary>
    [SugarColumn(ColumnDescription = "序号类型")]
    public SerialTypeEnum SerialType { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    [SugarColumn(ColumnDescription = "前缀", ColumnDataType = "varchar(5)")]
    public string Prefix { get; set; }

    /// <summary>
    /// 时间类型
    /// </summary>
    [SugarColumn(ColumnDescription = "时间类型")]
    public SerialDateTypeEnum DateType { get; set; }

    /// <summary>
    /// 分隔符
    /// </summary>
    [SugarColumn(ColumnDescription = "分隔符")]
    public SerialSpacerEnum Spacer { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    [SugarColumn(ColumnDescription = "长度")]
    public int Length { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}