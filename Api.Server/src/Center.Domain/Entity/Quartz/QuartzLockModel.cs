// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// Quartz 调度器锁定表Model类
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
