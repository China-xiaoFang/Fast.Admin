

namespace Fast.Shared;

/// <summary>
/// <see cref="YesOrNotEnum"/> 是否枚举
/// </summary>
[FastEnum("是否枚举")]
public enum YesOrNotEnum : byte
{
    /// <summary>
    /// 是
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("是")]
    Y = 1,

    /// <summary>
    /// 否
    /// </summary>
    [TagType(TagTypeEnum.Danger)] [Description("否")]
    N = 0
}