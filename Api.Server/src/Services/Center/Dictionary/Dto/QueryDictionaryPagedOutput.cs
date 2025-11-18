using Fast.Center.Enum;

namespace Fast.Center.Service.Dictionary.Dto;

/// <summary>
/// <see cref="QueryDictionaryPagedOutput"/> 获取字典分页列表输出
/// </summary>
public class QueryDictionaryPagedOutput : PagedOutput
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
}