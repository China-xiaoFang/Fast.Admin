using Fast.Center.Enum;

namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="EditMenuInput"/> 编辑菜单输入
/// </summary>
public class EditMenuInput : UpdateVersionInput
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    [LongRequired(ErrorMessage = "菜单Id不能为空")]
    public long MenuId { get; set; }
    /// <summary>
    /// 版本
    /// </summary>
    [EnumRequired(ErrorMessage = "版本不能为空")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 模块Id
    /// </summary>
    [LongRequired(ErrorMessage = "模块Id不能为空")]
    public long ModuleId { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    [StringRequired(ErrorMessage = "菜单编码不能为空")]
    public string MenuCode { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [StringRequired(ErrorMessage = "菜单名称不能为空")]
    public string MenuName { get; set; }

    /// <summary>
    /// 菜单标题
    /// </summary>
    [StringRequired(ErrorMessage = "菜单标题不能为空")]
    public string MenuTitle { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [LongRequired(ErrorMessage = "父级Id不能为空")]
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    [EnumRequired(ErrorMessage = "菜单类型不能为空")]
    public MenuTypeEnum MenuType { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    [Required(ErrorMessage = "是否桌面端不能为空")]
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
    [Required(ErrorMessage = "是否Web端不能为空")]
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
    [Required(ErrorMessage = "是否移动端不能为空")]
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
    [EnumRequired(ErrorMessage = "是否显示不能为空")]
    public YesOrNotEnum Visible { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [IntRequired(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [IntRequired(ErrorMessage = "状态不能为空")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 按钮信息
    /// </summary>
    public List<EditMenuButtonDto> ButtonList { get; set; }

    /// <summary>
    /// <see cref="EditMenuButtonDto"/> 编辑菜单按钮Dto
    /// </summary>
    public class EditMenuButtonDto
    {
        /// <summary>
        /// 按钮Id
        /// </summary>
        public long? ButtonId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [EnumRequired(ErrorMessage = "版本不能为空")]
        public EditionEnum Edition { get; set; }

        /// <summary>
        /// 按钮编码
        /// </summary>
        [StringRequired(ErrorMessage = "按钮编码不能为空")]
        public string ButtonCode { get; set; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        [StringRequired(ErrorMessage = "按钮名称不能为空")]
        public string ButtonName { get; set; }

        /// <summary>
        /// 是否桌面端
        /// </summary>
        [Required(ErrorMessage = "是否桌面端不能为空")]
        public bool HasDesktop { get; set; }

        /// <summary>
        /// 是否Web端
        /// </summary>
        [Required(ErrorMessage = "是否Web端不能为空")]
        public bool HasWeb { get; set; }

        /// <summary>
        /// 是否移动端
        /// </summary>
        [Required(ErrorMessage = "是否移动端不能为空")]
        public bool HasMobile { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>从小到大</remarks>
        [IntRequired(ErrorMessage = "排序不能为空")]
        public int Sort { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [IntRequired(ErrorMessage = "状态不能为空")]
        public CommonStatusEnum Status { get; set; }
    }
}