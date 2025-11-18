

namespace Fast.CenterLog.Enum;

/// <summary>
/// <see cref="DiffLogTypeEnum"/> 差异日志类型枚举
/// </summary>
[Flags]
[FastEnum("差异日志类型枚举")]
public enum DiffLogTypeEnum : byte
{
    /// <summary>
    /// 未知
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("未知")]
    None = 0,

    /// <summary>
    /// 添加
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("添加")]
    Insert = 1,

    /// <summary>
    /// 更新
    /// </summary>
    [TagType(TagTypeEnum.Warning)] [Description("更新")]
    Update = 2,

    /// <summary>
    /// 删除
    /// </summary>
    [TagType(TagTypeEnum.Danger)] [Description("删除")]
    Delete = 4
}