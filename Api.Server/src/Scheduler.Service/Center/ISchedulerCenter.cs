// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Scheduler;

/// <summary>
/// 调度中心
/// </summary>
public interface ISchedulerCenter
{
    /// <summary>
    /// 初始化调度程序
    /// </summary>
    Task InitializeScheduler();

    /// <summary>
    /// 同步调度程序
    /// </summary>
    Task SyncScheduler();

    /// <summary>
    /// 同步调度程序运行状态
    /// </summary>
    Task SyncSchedulerState();

    /// <summary>
    /// 获取调度器详情
    /// </summary>
    /// <returns>调度器详情</returns>
    Task<QuerySchedulerDetailOutput> QuerySchedulerDetail(long? tenantId = null);

    /// <summary>
    /// 启动调度器
    /// </summary>
    /// <returns>管理宿主返回期望状态是否保存，执行宿主返回是否实际运行</returns>
    Task<bool> StartScheduler(long? tenantId = null);

    /// <summary>
    /// 停止调度器
    /// </summary>
    /// <returns>管理宿主返回期望状态是否保存，执行宿主返回是否实际待机</returns>
    Task<bool> StopScheduler(long? tenantId = null);

    /// <summary>
    /// 暂停调度作业
    /// </summary>
    Task StopSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 恢复调度作业
    /// </summary>
    Task ResumeSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 立即执行调度作业
    /// </summary>
    Task TriggerSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 是否存在调度作业
    /// </summary>
    /// <returns>调度作业是否存在</returns>
    Task<bool> ExistsSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 获取调度作业日志
    /// </summary>
    /// <returns>调度作业日志</returns>
    Task<List<string>> QuerySchedulerJobLogs(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 获取调度作业运行次数
    /// </summary>
    /// <returns>调度作业累计运行次数</returns>
    Task<long> QuerySchedulerJobRunNumber(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 获取全部调度作业
    /// </summary>
    /// <returns>调度作业列表</returns>
    Task<List<QueryAllSchedulerJobOutput>> QueryAllSchedulerJob(SchedulerJobGroupEnum? jobGroup = null, long? tenantId = null);

    /// <summary>
    /// 获取调度作业
    /// </summary>
    /// <returns>调度作业信息</returns>
    Task<SchedulerJobInfo> QuerySchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 添加调度作业
    /// </summary>
    Task AddSchedulerJob(AddSchedulerJobInput input);

    /// <summary>
    /// 编辑调度作业
    /// </summary>
    /// <remarks>注：这里更新作业会导致触发器的执行记录被清空。所以会导致更新后可能会立即执行一次</remarks>
    Task EditSchedulerJob(EditSchedulerJobInput input);

    /// <summary>
    /// 删除调度作业
    /// </summary>
    Task DeleteSchedulerJob(SchedulerJobKeyInput input, long? tenantId = null);

    /// <summary>
    /// 移除调度作业异常信息
    /// </summary>
    /// <remarks>因为只能在 <see cref="Quartz.IJob"/> 中持久化修改 <see cref="Quartz.JobDataMap"/>，所以这里直接操作数据库</remarks>
    Task DeleteSchedulerJobException(SchedulerJobKeyInput input, long? tenantId = null);
}
