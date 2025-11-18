

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzSimpleTriggerModel"/> Quartz 简单触发器表Model类
/// </summary>
[SugarTable("QRTZ_SIMPLE_TRIGGERS", "Quartz 简单触发器表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzSimpleTriggerModel : IDatabaseEntity
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
    /// 重复次数（-1 表示无限）
    /// </summary>
    [SugarColumn(ColumnName = "REPEAT_COUNT", ColumnDescription = "重复次数（-1 表示无限）")]
    public int RepeatCount { get; set; }

    /// <summary>
    /// 重复间隔（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "REPEAT_INTERVAL", ColumnDescription = "重复间隔（毫秒）")]
    public long RepeatInterval { get; set; }

    /// <summary>
    /// 已触发次数
    /// </summary>
    [SugarColumn(ColumnName = "TIMES_TRIGGERED", ColumnDescription = "已触发次数")]
    public int TimesTriggered { get; set; }
}