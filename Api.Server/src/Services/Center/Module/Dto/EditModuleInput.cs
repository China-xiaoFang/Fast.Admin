namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="EditModuleInput"/> 编辑模块输入
/// </summary>
public class EditModuleInput : AddModuleInput
{
    /// <summary>
    /// 模块Id
    /// </summary>
    [LongRequired(ErrorMessage = "模块Id不能为空")]
    public long ModuleId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [IntRequired(ErrorMessage = "状态不能为空")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }
}