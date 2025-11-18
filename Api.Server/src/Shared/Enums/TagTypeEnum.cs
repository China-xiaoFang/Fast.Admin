

namespace Fast.Shared;

/// <summary>
/// <see cref="TagTypeEnum"/> 标签类型枚举
/// </summary>
[Flags]
[FastEnum("标签类型枚举")]
public enum TagTypeEnum : byte
{
    /// <summary>
    /// Primary
    /// </summary>
    [Description("primary")] Primary = 1,

    /// <summary>
    /// Success
    /// </summary>
    [Description("success")] Success = 2,

    /// <summary>
    /// Info
    /// </summary>
    [Description("info")] Info = 4,

    /// <summary>
    /// Warning
    /// </summary>
    [Description("warning")] Warning = 8,

    /// <summary>
    /// Danger
    /// </summary>
    [Description("danger")] Danger = 16
}