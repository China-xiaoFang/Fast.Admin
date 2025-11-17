// ReSharper disable once CheckNamespace

namespace Fast.CenterLog.Entity;

/// <summary>
/// <see cref="SqlDiffLogModel"/> Sql差异日志Model类
/// </summary>
[SugarTable("Sql_DiffLog_{year}{month}{day}", "Sql差异日志表")]
[SplitTable(SplitType.Week)]
[SugarDbType(DatabaseTypeEnum.CenterLog)]
[SugarIndex($"IX_{{table}}_{nameof(CreatedTime)}", nameof(CreatedTime), OrderByType.Asc)]
[SugarIndex($"IX_{{table}}_{nameof(TenantId)}", nameof(TenantId), OrderByType.Asc)]
public class SqlDiffLogModel : BaseRecordEntity
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
    /// 差异日志类型
    /// </summary>
    [SugarColumn(ColumnDescription = "差异日志类型")]
    public DiffLogTypeEnum DiffType { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "表名称", Length = 100)]
    public string TableName { get; set; }

    /// <summary>
    /// 表描述
    /// </summary>
    [SugarColumn(ColumnDescription = "表描述", Length = 100)]
    public string TableDescription { get; set; }

    /// <summary>
    /// 业务数据
    /// </summary>
    [SugarColumn(ColumnDescription = "差异描述", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public object BusinessData { get; set; }

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
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "差异时间", CreateTableFieldSort = 993)]
    public override DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", CreateTableFieldSort = 997)]
    public long? TenantId { get; set; }
}