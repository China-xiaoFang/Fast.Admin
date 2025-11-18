using Fast.Center.Enum;

namespace Fast.Center.Service.Dictionary.Dto;

/// <summary>
/// <see cref="EditDictionaryInput"/> 编辑字典输入
/// </summary>
public class EditDictionaryInput : UpdateVersionInput
{
    /// <summary>
    /// 字典Id
    /// </summary>
    [LongRequired(ErrorMessage = "字典Id不能为空")]
    public long DictionaryId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [StringRequired(ErrorMessage = "字典名称不能为空")]
    public string DictionaryName { get; set; }

    /// <summary>
    /// 字典值类型
    /// </summary>
    [EnumRequired(ErrorMessage = "字典值类型不能为空")]
    public DictionaryValueTypeEnum ValueType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [EnumRequired(ErrorMessage = "状态不能为空")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 字典项集合
    /// </summary>
    public List<EditDictionaryItemInput> DictionaryItemList { get; set; }

    /// <summary>
    /// <see cref="EditDictionaryItemInput"/> 编辑字典项输入
    /// </summary>
    public class EditDictionaryItemInput
    {
        /// <summary>
        /// 字典项Id
        /// </summary>
        public long? DictionaryItemId { get; set; }

        /// <summary>
        /// 字典项名称
        /// </summary>
        [StringRequired(ErrorMessage = "字典项名称不能为空")]
        public string Label { get; set; }

        /// <summary>
        /// 字典项值
        /// </summary>
        [StringRequired(ErrorMessage = "字典项值不能为空")]
        public string Value { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        [EnumRequired(ErrorMessage = "标签类型不能为空")]
        public TagTypeEnum Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>从小到大</remarks>
        [IntRequired(ErrorMessage = "排序不能为空")]
        public int Order { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [EnumRequired(ErrorMessage = "是否显示不能为空")]
        public YesOrNotEnum Visible { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EnumRequired(ErrorMessage = "状态不能为空")]
        public CommonStatusEnum Status { get; set; }
    }
}