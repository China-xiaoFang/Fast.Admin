// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 系统序号配置表Model类
/// </summary>
[SugarTable("SysSerialSetting", "系统序号配置表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(RuleType)}", nameof(RuleType), OrderByType.Asc, true)]
public class SysSerialSettingModel : IDatabaseEntity
{
    /// <summary>
    /// 序号配置Id
    /// </summary>
    [SugarColumn(ColumnDescription = "序号配置Id", IsPrimaryKey = true)]
    public long SerialSettingId { get; set; }

    /// <summary>
    /// 规则类型
    /// </summary>
    [SugarColumn(ColumnDescription = "规则类型")]
    public SysSerialRuleTypeEnum RuleType { get; set; }

    /// <summary>
    /// 最后一个序号
    /// </summary>
    [SugarColumn(ColumnDescription = "最后一个序号")]
    public long? LastSerial { get; set; }

    /// <summary>
    /// 最后一个序号编号
    /// </summary>
    [SugarColumn(ColumnDescription = "最后一个序号编号", Length = 50)]
    public string LastSerialNo { get; set; }

    /// <summary>
    /// 最后一个序号生成时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最后一个序号生成时间", CreateTableFieldSort = 993)]
    public DateTime? LastTime { get; set; }
}
