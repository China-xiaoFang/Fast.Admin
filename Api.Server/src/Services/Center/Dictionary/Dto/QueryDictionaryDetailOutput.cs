using Fast.Center.Enum;

namespace Fast.Center.Service.Dictionary.Dto;

/// <summary>
/// <see cref="QueryDictionaryDetailOutput"/> 获取字典详情输出
/// </summary>
public class QueryDictionaryDetailOutput
{
    /// <summary>
    /// 字典Id
    /// </summary>
    public long DictionaryId { get; set; }

    /// <summary>
    /// 字典Key
    /// </summary>
    public string DictionaryKey { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string DictionaryName { get; set; }

    /// <summary>
    /// 字典值类型
    /// </summary>
    public DictionaryValueTypeEnum ValueType { get; set; }

    /// <summary>
    /// Flags枚举
    /// </summary>
    public YesOrNotEnum HasFlags { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public virtual string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    public virtual string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    public long RowVersion { get; set; }

    /// <summary>
    /// 字典项集合
    /// </summary>
    public List<QueryDictionaryItemDetailDto> DictionaryItemList { get; set; }

    /// <summary>
    /// <see cref="QueryDictionaryItemDetailDto"/> 获取字典项详情Dto
    /// </summary>
    public class QueryDictionaryItemDetailDto
    {
        /// <summary>
        /// 字典项Id
        /// </summary>
        public long DictionaryItemId { get; set; }

        /// <summary>
        /// 字典项名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 字典项值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public TagTypeEnum Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>从小到大</remarks>
        public int Order { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public YesOrNotEnum Visible { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatusEnum Status { get; set; }
    }
}