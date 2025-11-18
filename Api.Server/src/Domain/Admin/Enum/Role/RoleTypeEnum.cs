

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="RoleTypeEnum"/> 角色类型枚举
/// </summary>
[Flags]
[FastEnum("角色类型枚举")]
public enum RoleTypeEnum : byte
{
    /// <summary>
    /// 普通
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("普通")]
    Normal = 0,

    /// <summary>
    /// 管理员
    /// </summary>
    /// <remarks>默认查看所有数据</remarks>
    [TagType(TagTypeEnum.Primary)] [Description("管理员")]
    Admin = 2
}