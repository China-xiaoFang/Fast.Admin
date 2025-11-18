

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="ModuleViewTypeEnum"/> 模块查看类型枚举
/// </summary>
[Flags]
[FastEnum("模块查看类型枚举")]
public enum ModuleViewTypeEnum : byte
{
    /// <summary>
    /// 超级管理员
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("超级管理员")]
    SuperAdmin = 1,

    /// <summary>
    /// 管理员
    /// </summary>
    /// <remarks>只有超级管理员和管理员可以查看</remarks>
    [TagType(TagTypeEnum.Success)] [Description("管理员")]
    Admin = 2,

    /// <summary>
    /// 全部
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("全部")]
    All = 4
}