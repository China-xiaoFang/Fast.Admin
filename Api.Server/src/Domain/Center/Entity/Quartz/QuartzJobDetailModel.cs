

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="QuartzJobDetailModel"/> Quartz 作业详情表Model类
/// </summary>
[SugarTable("QRTZ_JOB_DETAILS", "Quartz 作业详情表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzJobDetailModel : IDatabaseEntity
{
    /// <summary>
    /// 调度器名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "SCHED_NAME", ColumnDescription = "调度器名称", Length = 120, IsPrimaryKey = true)]
    public string SchedName { get; set; }

    /// <summary>
    /// 作业名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "JOB_NAME", ColumnDescription = "作业名称", Length = 150, IsPrimaryKey = true)]
    public string JobName { get; set; }

    /// <summary>
    /// 作业分组
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "JOB_GROUP", ColumnDescription = "作业名称", Length = 150, IsPrimaryKey = true)]
    public string JobGroup { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    [SugarColumn(ColumnName = "DESCRIPTION", ColumnDescription = "作业描述", Length = 250)]
    public string Description { get; set; }

    /// <summary>
    /// 作业实现类的全名
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "JOB_CLASS_NAME", ColumnDescription = "作业实现类的全名", Length = 250)]
    public string JobClassName { get; set; }

    /// <summary>
    /// 是否持久化（任务完成后是否保留）
    /// </summary>
    [SugarColumn(ColumnName = "IS_DURABLE", ColumnDescription = "是否持久化（任务完成后是否保留）")]
    public bool IsDurable { get; set; }

    /// <summary>
    /// 是否禁止并发执行
    /// </summary>
    [SugarColumn(ColumnName = "IS_NONCONCURRENT", ColumnDescription = "是否禁止并发执行")]
    public bool IsNonConcurrent { get; set; }

    /// <summary>
    /// 是否更新 JobDataMap 数据
    /// </summary>
    [SugarColumn(ColumnName = "IS_UPDATE_DATA", ColumnDescription = "是否更新 JobDataMap 数据")]
    public bool IsUpdateData { get; set; }

    /// <summary>
    /// 是否请求恢复
    /// </summary>
    /// <remarks>true 表示当调度器崩溃或中断后允许重新恢复执行</remarks>
    [SugarColumn(ColumnName = "REQUESTS_RECOVERY", ColumnDescription = "是否请求恢复")]
    public bool RequestsRecovery { get; set; }

    /// <summary>
    /// 作业数据
    /// </summary>
    [SugarColumn(ColumnName = "JOB_DATA", ColumnDescription = "作业数据")]
    public byte[] JobData { get; set; }
}