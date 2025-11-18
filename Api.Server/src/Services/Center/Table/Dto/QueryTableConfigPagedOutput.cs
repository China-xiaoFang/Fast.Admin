namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="QueryTableConfigPagedOutput"/> 获取表格配置分页列表输出
/// </summary>
public class QueryTableConfigPagedOutput : PagedOutput
{
    /// <summary>
    /// 表格Id
    /// </summary>
    public long TableId { get; set; }

    /// <summary>
    /// 表格Key
    /// </summary>
    public string TableKey { get; set; }

    /// <summary>
    /// 表格名称
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}