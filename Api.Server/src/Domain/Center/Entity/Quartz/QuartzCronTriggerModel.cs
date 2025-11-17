// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzCronTriggerModel"/> Quartz Cron表达式触发器表Model类
/// </summary>
[SugarTable("QRTZ_CRON_TRIGGERS", "Quartz Cron表达式触发器表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzCronTriggerModel : IDatabaseEntity
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
    /// Cron表达式
    /// </summary>
    [SugarColumn(ColumnName = "CRON_EXPRESSION", ColumnDescription = "Cron表达式", Length = 120)]
    public string CronExpression { get; set; }

    /// <summary>
    /// 时区标识
    /// </summary>
    [SugarColumn(ColumnName = "TIME_ZONE_ID", ColumnDescription = "时区标识", Length = 80)]
    public string TimeZoneId { get; set; }
}