// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzCalendarModel"/> Quartz Blob二进制触发器表Model类
/// </summary>
[SugarTable("QRTZ_BLOB_TRIGGERS", "Quartz Blob二进制触发器表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzBlobTriggerModel : IDatabaseEntity
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
    /// 触发器数据
    /// </summary>
    [SugarColumn(ColumnName = "BLOB_DATA", ColumnDescription = "触发器数据")]
    public byte[] BlobData { get; set; }
}