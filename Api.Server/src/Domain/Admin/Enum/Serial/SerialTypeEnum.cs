// ReSharper disable once CheckNamespace

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="SerialTypeEnum"/> 序号类型枚举
/// </summary>
[Flags]
[FastEnum("序号类型枚举")]
public enum SerialTypeEnum : byte
{
    /// <summary>
    /// 系统序列号
    /// </summary>
    [Description("系统序列号")] AutoSerial = 1,

    /// <summary>
    /// 自定义序列号
    /// </summary>
    [Description("自定义序列号")] CustomSerial = 2
}