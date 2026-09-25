// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

// ReSharper disable once CheckNamespace

namespace Quartz;

/// <summary>
/// 支持依赖注入的调度器工厂
/// </summary>
public interface IDependencySchedulerFactory
{
    /// <summary>
    /// 获取调度器
    /// </summary>
    /// <param name="tenantId">租户Id；未指定时获取默认调度器</param>
    /// <param name="cancellationToken">用于取消异步操作的令牌</param>
    /// <returns>调度器实例</returns>
    Task<IScheduler> GetScheduler(long? tenantId = null, CancellationToken cancellationToken = new());

    /// <summary>
    /// 获取调度器
    /// </summary>
    /// <param name="schedulerName">调度器名称</param>
    /// <param name="cancellationToken">用于取消异步操作的令牌</param>
    /// <returns>调度器实例</returns>
    Task<IScheduler> GetScheduler(string schedulerName, CancellationToken cancellationToken = new());

    /// <summary>
    /// 尝试获取调度器
    /// </summary>
    /// <returns>找到的调度器；不存在时为 <see langword="null"/></returns>
    Task<IScheduler> TryGetScheduler(string schedulerName);

    /// <summary>
    /// 获取所有调度器
    /// </summary>
    /// <param name="cancellationToken">用于取消异步操作的令牌</param>
    /// <returns>所有已注册的调度器</returns>
    Task<IReadOnlyList<IScheduler>> GetAllSchedulers(CancellationToken cancellationToken = new());
}
