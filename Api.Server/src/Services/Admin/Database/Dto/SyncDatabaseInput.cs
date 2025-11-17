namespace Fast.Admin.Service.Database.Dto;

/// <summary>
/// <see cref="SyncDatabaseInput"/> 同步租户数据库输入
/// </summary>
public class SyncDatabaseInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [LongRequired(ErrorMessage = "租户Id不能为空")]
    public long TenantId { get; set; }
}