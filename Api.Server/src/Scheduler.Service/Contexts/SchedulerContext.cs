// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using System.Collections.Concurrent;

namespace Fast.Scheduler;

/// <summary>
/// 作业调度程序上下文
/// </summary>
[SuppressSniffer]
public class SchedulerContext
{
    /// <summary>
    /// 是否为调度执行宿主
    /// </summary>
    public static bool IsExecutionHost { get; internal set; }

    /// <summary>
    /// 初始化
    /// </summary>
    internal static bool Initialized { get; set; } = false;

    /// <summary>
    /// 已经存在调度器的 TenantId
    /// <para>租户名称</para>
    /// <para>租户编号</para>
    /// <para>虚拟的设备Id</para>
    /// </summary>
    public static ConcurrentDictionary<long, (string tenantName, string tenantNo, string tenantCode, string deviceId)>
        SchedulerTenantList { get; internal set; } = [];

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
