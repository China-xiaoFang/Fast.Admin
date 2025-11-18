

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="AcademicSystemEnum"/> 学制枚举
/// </summary>
[Flags]
[FastEnum("学制枚举")]
public enum AcademicSystemEnum : byte
{
    /// <summary>
    /// 两年制
    /// </summary>
    [Description("两年制")] TwoYears = 1,

    /// <summary>
    /// 三年制
    /// </summary>
    [Description("三年制")] ThreeYears = 2,

    /// <summary>
    /// 四年制
    /// </summary>
    [Description("四年制")] FourYears = 4,

    /// <summary>
    /// 五年制
    /// </summary>
    [Description("五年制")] FiveYears = 8,

    /// <summary>
    /// 六年制
    /// </summary>
    [Description("六年制")] SixYears = 16,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")] Other = 32
}