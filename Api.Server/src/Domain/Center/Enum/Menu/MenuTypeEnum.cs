// ReSharper disable once CheckNamespace

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="MenuTypeEnum"/> 菜单类型枚举
/// </summary>
[Flags]
[FastEnum("菜单类型枚举")]
public enum MenuTypeEnum : byte
{
    /// <summary>
    /// 目录
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("目录")]
    Catalog = 1,

    /// <summary>
    /// 菜单
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("菜单")]
    Menu = 2,

    /// <summary>
    /// 内链
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("内链")]
    Internal = 4,

    /// <summary>
    /// 外链
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("外链")]
    Outside = 8
}