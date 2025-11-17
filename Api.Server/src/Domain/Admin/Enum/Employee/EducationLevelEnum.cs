// ReSharper disable once CheckNamespace

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="EducationLevelEnum"/> 文化程度枚举
/// </summary>
[Flags]
[FastEnum("文化程度枚举")]
public enum EducationLevelEnum
{
    /// <summary>
    /// 文盲
    /// </summary>
    [Description("文盲")] Illiterate = 1,

    /// <summary>
    /// 半文盲
    /// </summary>
    [Description("半文盲")] SemiLiterate = 2,

    /// <summary>
    /// 小学
    /// </summary>
    [Description("小学")] PrimarySchool = 4,

    /// <summary>
    /// 初中
    /// </summary>
    [Description("初中")] JuniorMiddleSchool = 8,

    /// <summary>
    /// 高中
    /// </summary>
    [Description("高中")] HighSchool = 16,

    /// <summary>
    /// 中专
    /// </summary>
    [Description("中专")] SecondaryVocational = 32,

    /// <summary>
    /// 大专
    /// </summary>
    [Description("大专")] JuniorCollege = 64,

    /// <summary>
    /// 本科
    /// </summary>
    [Description("本科")] Bachelor = 128,

    /// <summary>
    /// 硕士
    /// </summary>
    [Description("硕士")] Master = 512,

    /// <summary>
    /// 博士
    /// </summary>
    [Description("博士")] Doctor = 1024,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")] Other = 2048
}