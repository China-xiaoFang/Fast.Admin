using Fast.Center.Enum;

namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="QueryModulePagedOutput"/> 获取模块分页列表输出
/// </summary>
public class QueryModulePagedOutput
{
    /// <summary>
    /// 模块Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    [SugarSearchValue]
    public string ModuleName { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// 查看类型
    /// </summary>
    public ModuleViewTypeEnum ViewType { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    public string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }
}