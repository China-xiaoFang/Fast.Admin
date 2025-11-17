namespace Fast.Center.Service.File.Dto;

/// <summary>
/// <see cref="QueryFilePagedInput"/> 获取文件分页列表输入
/// </summary>
public class QueryFilePagedInput : PagedInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }
}