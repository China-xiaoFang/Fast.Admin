namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="QueryApplicationPagedInput"/> 获取应用分页列表输入
/// </summary>
public class QueryApplicationPagedInput : PagedInput
{
    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum? Edition { get; set; }
}