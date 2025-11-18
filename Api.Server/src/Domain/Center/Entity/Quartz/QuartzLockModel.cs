

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzLockModel"/> Quartz 调度器锁定表Model类
/// </summary>
[SugarTable("QRTZ_LOCKS", "Quartz 调度器锁定表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzLockModel : IDatabaseEntity
{
    /// <summary>
    /// 调度器名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SCHED_NAME", ColumnDescription = "调度器名称", Length = 120, IsPrimaryKey = true)]
    public string SchedName { get; set; }

    /// <summary>
    /// 锁定名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "LOCK_NAME", ColumnDescription = "锁定名称", Length = 40, IsPrimaryKey = true)]
    public string LockName { get; set; }
}