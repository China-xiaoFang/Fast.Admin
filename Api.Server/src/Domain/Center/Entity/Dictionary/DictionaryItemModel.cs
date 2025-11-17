// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="DictionaryItemModel"/> 字典项表Model类
/// </summary>
[SugarTable("DictionaryItem", "字典项表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class DictionaryItemModel : BaseEntity
{
    /// <summary>
    /// 字典项Id
    /// </summary>
    [SugarColumn(ColumnDescription = "字典项Id", IsPrimaryKey = true)]
    public long DictionaryItemId { get; set; }

    /// <summary>
    /// 字典Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "字典Id")]
    public long DictionaryId { get; set; }

    /// <summary>
    /// 字典项名称
    /// </summary>
    [SugarColumn(ColumnDescription = "字典项名称", Length = 50)]
    public string Label { get; set; }

    /// <summary>
    /// 字典项值
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "字典项值", Length = 50)]
    public string Value { get; set; }

    /// <summary>
    /// 标签类型
    /// </summary>
    [SugarColumn(ColumnDescription = "标签类型")]
    public TagTypeEnum Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [Required]
    [SugarColumn(ColumnDescription = "排序")]
    public int Order { get; set; }

    /// <summary>
    /// 提示
    /// </summary>
    [SugarColumn(ColumnDescription = "提示", Length = 100)]
    public string Tips { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    [SugarColumn(ColumnDescription = "是否显示")]
    public YesOrNotEnum Visible { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return DictionaryItemId.GetHashCode();
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        if (obj is not DictionaryItemModel oldDictionaryItemModel)
            return false;

        return DictionaryItemId == oldDictionaryItemModel.DictionaryItemId
               && DictionaryId == oldDictionaryItemModel.DictionaryId
               && Label == oldDictionaryItemModel.Label
               && Value == oldDictionaryItemModel.Value
               && Type == oldDictionaryItemModel.Type
               && Order == oldDictionaryItemModel.Order;
    }
}