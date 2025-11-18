

// ReSharper disable once CheckNamespace
namespace Quartz;

/// <summary>
/// <see cref="IDependencySchedulerFactory"/> DI注入调度器工厂
/// </summary>
public interface IDependencySchedulerFactory
{
    /// <summary>
    /// 获取调度器
    /// </summary>
    /// <param name="tenantId"><see cref="long"/> 租户Id，如果不传入则获取的是默认调度器</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IScheduler> GetScheduler(long? tenantId = null, CancellationToken cancellationToken = new());

    /// <summary>
    /// 获取调度器
    /// </summary>
    /// <param name="schedulerName"><see cref="string"/> 调度器名称</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IScheduler> GetScheduler(string schedulerName, CancellationToken cancellationToken = new());

    /// <summary>
    /// 尝试获取调度器
    /// </summary>
    /// <param name="schedulerName"><see cref="string"/> 调度器名称</param>
    /// <returns></returns>
    Task<IScheduler> TryGetScheduler(string schedulerName);

    /// <summary>
    /// 获取所有调度器
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<IScheduler>> GetAllSchedulers(CancellationToken cancellationToken = new());
}