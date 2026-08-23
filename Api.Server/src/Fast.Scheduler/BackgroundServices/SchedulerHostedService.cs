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

namespace Fast.Scheduler.BackgroundServices;

/// <summary>
/// 调度后台托管服务
/// </summary>
public class SchedulerHostedService : BackgroundService
{
    /// <summary>
    /// 托管应用程序生命周期
    /// </summary>
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    /// <summary>
    /// 调度中心
    /// </summary>
    private readonly ISchedulerCenter _schedulerCenter;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 调度后台托管服务
    /// </summary>
    public SchedulerHostedService(IHostApplicationLifetime hostApplicationLifetime, ISchedulerCenter schedulerCenter,
        ILogger<SchedulerHostedService> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _schedulerCenter = schedulerCenter;
        _logger = logger;
    }

    /// <summary>
    /// 执行
    /// </summary>
    private async Task SyncSchedulerAsync()
    {
        try
        {
            // 同步调度程序
            await _schedulerCenter.SyncScheduler();
        }
        catch (Exception ex)
        {
            // 同步调度器错误
            _logger.LogError(ex, $"Sync scheduler error. {ex.Message}");
        }
    }

    /// <summary>
    /// 同步调度程序运行状态
    /// </summary>
    private async Task SyncSchedulerStateAsync()
    {
        try
        {
            await _schedulerCenter.SyncSchedulerState();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Sync scheduler state error. {ex.Message}");
        }
    }

    /// <summary>
    /// 在应用启动完成后初始化调度器，并每半小时同步一次调度定义
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!_hostApplicationLifetime.ApplicationStarted.IsCancellationRequested)
            {
                var startedCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                await using var startedRegistration =
                    _hostApplicationLifetime.ApplicationStarted.Register(() => startedCompletion.TrySetResult());
                await using var stoppingRegistration = stoppingToken.Register(() => startedCompletion.TrySetCanceled(stoppingToken));
                await startedCompletion.Task;
            }

            try
            {
                // 初始化调度程序；初始化失败时不能继续提供半可用的调度服务
                await _schedulerCenter.InitializeScheduler();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Initialize scheduler error. Scheduler host will stop.");
                _hostApplicationLifetime.StopApplication();
                return;
            }

            // 启动完成后立即同步一次，确保运行期间新增的租户调度器能够及时加载
            await SyncSchedulerAsync();

            var dateTime = DateTime.Now;
            var nextExecTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                dateTime.Minute < 30 ? 30 : 0, 0);
            if (nextExecTime <= dateTime)
                nextExecTime = nextExecTime.AddHours(1);

            _logger.LogInformation("Next execute sync scheduler time {NextExecuteTime:yyyy-MM-dd HH:mm:ss}", nextExecTime);

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await SyncSchedulerStateAsync();

                if (DateTime.Now < nextExecTime)
                {
                    continue;
                }

                // 串行等待每次同步完成，禁止 Timer 回调重叠造成重复调度或数据库并发写入
                await SyncSchedulerAsync();
                do
                {
                    nextExecTime = nextExecTime.AddMinutes(30);
                } while (nextExecTime <= DateTime.Now);

                _logger.LogInformation("Next execute sync scheduler time {NextExecuteTime:yyyy-MM-dd HH:mm:ss}", nextExecTime);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // 宿主停止时取消启动等待、延迟或定时器属于正常停机流程
        }
    }
}