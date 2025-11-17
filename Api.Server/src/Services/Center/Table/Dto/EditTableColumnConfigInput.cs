namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="EditTableColumnConfigInput"/> 编辑表格列配置输入
/// </summary>
public class EditTableColumnConfigInput
{
    /// <summary>
    /// 表格Id
    /// </summary>
    [LongRequired(ErrorMessage = "表格Id不能为空")]
    public long TableId { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }

    /// <summary>
    /// 表格列
    /// </summary>
    public List<FaTableColumnCtx> Columns { get; set; }
}