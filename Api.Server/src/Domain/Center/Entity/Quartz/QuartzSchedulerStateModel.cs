

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzSchedulerStateModel"/> Quartz 调度器状态表Model类
/// </summary>
[SugarTable("QRTZ_SCHEDULER_STATE", "Quartz 调度器状态表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzSchedulerStateModel : IDatabaseEntity
{
    /// <summary>
    /// 调度器名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SCHED_NAME", ColumnDescription = "调度器名称", Length = 120, IsPrimaryKey = true)]
    public string SchedName { get; set; }

    /// <summary>
    /// 实例名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "INSTANCE_NAME", ColumnDescription = "实例名称", Length = 200, IsPrimaryKey = true)]
    public string InstanceName { get; set; }

    /// <summary>
    /// 最后检查时间（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "LAST_CHECKIN_TIME", ColumnDescription = "最后检查时间（毫秒）")]
    public long LastCheckInTime { get; set; }

    /// <summary>
    /// 检查间隔（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "CHECKIN_INTERVAL", ColumnDescription = "检查间隔（毫秒）")]
    public long CheckInInterval { get; set; }
}