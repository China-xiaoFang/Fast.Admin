// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Scheduler;

/// <summary>
/// 获取调度器详情输出
/// </summary>
public class QuerySchedulerDetailOutput
{
    /// <summary>
    /// 是否实际运行
    /// </summary>
    public bool IsStarted { get; set; }

    /// <summary>
    /// Quartz 版本
    /// </summary>
    public string QuartzVersion { get; set; }

    /// <summary>
    /// 期望状态
    /// </summary>
    public string DesiredStatus { get; set; }

    /// <summary>
    /// 调度执行宿主是否在线
    /// </summary>
    public bool ExecutionOnline { get; set; }

    /// <summary>
    /// 实际状态
    /// </summary>
    public string ActualStatus { get; set; }

    /// <summary>
    /// 调度器实际状态
    /// </summary>
    public string SchedulerStatus { get; set; }

    /// <summary>
    /// 调度执行宿主是否离线
    /// </summary>
    public bool SchedulerShutdown { get; set; }

    /// <summary>
    /// 是否期望调度器待机
    /// </summary>
    public bool SchedulerInStandbyMode { get; set; }

    /// <summary>
    /// 调度器是否实际运行
    /// </summary>
    public bool SchedulerStarted { get; set; }

    /// <summary>
    /// 调度器实例Id
    /// </summary>
    public string SchedulerInstanceId { get; set; }

    /// <summary>
    /// 调度器名称
    /// </summary>
    public string SchedulerName { get; set; }

    /// <summary>
    /// 是否远程调度器
    /// </summary>
    public bool SchedulerRemote { get; set; }

    /// <summary>
    /// 调度器类型
    /// </summary>
    public string SchedulerType { get; set; }

    /// <summary>
    /// 持久化类型
    /// </summary>
    public string JobStoreType { get; set; }

    /// <summary>
    /// 强制作业数据映射的值被视为字符串
    /// </summary>
    public bool SupportsPersistence { get; set; }

    /// <summary>
    /// 集群
    /// </summary>
    public bool Clustered { get; set; }

    /// <summary>
    /// 线程池大小
    /// </summary>
    public int ThreadPoolSize { get; set; }

    /// <summary>
    /// 线程池类型
    /// </summary>
    public string ThreadPoolType { get; set; }

    /// <summary>
    /// 调度作业执行数
    /// </summary>
    public int JobExecutedNumber { get; set; }

    /// <summary>
    /// 运行时间
    /// </summary>
    public string RunTimes { get; set; }

    /// <summary>
    /// 调度作业总数量
    /// </summary>
    public int JobCountNumber { get; set; }

    /// <summary>
    /// 触发器总数量
    /// </summary>
    public int TriggerCountNumber { get; set; }

    /// <summary>
    /// 正在执行的调度作业数
    /// </summary>
    public int JobExecuteNumber { get; set; }
}
