

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="PasswordRecordModel"/> 密码记录表Model类
/// </summary>
[SugarTable("PasswordRecord", "密码记录表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(AccountId)}", nameof(AccountId), OrderByType.Asc)]
public class PasswordRecordModel : IDatabaseEntity
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [SugarColumn(ColumnDescription = "记录Id", IsPrimaryKey = true, IsIdentity = true)]
    public long RecordId { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    [SugarColumn(ColumnDescription = "账号Id")]
    public long AccountId { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    [SugarColumn(ColumnDescription = "操作类型")]
    public PasswordOperationTypeEnum OperationType { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [SugarColumn(ColumnDescription = "类型")]
    public PasswordTypeEnum Type { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "密码", Length = 50)]
    public string Password { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "创建时间", CreateTableFieldSort = 993)]
    public DateTime? CreatedTime { get; set; }
}