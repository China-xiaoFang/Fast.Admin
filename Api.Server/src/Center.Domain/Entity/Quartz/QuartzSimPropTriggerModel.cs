// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// Quartz 简单属性触发器表Model类
/// </summary>
[SugarTable("QRTZ_SIMPROP_TRIGGERS", "Quartz 简单属性触发器表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class QuartzSimPropTriggerModel : IDatabaseEntity
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
    /// 字符串属性1
    /// </summary>
    [SugarColumn(ColumnName = "STR_PROP_1", ColumnDescription = "字符串属性1", Length = 512)]
    public string Str_Prop_1 { get; set; }

    /// <summary>
    /// 字符串属性2
    /// </summary>
    [SugarColumn(ColumnName = "STR_PROP_2", ColumnDescription = "字符串属性2", Length = 512)]
    public string Str_Prop_2 { get; set; }

    /// <summary>
    /// 字符串属性3
    /// </summary>
    [SugarColumn(ColumnName = "STR_PROP_3", ColumnDescription = "字符串属性3", Length = 512)]
    public string Str_Prop_3 { get; set; }

    /// <summary>
    /// 整数属性1
    /// </summary>
    [SugarColumn(ColumnName = "INT_PROP_1", ColumnDescription = "整数属性1")]
    public int? Int_Prop_1 { get; set; }

    /// <summary>
    /// 整数属性2
    /// </summary>
    [SugarColumn(ColumnName = "INT_PROP_2", ColumnDescription = "整数属性2")]
    public int? Int_Prop_2 { get; set; }

    /// <summary>
    /// 长整数属性1
    /// </summary>
    [SugarColumn(ColumnName = "LONG_PROP_1", ColumnDescription = "长整数属性1")]
    public long? Long_Prop_1 { get; set; }

    /// <summary>
    /// 长整数属性2
    /// </summary>
    [SugarColumn(ColumnName = "LONG_PROP_2", ColumnDescription = "长整数属性2")]
    public long? Long_Prop_2 { get; set; }

    /// <summary>
    /// 小数属性1
    /// </summary>
    [SugarColumn(ColumnName = "DEC_PROP_1", ColumnDescription = "小数属性1", Length = 13, DecimalDigits = 4)]
    public decimal? Dec_Prop_1 { get; set; }

    /// <summary>
    /// 小数属性2
    /// </summary>
    [SugarColumn(ColumnName = "DEC_PROP_2", ColumnDescription = "小数属性2", Length = 13, DecimalDigits = 4)]
    public decimal? Dec_Prop_2 { get; set; }

    /// <summary>
    /// 布尔属性1
    /// </summary>
    [SugarColumn(ColumnName = "BOOL_PROP_1", ColumnDescription = "布尔属性1")]
    public bool? Bool_Prop_1 { get; set; }

    /// <summary>
    /// 布尔属性2
    /// </summary>
    [SugarColumn(ColumnName = "BOOL_PROP_2", ColumnDescription = "布尔属性2")]
    public bool? Bool_Prop_2 { get; set; }

    /// <summary>
    /// 时区标识
    /// </summary>
    [SugarColumn(ColumnName = "TIME_ZONE_ID", ColumnDescription = "时区标识", Length = 80)]
    public string TimeZoneId { get; set; }
}
