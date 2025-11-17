namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="ModuleIdInput"/> 模块Id输入
/// </summary>
public class ModuleIdInput
{
    /// <summary>
    /// 模块Id
    /// </summary>
    [LongRequired(ErrorMessage = "模块Id不能为空")]
    public long ModuleId { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }
}