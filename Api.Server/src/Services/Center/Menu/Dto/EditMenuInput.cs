namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="EditMenuInput"/> 编辑菜单输入
/// </summary>
public class EditMenuInput : AddMenuInput
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    [LongRequired(ErrorMessage = "菜单Id不能为空")]
    public long MenuId { get; set; }

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