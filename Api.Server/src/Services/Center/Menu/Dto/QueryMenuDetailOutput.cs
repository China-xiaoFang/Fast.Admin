using Fast.Center.Enum;

namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="QueryMenuDetailOutput"/> 获取菜单详情输出
/// </summary>
public class QueryMenuDetailOutput
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    public long MenuId { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    public long AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 模块Id
    /// </summary>
    public long ModuleId { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    public string ModuleName { get; set; }

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
    /// 菜单类型
    /// </summary>
    public MenuTypeEnum MenuType { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    public bool HasDesktop { get; set; }

    /// <summary>
    /// 桌面端图标
    /// </summary>
    public string DesktopIcon { get; set; }

    /// <summary>
    /// 桌面端路由地址
    /// </summary>
    public string DesktopRouter { get; set; }

    /// <summary>
    /// 是否Web端
    /// </summary>
    public bool HasWeb { get; set; }

    /// <summary>
    /// Web端图标
    /// </summary>
    public string WebIcon { get; set; }

    /// <summary>
    /// Web端路由地址
    /// </summary>
    public string WebRouter { get; set; }

    /// <summary>
    /// Web端组件地址
    /// </summary>
    public string WebComponent { get; set; }

    /// <summary>
    /// 是否移动端
    /// </summary>
    public bool HasMobile { get; set; }

    /// <summary>
    /// 移动端图标
    /// </summary>
    public string MobileIcon { get; set; }

    /// <summary>
    /// 移动端路由地址
    /// </summary>
    public string MobileRouter { get; set; }

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

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    public long RowVersion { get; set; }

    /// <summary>
    /// 按钮信息
    /// </summary>
    public List<QueryMenuButtonDetailDto> ButtonList { get; set; }

    /// <summary>
    /// <see cref="QueryMenuButtonDetailDto"/> 获取菜单按钮详情Dto
    /// </summary>
    public class QueryMenuButtonDetailDto
    {
        /// <summary>
        /// 按钮Id
        /// </summary>
        public long ButtonId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public EditionEnum Edition { get; set; }

        /// <summary>
        /// 按钮编码
        /// </summary>
        public string ButtonCode { get; set; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string ButtonName { get; set; }

        /// <summary>
        /// 是否桌面端
        /// </summary>
        public bool HasDesktop { get; set; }

        /// <summary>
        /// 是否Web端
        /// </summary>
        public bool HasWeb { get; set; }

        /// <summary>
        /// 是否移动端
        /// </summary>
        public bool HasMobile { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>从小到大</remarks>
        public int Sort { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatusEnum Status { get; set; }
    }
}