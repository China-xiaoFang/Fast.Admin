

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="SysSerialDateTypeEnum"/> 系统序号时间类型枚举
/// </summary>
[Flags]
[FastEnum("系统序号时间类型枚举")]
public enum SysSerialDateTypeEnum : byte
{
    /// <summary>
    /// 年(yyyy)
    /// </summary>
    [Description("年(yyyy)")] Year = 1,

    /// <summary>
    /// 年月(yyyyMM)
    /// </summary>
    [Description("年月(yyyyMM)")] Month = 2,

    /// <summary>
    /// 年月日(yyyyMMdd)
    /// </summary>
    [Description("年月日(yyyyMMdd)")] Day = 4,

    /// <summary>
    /// 年月日时(yyyyMMddHH)
    /// </summary>
    [Description("年月日时(yyyyMMddHH)")] Hour = 8
}