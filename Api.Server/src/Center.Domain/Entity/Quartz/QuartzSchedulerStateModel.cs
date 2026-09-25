// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// Quartz 调度器状态表Model类
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
