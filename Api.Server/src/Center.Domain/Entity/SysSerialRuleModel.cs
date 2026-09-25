// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 系统序号规则表Model类
/// </summary>
[SugarTable("SysSerialRule", "系统序号规则表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(RuleType)}", nameof(RuleType), OrderByType.Asc, true)]
public class SysSerialRuleModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 序号规则Id
    /// </summary>
    [SugarColumn(ColumnDescription = "序号规则Id", IsPrimaryKey = true)]
    public long SerialRuleId { get; set; }

    /// <summary>
    /// 规则类型
    /// </summary>
    [SugarColumn(ColumnDescription = "规则类型")]
    public SysSerialRuleTypeEnum RuleType { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    [SugarColumn(ColumnDescription = "前缀", ColumnDataType = "varchar(5)")]
    public string Prefix { get; set; }

    /// <summary>
    /// 时间类型
    /// </summary>
    [SugarColumn(ColumnDescription = "时间类型")]
    public SerialDateTypeEnum DateType { get; set; }

    /// <summary>
    /// 分隔符
    /// </summary>
    [SugarColumn(ColumnDescription = "分隔符")]
    public SerialSpacerEnum Spacer { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    [SugarColumn(ColumnDescription = "长度")]
    public int Length { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}
