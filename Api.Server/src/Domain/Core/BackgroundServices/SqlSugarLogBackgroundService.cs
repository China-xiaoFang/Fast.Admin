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

using Fast.CenterLog.Entity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Fast.Core;

/// <summary>
/// 逐条持久化 <see cref="SqlSugarEntityHandler"/> 产生的 SQL 日志
/// </summary>
/// <remarks>
/// 服务固定使用单消费者，不批量合并日志；停止宿主时先关闭写入端，再等待通道中的现有日志排空
/// </remarks>
[Order(101)]
public sealed class SqlSugarLogBackgroundService : BackgroundService
{
    /// <summary>
    /// SQL 日志专用通道
    /// </summary>
    private readonly SqlSugarLogChannel _sqlSugarLogChannel;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 初始化 SQL 日志消费者
    /// </summary>
    public SqlSugarLogBackgroundService(SqlSugarLogChannel sqlSugarLogChannel, ILogger<SqlSugarLogBackgroundService> logger)
    {
        _sqlSugarLogChannel = sqlSugarLogChannel;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await foreach (var workItem in _sqlSugarLogChannel.ReadAllAsync(stoppingToken))
            {
                try
                {
                    // 独立客户端不加载 AOP，避免写日志时再次产生 SQL 日志
                    using var db = new SqlSugarClient(workItem.ConnectionConfig);
                    switch (workItem.LogModel)
                    {
                        case SqlExecutionLogModel sqlExecutionLogModel:
                            await db.Insertable(sqlExecutionLogModel)
                                .SplitTable()
                                .ExecuteCommandAsync();
                            break;
                        case SqlTimeoutLogModel sqlTimeoutLogModel:
                            await db.Insertable(sqlTimeoutLogModel)
                                .ExecuteCommandAsync(stoppingToken);
                            break;
                        case SqlDiffLogModel sqlDiffLogModel:
                            await db.Insertable(sqlDiffLogModel)
                                .SplitTable()
                                .ExecuteCommandAsync();
                            break;
                        case SqlExceptionLogModel sqlExceptionLogModel:
                            await db.Insertable(sqlExceptionLogModel)
                                .ExecuteCommandAsync(stoppingToken);
                            break;
                        default:
                            _logger.LogError("不支持的 SQL 日志类型 {LogType}。", workItem.LogModel.GetType()
                                .FullName);
                            break;
                    }
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "SQL 日志 {LogType} 写入失败。", workItem.LogModel.GetType()
                        .Name);
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // 宿主停止时取消通道读取或当前日志写入属于正常停机流程
        }
    }

    /// <inheritdoc />
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // 正常停机先拒绝新日志，再等待通道排空；超过宿主停机时限后由基类取消消费者
        _sqlSugarLogChannel.Complete();

        if (ExecuteTask == null)
            return;

        try
        {
            await ExecuteTask.WaitAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}