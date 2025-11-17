// ReSharper disable once CheckNamespace

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="SerialSettingModel"/> 序号配置表Model类
/// </summary>
[SugarTable("SerialSetting", "序号配置表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(RuleType)}", nameof(RuleType), OrderByType.Asc, true)]
public class SerialSettingModel : IDatabaseEntity
{
    /// <summary>
    /// 序号配置Id
    /// </summary>
    [SugarColumn(ColumnDescription = "序号配置Id", IsPrimaryKey = true)]
    public long SerialSettingId { get; set; }

    /// <summary>
    /// 规则类型
    /// </summary>
    [SugarColumn(ColumnDescription = "规则类型")]
    public SerialRuleTypeEnum RuleType { get; set; }

    /// <summary>
    /// 最后一个序号
    /// </summary>
    [SugarColumn(ColumnDescription = "最后一个序号")]
    public int? LastSerial { get; set; }

    /// <summary>
    /// 最后一个序号编号
    /// </summary>
    [SugarColumn(ColumnDescription = "最后一个序号编号", Length = 50)]
    public string LastSerialNo { get; set; }

    /// <summary>
    /// 最后一个序号生成时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最后一个序号生成时间", CreateTableFieldSort = 993)]
    public DateTime? LastTime { get; set; }
}