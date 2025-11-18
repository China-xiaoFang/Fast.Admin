namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="QueryTableColumnConfigOutput"/> 获取表格列配置输出
/// </summary>
public class QueryTableColumnConfigOutput
{
    /// <summary>
    /// 表格Key
    /// </summary>
    public string TableKey { get; set; }

    /// <summary>
    /// 原始列
    /// </summary>
    public List<IDictionary<string, object>> Columns { get; set; }

    /// <summary>
    /// 缓存列
    /// </summary>
    public List<IDictionary<string, object>> CacheColumns { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public  DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 是否存在改变
    /// </summary>
    public bool Change { get; set; }

    /// <summary>
    /// 是否存在缓存
    /// </summary>
    public bool Cache { get; set; }
}