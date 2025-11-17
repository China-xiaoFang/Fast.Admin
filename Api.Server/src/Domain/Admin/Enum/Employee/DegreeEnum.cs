// ReSharper disable once CheckNamespace

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="DegreeEnum"/> 学位枚举
/// </summary>
[Flags]
[FastEnum("学位枚举")]
public enum DegreeEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("无")] None = 1,

    /// <summary>
    /// 学士
    /// </summary>
    [Description("学士")] Bachelor = 2,

    /// <summary>
    /// 硕士
    /// </summary>
    [Description("硕士")] Master = 4,

    /// <summary>
    /// 博士
    /// </summary>
    [Description("博士")] Doctor = 8,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")] Other = 16
}