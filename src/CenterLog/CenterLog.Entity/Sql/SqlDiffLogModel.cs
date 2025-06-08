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

namespace Fast.CenterLog.Entity;

/// <summary>
/// <see cref="SqlDiffLogModel"/> Sql差异日志Model类
/// </summary>
[SugarTable("SqlDiffLog{year}{month}{day}", "Sql差异日志表")]
[SplitTable(SplitType.Month)]
[SugarDbType(DatabaseTypeEnum.CenterLog)]
public class SqlDiffLogModel : BaseSnowflakeRecordEntity, IBaseTEntity
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id", IsNullable = false)]
    public long AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [SugarColumn(ColumnDescription = "应用名称", ColumnDataType = "Nvarchar(20)", IsNullable = false)]
    public string AppName { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)", IsNullable = true)]
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", ColumnDataType = "Nvarchar(20)", IsNullable = true)]
    public string NickName { get; set; }

    /// <summary>
    /// 差异日志类型
    /// </summary>
    [SugarColumn(ColumnDescription = "差异日志类型", ColumnDataType = "tinyint", IsNullable = true)]
    public DiffLogTypeEnum DiffType { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    [SugarColumn(ColumnDescription = "表名称", ColumnDataType = "Nvarchar(MAX)", IsNullable = false)]
    public string TableName { get; set; }

    /// <summary>
    /// 表描述
    /// </summary>
    [SugarColumn(ColumnDescription = "表描述", ColumnDataType = "Nvarchar(MAX)", IsNullable = true)]
    public string TableDescription { get; set; }

    /// <summary>
    /// 差异描述
    /// </summary>
    [SugarColumn(ColumnDescription = "差异描述", ColumnDataType = "Nvarchar(MAX)", IsNullable = true)]
    public string DiffDescription { get; set; }

    /// <summary>
    /// 旧的列信息
    /// </summary>
    [SugarColumn(ColumnDescription = "旧的列信息", ColumnDataType = "Nvarchar(MAX)", IsNullable = true, IsJson = true)]
    public List<List<DiffLogColumnInfo>> BeforeColumnList { get; set; }

    /// <summary>
    /// 新的列信息
    /// </summary>
    [SugarColumn(ColumnDescription = "新的列信息", ColumnDataType = "Nvarchar(MAX)", IsNullable = true, IsJson = true)]
    public List<List<DiffLogColumnInfo>> AfterColumnList { get; set; }

    /// <summary>
    /// 执行秒数
    /// </summary>
    [SugarColumn(ColumnDescription = "执行秒数", IsNullable = true)]
    public double? ExecuteSeconds { get; set; }

    /// <summary>
    /// 原始Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "原始Sql", ColumnDataType = "Nvarchar(MAX)", IsNullable = true)]
    public string RawSql { get; set; }

    /// <summary>
    /// Sql参数
    /// </summary>
    [SugarColumn(ColumnDescription = "Sql参数", ColumnDataType = "Nvarchar(MAX)", IsNullable = true, IsJson = true)]
    public SugarParameter[] Parameters { get; set; }

    /// <summary>
    /// 纯Sql，参数化之后的Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "纯Sql，参数化之后的Sql", ColumnDataType = "Nvarchar(MAX)", IsNullable = true)]
    public string PureSql { get; set; }

    /// <summary>
    /// 差异时间
    /// </summary>
    [SplitField]
    [SugarColumn(ColumnDescription = "差异时间", ColumnDataType = "datetimeoffset", IsNullable = false)]
    public DateTime DiffTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsNullable = false, CreateTableFieldSort = 997)]
    public long TenantId { get; set; }
}