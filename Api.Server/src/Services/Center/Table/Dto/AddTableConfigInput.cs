namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="AddTableConfigInput"/> 添加表格配置输入
/// </summary>
public class AddTableConfigInput
{
    /// <summary>
    /// 表格名称
    /// </summary>
    [StringRequired(ErrorMessage = "表格名称不能为空")]
    public string TableName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}