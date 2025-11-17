// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerJobGroupEnum"/> 调度作业分组枚举
/// </summary>
[Flags]
[FastEnum("调度作业分组枚举")]
public enum SchedulerJobGroupEnum : byte
{
    /// <summary>
    /// 系统管理
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("系统管理")]
    System = 1,

    /// <summary>
    /// 业务处理
    /// </summary>
    [TagType(TagTypeEnum.Warning)] [Description("业务处理")]
    Business = 2,

    /// <summary>
    /// 第三方集成
    /// </summary>
    [TagType(TagTypeEnum.Danger)] [Description("第三方集成")]
    ThirdParty = 4,

    /// <summary>
    /// 自定义
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("自定义")]
    Custom = 8
}