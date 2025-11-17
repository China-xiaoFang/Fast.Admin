namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="CopyTableConfigInput"/> 复制表格配置输入
/// </summary>
public class CopyTableConfigInput
{
    /// <summary>
    /// 表格Id
    /// </summary>
    [LongRequired(ErrorMessage = "表格Id不能为空")]
    public long TableId { get; set; }

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