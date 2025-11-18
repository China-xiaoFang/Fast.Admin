

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="AcademicQualificationsEnum"/> 学历枚举
/// </summary>
[Flags]
[FastEnum("学历枚举")]
public enum AcademicQualificationsEnum
{
    /// <summary>
    /// 小学
    /// </summary>
    [Description("小学")] PrimarySchool = 1,

    /// <summary>
    /// 初中
    /// </summary>
    [Description("初中")] JuniorMiddleSchool = 2,

    /// <summary>
    /// 高中
    /// </summary>
    [Description("高中")] HighSchool = 4,

    /// <summary>
    /// 中专
    /// </summary>
    [Description("中专")] SecondaryVocational = 8,

    /// <summary>
    /// 大专
    /// </summary>
    [Description("大专")] JuniorCollege = 16,

    /// <summary>
    /// 本科
    /// </summary>
    [Description("本科")] Bachelor = 32,

    /// <summary>
    /// 硕士
    /// </summary>
    [Description("硕士")] Master = 64,

    /// <summary>
    /// 博士
    /// </summary>
    [Description("博士")] Doctor = 128,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")] Other = 256
}