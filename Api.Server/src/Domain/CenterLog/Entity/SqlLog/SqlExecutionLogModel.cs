// ReSharper disable once CheckNamespace

namespace Fast.CenterLog.Entity;

/// <summary>
/// <see cref="SqlExecutionLogModel"/> Sql执行日志Model类
/// </summary>
[SugarTable("Sql_ExecutionLog_{year}{month}{day}", "Sql执行日志表")]
[SplitTable(SplitType.Week)]
[SugarDbType(DatabaseTypeEnum.CenterLog)]
[SugarIndex($"IX_{{table}}_{nameof(CreatedTime)}", nameof(CreatedTime), OrderByType.Asc)]
[SugarIndex($"IX_{{table}}_{nameof(TenantId)}", nameof(TenantId), OrderByType.Asc)]
public class SqlExecutionLogModel : BaseRecordEntity
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [SugarColumn(ColumnDescription = "记录Id", IsPrimaryKey = true)]
    public long RecordId { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    [SugarColumn(ColumnDescription = "账号Id")]
    public long? AccountId { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "账号", Length = 20)]
    public string Account { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)")]
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", Length = 20)]
    public string NickName { get; set; }

    /// <summary>
    /// 原始Sql
    /// </summary>
    [SugarSearchValue]
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
    /// 执行时间
    /// </summary>
    [SplitField]
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "执行时间", CreateTableFieldSort = 993)]
    public override DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", CreateTableFieldSort = 997)]
    public long? TenantId { get; set; }
}