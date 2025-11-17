// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzSimPropTriggerModel"/> Quartz 简单属性触发器表Model类
/// </summary>
[SugarTable("QRTZ_SIMPROP_TRIGGERS", "Quartz 简单属性触发器表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzSimPropTriggerModel : IDatabaseEntity
{
    /// <summary>
    /// 调度器名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SCHED_NAME", ColumnDescription = "调度器名称", Length = 120, IsPrimaryKey = true)]
    public string SchedName { get; set; }

    /// <summary>
    /// 触发器名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "TRIGGER_NAME", ColumnDescription = "触发器名称", Length = 150, IsPrimaryKey = true)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 调度器分组
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "TRIGGER_GROUP", ColumnDescription = "调度器分组", Length = 150, IsPrimaryKey = true)]
    public string TriggerGroup { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "STR_PROP_1", Length = 512)]
    public string Str_Prop_1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "STR_PROP_2", Length = 512)]
    public string Str_Prop_2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "STR_PROP_3", Length = 512)]
    public string Str_Prop_3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "INT_PROP_1")]
    public int? Int_Prop_1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "INT_PROP_2")]
    public int? Int_Prop_2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "LONG_PROP_1")]
    public long? Long_Prop_1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "LONG_PROP_2")]
    public long? Long_Prop_2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "DEC_PROP_1", Length = 13, DecimalDigits = 4)]
    public decimal? Dec_Prop_1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "DEC_PROP_2", Length = 13, DecimalDigits = 4)]
    public decimal? Dec_Prop_2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "BOOL_PROP_1")]
    public bool? Bool_Prop_1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnName = "BOOL_PROP_2")]
    public bool? Bool_Prop_2 { get; set; }

    /// <summary>
    /// 时区标识
    /// </summary>
    [SugarColumn(ColumnName = "TIME_ZONE_ID", ColumnDescription = "时区标识", Length = 80)]
    public string TimeZoneId { get; set; }
}