namespace Fast.Center.Service.Module.Dto;

/// <summary>
/// <see cref="ModuleIdInput"/> 模块Id输入
/// </summary>
public class ModuleIdInput : UpdateVersionInput
{
    /// <summary>
    /// 模块Id
    /// </summary>
    [LongRequired(ErrorMessage = "模块Id不能为空")]
    public long ModuleId { get; set; }
}