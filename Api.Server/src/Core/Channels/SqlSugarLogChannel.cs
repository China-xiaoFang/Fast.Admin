// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

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
