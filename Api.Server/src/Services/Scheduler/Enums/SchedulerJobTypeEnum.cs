// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerJobTypeEnum"/> 调度作业类型枚举
/// </summary>
[Flags]
[FastEnum("调度作业类型枚举")]
public enum SchedulerJobTypeEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("无")] None = 0,

    /// <summary>
    /// 本地
    /// </summary>
    [Description("本地")] Local = 1,

    /// <summary>
    /// 内网Url
    /// </summary>
    /// <remarks>自动处理 AccessToken</remarks>
    [Description("内网Url")] IntranetUrl = 2,

    /// <summary>
    /// 外网Url
    /// </summary>
    [Description("外网Url")] OuterNetUrl = 4
}