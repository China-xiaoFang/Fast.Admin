

namespace Fast.Shared;

/// <summary>
/// <see cref="UserTypeEnum"/> 用户类型枚举
/// </summary>
[Flags]
[FastEnum("用户类型枚举")]
public enum UserTypeEnum : byte
{
    /// <summary>
    /// 超级管理员
    /// </summary>
    [TagType(TagTypeEnum.Danger)] [Description("超级管理员")]
    SuperAdmin = 1,

    /// <summary>
    /// 管理员
    /// </summary>
    /// <remarks>每个租户只有一个管理员账号</remarks>
    [TagType(TagTypeEnum.Warning)] [Description("管理员")]
    Admin = 2,

    /// <summary>
    /// 机器人
    /// </summary>
    /// <remarks>每个租户只有一个机器人账号</remarks>
    [TagType(TagTypeEnum.Primary)] [Description("机器人")]
    Robot = 4,

    /// <summary>
    /// 普通账号
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("普通账号")]
    None = 8
}