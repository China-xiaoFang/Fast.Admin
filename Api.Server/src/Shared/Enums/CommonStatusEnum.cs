// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="CommonStatusEnum"/> 公共状态枚举
/// </summary>
[Flags]
[FastEnum("公共状态枚举")]
public enum CommonStatusEnum : byte
{
    /// <summary>
    /// 正常
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("正常")]
    Enable = 1,

    /// <summary>
    /// 禁用
    /// </summary>
    [TagType(TagTypeEnum.Danger)] [Description("禁用")]
    Disable = 2
}