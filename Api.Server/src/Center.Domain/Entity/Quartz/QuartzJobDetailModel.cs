// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// Quartz 作业详情表Model类
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
    [SugarColumn(ColumnName = "JOB_GROUP", ColumnDescription = "作业分组", Length = 150, IsPrimaryKey = true)]
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
    /// 是否更新 <c>JobDataMap</c> 数据
    /// </summary>
    [SugarColumn(ColumnName = "IS_UPDATE_DATA", ColumnDescription = "是否更新 JobDataMap 数据")]
    public bool IsUpdateData { get; set; }

    /// <summary>
    /// 是否请求恢复
    /// </summary>
    /// <remarks><see langword="true"/> 表示当调度器崩溃或中断后允许重新恢复执行</remarks>
    [SugarColumn(ColumnName = "REQUESTS_RECOVERY", ColumnDescription = "是否请求恢复")]
    public bool RequestsRecovery { get; set; }

    /// <summary>
    /// 作业数据
    /// </summary>
    [SugarColumn(ColumnName = "JOB_DATA", ColumnDescription = "作业数据")]
    public byte[] JobData { get; set; }
}
