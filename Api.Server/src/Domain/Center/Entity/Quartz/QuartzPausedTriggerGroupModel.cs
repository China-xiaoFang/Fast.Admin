// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzPausedTriggerGroupModel"/> Quartz 暂停的触发器分组表Model类
/// </summary>
[SugarTable("QRTZ_PAUSED_TRIGGER_GRPS", "Quartz 暂停的触发器分组表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzPausedTriggerGroupModel : IDatabaseEntity
{
    /// <summary>
    /// 调度器名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SCHED_NAME", ColumnDescription = "调度器名称", Length = 120, IsPrimaryKey = true)]
    public string SchedName { get; set; }

    /// <summary>
    /// 调度器分组
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "TRIGGER_GROUP", ColumnDescription = "调度器分组", Length = 150, IsPrimaryKey = true)]
    public string TriggerGroup { get; set; }
}