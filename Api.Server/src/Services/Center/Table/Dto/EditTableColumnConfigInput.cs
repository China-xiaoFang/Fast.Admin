namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="EditTableColumnConfigInput"/> 编辑表格列配置输入
/// </summary>
public class EditTableColumnConfigInput : UpdateVersionInput
{
    /// <summary>
    /// 表格Id
    /// </summary>
    [LongRequired(ErrorMessage = "表格Id不能为空")]
    public long TableId { get; set; }

    /// <summary>
    /// 表格列
    /// </summary>
    public List<FaTableColumnCtx> Columns { get; set; }
}