namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="TableIdInput"/> 表格Id输入
/// </summary>
public class TableIdInput : UpdateVersionInput
{
    /// <summary>
    /// 表格Id
    /// </summary>
    [LongRequired(ErrorMessage = "表格Id不能为空")]
    public long TableId { get; set; }

}