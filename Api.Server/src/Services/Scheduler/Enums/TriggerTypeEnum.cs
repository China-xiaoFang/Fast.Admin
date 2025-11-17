// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="TriggerTypeEnum"/> 触发器类型枚举
/// </summary>
[Flags]
[FastEnum("触发器类型枚举")]
public enum TriggerTypeEnum : byte
{
    /// <summary>
    /// Cron
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("Cron")]
    Cron = 1,

    /// <summary>
    /// Daily
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("Daily")]
    Daily = 2,

    /// <summary>
    /// Simple
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("Simple")]
    Simple = 4
}