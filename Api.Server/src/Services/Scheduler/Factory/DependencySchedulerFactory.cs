using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Util;


// ReSharper disable once CheckNamespace
namespace Quartz;

/// <summary>
/// <see cref="DependencySchedulerFactory"/> DI注入调度器工厂
/// </summary>
internal class DependencySchedulerFactory : StdSchedulerFactory, IDependencySchedulerFactory
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 容器配置处理器
    /// </summary>
    private readonly ContainerConfigurationProcessor _processor;

    /// <summary>
    /// 调度器仓库
    /// </summary>
    private readonly ISchedulerRepository _schedulerRepository;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 异步线程锁
    /// </summary>
    private static readonly SemaphoreSlim semaphoreSlim = new(1, 1);

    public DependencySchedulerFactory(IServiceProvider serviceProvider, ContainerConfigurationProcessor processor,
        ISchedulerRepository schedulerRepository, ILogger<IDependencySchedulerFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _processor = processor;
        _schedulerRepository = schedulerRepository;
        _logger = logger;
    }

    /// <summary>
    /// 获取调度器
    /// </summary>
    /// <param name="tenantId"><see cref="long"/> 租户Id，如果不传入则获取的是默认调度器</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IScheduler> GetScheduler(long? tenantId = null, CancellationToken cancellationToken = new())
    {
        // 获取锁
        await semaphoreSlim.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            // 调度器名称
            var schedulerName = tenantId != null ? $"TenantScheduler_{tenantId}" : "CoreScheduler";

            // 判断是否已经存在调度器
            var scheduler = _schedulerRepository.Lookup(schedulerName);

            if (scheduler != null)
            {
                return scheduler;
            }

            // 获取选项
            var options = _serviceProvider.GetService<IOptions<QuartzOptions>>();

            // 放入选项
            options.Value[PropertySchedulerInstanceName] = schedulerName;

            /*
             * 初始化配置
             * 这里是直接覆盖选项
             * 虽然我不知道覆盖后续会有问题问题发生
             * 但是居于目前 v3.13.1 版本，经过本人两天研究源码发现除了在改变源码的情况下，没有更好的解决方案了
             */
            Initialize(options.Value.ToNameValueCollection());

            // 获取调度器
            scheduler = await GetScheduler(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation(tenantId != null
                ? $"Create tenant scheduler by {tenantId} success."
                : "Create scheduler success.");

            return scheduler;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                tenantId != null ? $"Create tenant scheduler by {tenantId} error." : "Create scheduler error.");
            throw;
        }
        finally
        {
            // 释放锁
            semaphoreSlim.Release();
        }
    }

    /// <summary>
    /// 获取调度器
    /// </summary>
    /// <param name="schedulerName"><see cref="string"/> 调度器名称</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<IScheduler> GetScheduler(string schedulerName,
        CancellationToken cancellationToken = new())
    {
        // 获取锁
        await semaphoreSlim.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            // 判断是否已经存在调度器
            var scheduler = _schedulerRepository.Lookup(schedulerName);

            if (scheduler != null)
            {
                return scheduler;
            }

            // 获取选项
            var options = _serviceProvider.GetService<IOptions<QuartzOptions>>();

            // 放入选项
            options.Value[PropertySchedulerInstanceName] = schedulerName;

            /*
             * 初始化配置
             * 这里是直接覆盖选项
             * 虽然我不知道覆盖后续会有问题问题发生
             * 但是居于目前 v3.13.1 版本，经过本人两天研究源码发现除了在改变源码的情况下，没有更好的解决方案了
             */
            Initialize(options.Value.ToNameValueCollection());

            // 获取调度器
            scheduler = await GetScheduler(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation($"Create {schedulerName} scheduler success.");

            return scheduler;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create {schedulerName} scheduler error.");
            throw;
        }
        finally
        {
            // 释放锁
            semaphoreSlim.Release();
        }
    }

    /// <summary>
    /// 尝试获取调度器
    /// </summary>
    /// <param name="schedulerName"><see cref="string"/> 调度器名称</param>
    /// <returns></returns>
    public async Task<IScheduler> TryGetScheduler(string schedulerName)
    {
        return await Task.FromResult(_schedulerRepository.Lookup(schedulerName));
    }

    /// <summary>
    /// 获取所有调度器
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<IReadOnlyList<IScheduler>> GetAllSchedulers(CancellationToken cancellationToken = new())
    {
        return await Task.FromResult(_schedulerRepository.LookupAll());
    }

    /// <summary>
    /// Returns a handle to the Scheduler produced by this factory.
    /// </summary>
    /// <remarks>
    /// If one of the <see cref="M:Quartz.Impl.StdSchedulerFactory.Initialize" /> methods has not be previously
    /// called, then the default (no-arg) <see cref="M:Quartz.Impl.StdSchedulerFactory.Initialize" /> method
    /// will be called by this method.
    /// </remarks>
    public override async Task<IScheduler> GetScheduler(CancellationToken cancellationToken = default)
    {
        // 获取调度器，这里需要注意的是：如果原来的调度器被停止了，则调用 GetScheduler 会返回一个新的调度器。
        var scheduler = await base.GetScheduler(cancellationToken)
            .ConfigureAwait(false);

        // 初始化调度器
        await InitializeScheduler(scheduler, cancellationToken)
            .ConfigureAwait(false);

        return scheduler;
    }

    /// <summary>
    /// 初始化调度器
    /// </summary>
    /// <param name="scheduler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task InitializeScheduler(IScheduler scheduler, CancellationToken cancellationToken)
    {
        foreach (var listener in _serviceProvider.GetServices<ISchedulerListener>())
        {
            scheduler.ListenerManager.AddSchedulerListener(listener);
        }

        var jobListeners = _serviceProvider.GetServices<IJobListener>();
        var jobListenerConfigurations = _serviceProvider.GetServices<JobListenerConfiguration>()
            .ToArray();
        foreach (var listener in jobListeners)
        {
            var configuration = jobListenerConfigurations.SingleOrDefault(x => x.ListenerType == listener.GetType());
            scheduler.ListenerManager.AddJobListener(listener, configuration?.Matchers ?? []);
        }

        var triggerListeners = _serviceProvider.GetServices<ITriggerListener>();
        var triggerListenerConfigurations = _serviceProvider.GetServices<TriggerListenerConfiguration>()
            .ToArray();
        foreach (var listener in triggerListeners)
        {
            var configuration =
                triggerListenerConfigurations.SingleOrDefault(x => x.ListenerType == listener.GetType());
            scheduler.ListenerManager.AddTriggerListener(listener, configuration?.Matchers ?? []);
        }

        var calendars = _serviceProvider.GetServices<CalendarConfiguration>();
        foreach (var configuration in calendars)
        {
            await scheduler.AddCalendar(configuration.Name, configuration.Calendar, configuration.Replace,
                    configuration.UpdateTriggers, cancellationToken)
                .ConfigureAwait(false);
        }

        await _processor.ScheduleJobs(scheduler, cancellationToken)
            .ConfigureAwait(false);
    }

    protected override ISchedulerRepository GetSchedulerRepository()
    {
        return _schedulerRepository;
    }

    protected override IDbConnectionManager GetDBConnectionManager()
    {
        return _serviceProvider.GetService<IDbConnectionManager>();
    }

    protected override string GetNamedConnectionString(string connectionStringName)
    {
        var configuration = _serviceProvider.GetService<IConfiguration>();
        var connectionString = configuration?.GetConnectionString(connectionStringName);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        return base.GetNamedConnectionString(connectionStringName);
    }

    protected override T InstantiateType<T>(Type implementationType)
    {
        var service = _serviceProvider.GetService<T>();
        if (service is null)
        {
            service = ObjectUtils.InstantiateType<T>(implementationType);
        }

        return service;
    }
}