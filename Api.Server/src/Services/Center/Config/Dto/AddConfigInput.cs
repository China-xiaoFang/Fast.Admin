namespace Fast.Center.Service.Config.Dto;

/// <summary>
/// <see cref="AddConfigInput"/> 添加配置输入
/// </summary>
public class AddConfigInput
{
    /// <summary>
    /// 配置编码
    /// </summary>
    [StringRequired(ErrorMessage = "配置编码不能为空")]
    public string ConfigCode { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    [StringRequired(ErrorMessage = "配置名称不能为空")]
    public string ConfigName { get; set; }

    /// <summary>
    /// 配置值
    /// </summary>
    /// <remarks>
    /// <para>Boolean：[True, False]</para>
    /// </remarks>
    [StringRequired(ErrorMessage = "配置值不能为空")]
    public string ConfigValue { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}