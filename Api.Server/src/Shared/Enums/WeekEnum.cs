// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

namespace Fast.Shared;

/// <summary>
/// <see cref="WeekEnum"/> 星期枚举
/// </summary>
[Flags]
[FastEnum("星期枚举")]
public enum WeekEnum
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("无")]
    None = 0,

    /// <summary>
    /// 星期一
    /// </summary>
    [Description("星期一")]
    Monday = 1,

    /// <summary>
    /// 星期二
    /// </summary>
    [Description("星期二")]
    Tuesday = 2,

    /// <summary>
    /// 星期三
    /// </summary>
    [Description("星期三")]
    Wednesday = 4,

    /// <summary>
    /// 星期四
    /// </summary>
    [Description("星期四")]
    Thursday = 8,

    /// <summary>
    /// 星期五
    /// </summary>
    [Description("星期五")]
    Friday = 16,

    /// <summary>
    /// 星期六
    /// </summary>
    [Description("星期六")]
    Saturday = 32,

    /// <summary>
    /// 星期日
    /// </summary>
    [Description("星期日")]
    Sunday = 64,

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
    /// 星期映射
    /// </summary>
    private static readonly (WeekEnum weekEnum, DayOfWeek dayOfWeek)[] WeekMap =
    [
        (WeekEnum.Monday, DayOfWeek.Monday), (WeekEnum.Tuesday, DayOfWeek.Tuesday), (WeekEnum.Wednesday, DayOfWeek.Wednesday),
        (WeekEnum.Thursday, DayOfWeek.Thursday), (WeekEnum.Friday, DayOfWeek.Friday), (WeekEnum.Saturday, DayOfWeek.Saturday),
        (WeekEnum.Sunday, DayOfWeek.Sunday)
    ];

    /// <summary>
    /// 转换为 <see cref="DayOfWeek"/> 集合
    /// </summary>
    /// <param name="week"></param>
    /// <returns></returns>
    public static List<DayOfWeek> ToDayOfWeeks(this WeekEnum week)
    {
        return WeekMap.Where(wh => (week & wh.weekEnum) != 0)
            .Select(sl => sl.dayOfWeek)
            .ToList();
    }

    /// <summary>
    /// 转换为 <see cref="DayOfWeek"/>
    /// </summary>
    /// <param name="week"></param>
    /// <returns></returns>
    public static DayOfWeek ToDayOfWeek(this WeekEnum week)
    {
        return ToDayOfWeeks(week)
            .FirstOrDefault();
    }

    /// <summary>
    /// 转换为 <see cref="WeekEnum"/>
    /// </summary>
    /// <param name="daysOfWeek"></param>
    /// <returns></returns>
    public static WeekEnum? ToWeekEnum(this DayOfWeek daysOfWeek)
    {
        foreach (var item in WeekMap)
        {
            if (item.dayOfWeek == daysOfWeek)
                return item.weekEnum;
        }

        return null;
    }

    /// <summary>
    /// 转换为 <see cref="WeekEnum"/>
    /// </summary>
    /// <param name="daysOfWeek"></param>
    /// <returns></returns>
    public static WeekEnum? ToWeekEnum(this List<DayOfWeek> daysOfWeek)
    {
        var week = WeekEnum.None;

        foreach (var item in WeekMap)
        {
            if (daysOfWeek.Contains(item.dayOfWeek))
                week |= item.weekEnum;
        }

        return week == WeekEnum.None ? null : week;
    }
}