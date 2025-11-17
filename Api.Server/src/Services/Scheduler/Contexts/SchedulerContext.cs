using System.Collections.Concurrent;

// ReSharper disable once CheckNamespace
namespace Fast.Scheduler;

/// <summary>
/// <see cref="SchedulerContext"/> 作业调度程序上下文
/// </summary>
[SuppressSniffer]
public class SchedulerContext
{
    /// <summary>
    /// 初始化
    /// </summary>
    internal static bool Initialized { get; set; } = false;

    /// <summary>
    /// 已经存在调度器的 TenantId
    /// <para>租户名称</para>
    /// <para>租户编号</para>
    /// <para>虚拟的设备ID</para>
    /// </summary>
    public static ConcurrentDictionary<long, (string tenantName, string tenantNo, string deviceId)> SchedulerTenantList
    {
        get;
        internal set;
    } = [];

    /// <summary>
    /// 本地调度作业类型
    /// </summary>
    public static ConcurrentDictionary<string, Type> LocalSchedulerJobTypes { get; internal set; } =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 本地调度作业Entity
    /// </summary>
    public static List<SchedulerLocalJobInfo> LocalSchedulerJobList { get; internal set; } = [];
}