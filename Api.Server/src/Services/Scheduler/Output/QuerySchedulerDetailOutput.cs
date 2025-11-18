// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

namespace Fast.Scheduler;

/// <summary>
/// <see cref="QuerySchedulerDetailOutput"/> 获取调度器详情输出
/// </summary>
public class QuerySchedulerDetailOutput
{
    /// <summary>
    /// 是否启动
    /// </summary>
    public bool IsStarted { get; set; }

    /// <summary>
    /// Quartz 版本
    /// </summary>
    public string QuartzVersion { get; set; }

    /// <summary>
    /// 调度器状态
    /// </summary>
    public string SchedulerStatus { get; set; }

    /// <summary>
    /// 调度器关闭
    /// </summary>
    public bool SchedulerShutdown { get; set; }

    /// <summary>
    /// 调度器待机
    /// </summary>
    public bool SchedulerInStandbyMode { get; set; }

    /// <summary>
    /// 调度器启动
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