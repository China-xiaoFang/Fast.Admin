using Fast.Center.Enum;

namespace Fast.Center.Service.Module.Dto;

/// <summary>
/// <see cref="AddModuleInput"/> 添加模块输入
/// </summary>
public class AddModuleInput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [LongRequired(ErrorMessage = "应用Id不能为空")]
    public long AppId { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    [StringRequired(ErrorMessage = "模块名称不能为空")]
    public string ModuleName { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [StringRequired(ErrorMessage = "图标不能为空")]
    public string Icon { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// 查看类型
    /// </summary>
    [EnumRequired(ErrorMessage = "查看类型不能为空")]
    public ModuleViewTypeEnum ViewType { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [IntRequired(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }
}