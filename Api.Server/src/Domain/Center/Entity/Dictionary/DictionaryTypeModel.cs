

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="DictionaryTypeModel"/> 字典类型表Model类
/// </summary>
[SugarTable("DictionaryType", "字典类型表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(DictionaryKey)}", nameof(DictionaryKey), OrderByType.Asc, true)]
public class DictionaryTypeModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 字典Id
    /// </summary>
    [SugarColumn(ColumnDescription = "字典Id", IsPrimaryKey = true)]
    public long DictionaryId { get; set; }

    /// <summary>
    /// 字典Key
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "字典Key", Length = 50)]
    public string DictionaryKey { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "字典名称", Length = 50)]
    public string DictionaryName { get; set; }

    /// <summary>
    /// 字典值类型
    /// </summary>
    [SugarColumn(ColumnDescription = "字典值类型")]
    public DictionaryValueTypeEnum ValueType { get; set; }

    /// <summary>
    /// Flags枚举
    /// </summary>
    [SugarColumn(ColumnDescription = "Flags枚举")]
    public YesOrNotEnum HasFlags { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string Remark { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }

    /// <summary>
    /// 字典项信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(DictionaryItemModel.DictionaryId), nameof(DictionaryId))]
    public List<DictionaryItemModel> DictionaryItemList { get; set; }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return DictionaryId.GetHashCode();
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        if (obj is not DictionaryTypeModel oldDictionaryTypeModel)
            return false;

        return DictionaryId == oldDictionaryTypeModel.DictionaryId
               && DictionaryKey == oldDictionaryTypeModel.DictionaryKey
               && DictionaryName == oldDictionaryTypeModel.DictionaryName
               && ValueType == oldDictionaryTypeModel.ValueType
               && HasFlags == oldDictionaryTypeModel.HasFlags;
    }
}