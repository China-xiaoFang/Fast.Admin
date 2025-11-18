

namespace Fast.Shared;

/// <summary>
/// <see cref="LevelEnum"/> 级别枚举
/// </summary>
[Flags]
[FastEnum("级别枚举")]
public enum LevelEnum : byte
{
    /// <summary>
    /// 默认级
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("默认级")]
    Default = 0,

    /// <summary>
    /// 系统级
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("系统级")]
    System = 1,

    /// <summary>
    /// 租户级
    /// </summary>
    [TagType(TagTypeEnum.Warning)] [Description("租户级")]
    Tenant = 2,

    /// <summary>
    /// 自定义级
    /// </summary>
    [TagType(TagTypeEnum.Danger)] [Description("自定义级")]
    Custom = 32
}