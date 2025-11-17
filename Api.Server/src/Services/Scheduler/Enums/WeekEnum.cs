// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="MailMessageEnum"/> 星期枚举
/// </summary>
[Flags]
[FastEnum("星期枚举")]
public enum WeekEnum
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("无")] None = 0,

    /// <summary>
    /// 星期一
    /// </summary>
    [Description("星期一")] Monday = 1,

    /// <summary>
    /// 星期二
    /// </summary>
    [Description("星期二")] Tuesday = 2,

    /// <summary>
    /// 星期三
    /// </summary>
    [Description("星期三")] Wednesday = 4,

    /// <summary>
    /// 星期四
    /// </summary>
    [Description("星期四")] Thursday = 8,

    /// <summary>
    /// 星期五
    /// </summary>
    [Description("星期五")] Friday = 16,

    /// <summary>
    /// 星期六
    /// </summary>
    [Description("星期六")] Saturday = 32,

    /// <summary>
    /// 星期日
    /// </summary>
    [Description("星期日")] Sunday = 64,

    /// <summary>
    /// 全部
    /// </summary>
    All = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,

    /// <summary>
    /// 工作日
    /// </summary>
    Workday = Monday | Tuesday | Wednesday | Thursday | Friday,

    /// <summary>
    /// 休息日
    /// </summary>
    DayOff = Saturday | Sunday
}

/// <summary>
/// <see cref="WeekEnum"/> 拓展类
/// </summary>
public static class WeekEnumExtension
{
    /// <summary>
    /// 转换为 <see cref="DayOfWeek"/>
    /// </summary>
    /// <param name="week"></param>
    /// <returns></returns>
    public static List<DayOfWeek> ToDayOfWeek(this WeekEnum week)
    {
        var result = new List<DayOfWeek>();

        if ((week & WeekEnum.Monday) != 0)
        {
            result.Add(DayOfWeek.Monday);
        }

        if ((week & WeekEnum.Tuesday) != 0)
        {
            result.Add(DayOfWeek.Tuesday);
        }

        if ((week & WeekEnum.Wednesday) != 0)
        {
            result.Add(DayOfWeek.Wednesday);
        }

        if ((week & WeekEnum.Thursday) != 0)
        {
            result.Add(DayOfWeek.Thursday);
        }

        if ((week & WeekEnum.Friday) != 0)
        {
            result.Add(DayOfWeek.Friday);
        }

        if ((week & WeekEnum.Saturday) != 0)
        {
            result.Add(DayOfWeek.Saturday);
        }

        if ((week & WeekEnum.Sunday) != 0)
        {
            result.Add(DayOfWeek.Sunday);
        }

        return result;
    }

    /// <summary>
    /// 转换为 <see cref="WeekEnum"/>
    /// </summary>
    /// <param name="daysOfWeek"></param>
    /// <returns></returns>
    public static WeekEnum? ToWeekEnum(this List<DayOfWeek> daysOfWeek)
    {
        var week = WeekEnum.None;

        foreach (var day in daysOfWeek)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    week |= WeekEnum.Monday;
                    break;
                case DayOfWeek.Tuesday:
                    week |= WeekEnum.Tuesday;
                    break;
                case DayOfWeek.Wednesday:
                    week |= WeekEnum.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                    week |= WeekEnum.Thursday;
                    break;
                case DayOfWeek.Friday:
                    week |= WeekEnum.Friday;
                    break;
                case DayOfWeek.Saturday:
                    week |= WeekEnum.Saturday;
                    break;
                case DayOfWeek.Sunday:
                    week |= WeekEnum.Sunday;
                    break;
            }
        }

        return week == WeekEnum.None ? null : week;
    }
}