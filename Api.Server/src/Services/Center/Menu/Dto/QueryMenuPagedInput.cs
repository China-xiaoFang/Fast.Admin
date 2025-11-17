using Fast.Center.Enum;

namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="QueryMenuPagedInput"/> 获取菜单列表输入
/// </summary>
public class QueryMenuPagedInput : PagedInput
{
    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum? Edition { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    public long? AppId { get; set; }

    /// <summary>
    /// 模块Id
    /// </summary>
    public long? ModuleId { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuTypeEnum? MenuType { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    public bool? HasDesktop { get; set; }

    /// <summary>
    /// 是否Web端
    /// </summary>
    public bool? HasWeb { get; set; }

    /// <summary>
    /// 是否移动端
    /// </summary>
    public bool? HasMobile { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public YesOrNotEnum? Visible { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum? Status { get; set; }
}