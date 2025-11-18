using Fast.Center.Enum;

namespace Fast.Center.Service.Module.Dto;

/// <summary>
/// <see cref="QueryModulePagedInput"/> 获取模块分页列表
/// </summary>
public class QueryModulePagedInput : PagedInput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    public long? AppId { get; set; }

    /// <summary>
    /// 查看类型
    /// </summary>
    public ModuleViewTypeEnum? ViewType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum? Status { get; set; }
}