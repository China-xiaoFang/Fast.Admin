namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="QueryTableConfigDetailOutput"/> 获取表格配置详情输出
/// </summary>
public class QueryTableConfigDetailOutput : PagedOutput
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