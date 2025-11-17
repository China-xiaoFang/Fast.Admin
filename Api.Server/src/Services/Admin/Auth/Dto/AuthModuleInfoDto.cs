namespace Fast.Admin.Service.Auth.Dto;

/// <summary>
/// <see cref="AuthModuleInfoDto"/> 授权模块信息Dto
/// </summary>
public class AuthModuleInfoDto
{
    /// <summary>
    /// 模块Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
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
    /// 菜单集合
    /// </summary>
    public List<AuthMenuInfoDto> Children { get; set; } = [];
}