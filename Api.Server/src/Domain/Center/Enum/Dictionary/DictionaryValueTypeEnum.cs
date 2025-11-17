// ReSharper disable once CheckNamespace

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="DictionaryValueTypeEnum"/> 字典值类型枚举
/// </summary>
[Flags]
[FastEnum("字典值类型枚举")]
public enum DictionaryValueTypeEnum : byte
{
    /// <summary>
    /// 字符串
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("字符串")]
    String = 1,

    /// <summary>
    /// Int
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("Int")]
    Int = 2,

    /// <summary>
    /// Long
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("Long")]
    Long = 4,

    /// <summary>
    /// Boolean
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("Boolean")]
    Boolean = 8
}