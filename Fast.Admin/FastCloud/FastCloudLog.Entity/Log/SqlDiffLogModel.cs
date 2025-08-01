﻿// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace Fast.FastCloudLog.Entity;

/// <summary>
/// <see cref="SqlDiffLogModel"/> Sql差异日志Model类
/// </summary>
[SugarTable("SqlDiffLog_{year}{month}{day}", "Sql差异日志表")]
[SplitTable(SplitType.Week)]
[SugarDbType(DatabaseTypeEnum.FastCloudLog)]
public class SqlDiffLogModel : BaseSnowflakeRecordEntity
{
    /// <summary>
    /// 手机
    /// </summary>
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)")]
    public string Mobile { get; set; }

    /// <summary>
    /// 差异日志类型
    /// </summary>
    [SugarColumn(ColumnDescription = "差异日志类型")]
    public DiffLogTypeEnum DiffType { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    [SugarColumn(ColumnDescription = "表名称", Length = 100)]
    public string TableName { get; set; }

    /// <summary>
    /// 表描述
    /// </summary>
    [SugarColumn(ColumnDescription = "表描述", Length = 100)]
    public string TableDescription { get; set; }

    /// <summary>
    /// 差异描述
    /// </summary>
    [SugarColumn(ColumnDescription = "差异描述", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string DiffDescription { get; set; }

    /// <summary>
    /// 旧的列信息
    /// </summary>
    [SugarColumn(ColumnDescription = "旧的列信息", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<List<DiffLogColumnInfo>> BeforeColumnList { get; set; }

    /// <summary>
    /// 新的列信息
    /// </summary>
    [SugarColumn(ColumnDescription = "新的列信息", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<List<DiffLogColumnInfo>> AfterColumnList { get; set; }

    /// <summary>
    /// 执行秒数
    /// </summary>
    [SugarColumn(ColumnDescription = "执行秒数")]
    public double? ExecuteSeconds { get; set; }

    /// <summary>
    /// 原始Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "原始Sql", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string RawSql { get; set; }

    /// <summary>
    /// Sql参数
    /// </summary>
    [SugarColumn(ColumnDescription = "Sql参数", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public SugarParameter[] Parameters { get; set; }

    /// <summary>
    /// 纯Sql，参数化之后的Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "纯Sql，参数化之后的Sql", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string PureSql { get; set; }

    /// <summary>
    /// 差异时间
    /// </summary>
    [SplitField]
    [SugarSearchTime]
    public override DateTime CreatedTime { get; set; }

    [SugarColumn(IsIgnore = true)]
    public override long? DepartmentId { get; set; }

    [SugarColumn(IsIgnore = true)]
    public override string DepartmentName { get; set; }
}