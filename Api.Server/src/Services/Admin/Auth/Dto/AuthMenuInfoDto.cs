using System.Collections;
using Fast.Center.Enum;

namespace Fast.Admin.Service.Auth.Dto;

/// <summary>
/// <see cref="AuthMenuInfoDto"/> 授权菜单信息Dto
/// </summary>
public class AuthMenuInfoDto : ITreeNode<long>
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    public long MenuId { get; set; }

    /// <summary>
    /// 模块Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    public string MenuCode { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; }

    /// <summary>
    /// 菜单标题
    /// </summary>
    public string MenuTitle { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuTypeEnum MenuType { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public string Router { get; set; }

    /// <summary>
    /// 组件地址
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// 内链/外链地址
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public YesOrNotEnum Visible { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    public int Sort { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    public List<AuthMenuInfoDto> Children { get; set; } = [];

    /// <summary>获取节点id</summary>
    /// <returns></returns>
    public long GetId()
    {
        return MenuId;
    }

    /// <summary>获取节点父id</summary>
    /// <returns></returns>
    public long GetPid()
    {
        return ParentId;
    }

    /// <summary>获取排序字段</summary>
    /// <returns></returns>
    public long GetSort()
    {
        return Sort;
    }

    /// <summary>设置Children</summary>
    /// <param name="children"></param>
    public void SetChildren(IList children)
    {
        Children = (List<AuthMenuInfoDto>)children;
    }
}