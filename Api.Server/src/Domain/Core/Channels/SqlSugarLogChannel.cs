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

using System.Threading.Channels;
using SqlSugar;

namespace Fast.Core;

/// <summary>
/// Sql日志专用有界通道
/// </summary>
/// <remarks>
/// 通道只负责接收 <see cref="SqlSugarEntityHandler"/> 产生的四类 SQL 日志
/// 写入方使用 <see cref="ChannelWriter{T}.WriteAsync(T, CancellationToken)"/>，容量耗尽时等待空位，不会静默丢弃日志
/// </remarks>
public sealed class SqlSugarLogChannel : ISingletonDependency
{
    /// <summary>
    /// 通道容量。单消费者逐条持久化，容量只用于吸收短时请求峰值
    /// </summary>
    private const int Capacity = 2048;

    /// <summary>
    /// SQL 日志通道工作项
    /// </summary>
    /// <param name="ConnectionConfig">日志持久化使用的数据库连接配置</param>
    /// <param name="LogModel">待持久化的 SQL 日志实体</param>
    internal readonly record struct SqlSugarLogWorkItem(ConnectionConfig ConnectionConfig, object LogModel);

    /// <summary>
    /// 存放待持久化 SQL 日志的单消费者通道
    /// </summary>
    private readonly Channel<SqlSugarLogWorkItem> _channel = Channel.CreateBounded<SqlSugarLogWorkItem>(
        new BoundedChannelOptions(Capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false
        });

    /// <summary>
    /// 等待日志进入通道；通道已满时等待消费者释放空位
    /// </summary>
    /// <param name="connectionConfig">日志数据库连接配置</param>
    /// <param name="logModel">待持久化的 SQL 日志实体</param>
    internal ValueTask WriteAsync(ConnectionConfig connectionConfig, object logModel)
    {
        ArgumentNullException.ThrowIfNull(connectionConfig);
        ArgumentNullException.ThrowIfNull(logModel);
        return _channel.Writer.WriteAsync(new SqlSugarLogWorkItem(connectionConfig, logModel));
    }

    /// <summary>
    /// 按进入通道的先后顺序读取日志
    /// </summary>
    /// <param name="cancellationToken">消费者停止标记</param>
    /// <returns>可异步枚举的 SQL 日志序列</returns>
    internal IAsyncEnumerable<SqlSugarLogWorkItem> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }

    /// <summary>
    /// 关闭写入端，让消费者处理完通道中的现有日志后退出
    /// </summary>
    internal void Complete()
    {
        _channel.Writer.TryComplete();
    }
}