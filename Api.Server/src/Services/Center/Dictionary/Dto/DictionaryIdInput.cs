namespace Fast.Center.Service.Dictionary.Dto;

/// <summary>
/// <see cref="DictionaryIdInput"/> 字典Id输入
/// </summary>
public class DictionaryIdInput : UpdateVersionInput
{
    /// <summary>
    /// 字典Id
    /// </summary>
    [LongRequired(ErrorMessage = "字典Id不能为空")]
    public long DictionaryId { get; set; }
}