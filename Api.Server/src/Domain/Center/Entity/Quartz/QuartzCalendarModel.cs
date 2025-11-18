

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzCalendarModel"/> Quartz 日历表Model类
/// </summary>
[SugarTable("QRTZ_CALENDARS", "Quartz 日历表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzCalendarModel : IDatabaseEntity
{
    /// <summary>
    /// 调度器名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SCHED_NAME", ColumnDescription = "调度器名称", Length = 120, IsPrimaryKey = true)]
    public string SchedName { get; set; }

    /// <summary>
    /// 日历名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CALENDAR_NAME", ColumnDescription = "日历名称", Length = 200, IsPrimaryKey = true)]
    public string CalendarName { get; set; }

    /// <summary>
    /// 日历数据
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "CALENDAR", ColumnDescription = "日历数据")]
    public byte[] Calendar { get; set; }
}