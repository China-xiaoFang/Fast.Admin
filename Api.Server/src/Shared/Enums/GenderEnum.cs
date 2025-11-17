// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="GenderEnum"/> 性别枚举
/// </summary>
[Flags]
[FastEnum("性别枚举")]
public enum GenderEnum : byte
{
    /// <summary>
    /// 未知
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 男
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("男")]
    Man = 1,

    /// <summary>
    /// 女
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("女")]
    Woman = 2
}