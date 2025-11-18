

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="SysSerialSpacerEnum"/> 系统序号分隔符枚举
/// </summary>
[Flags]
[FastEnum("系统序号分隔符枚举")]
public enum SysSerialSpacerEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("无")] None = 0,

    /// <summary>
    /// 下划线
    /// </summary>
    [Description("_")] Underscore = 1,

    /// <summary>
    /// 中横线
    /// </summary>
    [Description("-")] Hyphen = 2,

    /// <summary>
    /// 点
    /// </summary>
    [Description(".")] Dot = 4
}