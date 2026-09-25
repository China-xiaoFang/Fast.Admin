// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// Quartz 简单触发器表Model类
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
