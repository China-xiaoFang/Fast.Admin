namespace Fast.Center.Service.Config.Dto;

/// <summary>
/// <see cref="QueryConfigPagedOutput"/> 获取配置分页列表输出
/// </summary>
public class QueryConfigPagedOutput : PagedOutput
{
    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }

    /// <summary>
    /// 配置编码
    /// </summary>
    public string ConfigCode { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    public string ConfigName { get; set; }

    /// <summary>
    /// 配置值
    /// </summary>
    /// <remarks>
    /// <para>Boolean：[True, False]</para>
    /// </remarks>
    public string ConfigValue { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}