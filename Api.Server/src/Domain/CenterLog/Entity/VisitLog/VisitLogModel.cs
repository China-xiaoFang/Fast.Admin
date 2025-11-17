// ReSharper disable once CheckNamespace

namespace Fast.CenterLog.Entity;

/// <summary>
/// <see cref="VisitLogModel"/> 访问日志Model类
/// </summary>
[SugarTable("VisitLog_{year}{month}{day}", "访问日志表")]
[SplitTable(SplitType.Month)]
[SugarDbType(DatabaseTypeEnum.CenterLog)]
[SugarIndex($"IX_{{table}}_{nameof(CreatedUserId)}", nameof(CreatedUserId), OrderByType.Asc)]
[SugarIndex($"IX_{{table}}_{nameof(CreatedTime)}", nameof(CreatedTime), OrderByType.Asc)]
[SugarIndex($"IX_{{table}}_{nameof(TenantId)}", nameof(TenantId), OrderByType.Asc)]
public class VisitLogModel : BaseRecordEntity, IBaseTEntity
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
    public long AccountId { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "账号", Length = 20)]
    public string Account { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)")]
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", Length = 20)]
    public string NickName { get; set; }

    /// <summary>
    /// 访问类型
    /// </summary>
    [SugarColumn(ColumnDescription = "访问类型")]
    public VisitTypeEnum VisitType { get; set; }

    /// <summary>
    /// 访问时间
    /// </summary>
    [SplitField]
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "访问时间", CreateTableFieldSort = 993)]
    public override DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", CreateTableFieldSort = 997)]
    public long TenantId { get; set; }
}