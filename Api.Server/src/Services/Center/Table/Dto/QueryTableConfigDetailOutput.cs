namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="QueryTableConfigDetailOutput"/> 获取表格配置详情输出
/// </summary>
public class QueryTableConfigDetailOutput
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

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public virtual string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    public virtual string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    public long RowVersion { get; set; }
}