using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="DeleteLogHostedService"/> 删除日志托管服务
/// </summary>
[Order(1)]
public class DeleteLogHostedService : IHostedService
{
    /// <summary>
    /// 最大保留天数
    /// </summary>
    private const int MaxRetainDay = 90;

    /// <summary>
    /// 日志文件正则表达式
    /// </summary>
    private readonly Regex LogRegex = new(@"^(?:\\[^\\]+)*\\[^\\]+\.log$", RegexOptions.IgnoreCase);

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="DeleteLogHostedService"/> 删除日志托管服务
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public DeleteLogHostedService(ILogger<DeleteLogHostedService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 处理空文件夹
    /// </summary>
    /// <param name="stopDirectory"></param>
    /// <param name="directoryInfo"></param>
    private void HandleEmptyDirectory(string stopDirectory, DirectoryInfo directoryInfo)
    {
        while (directoryInfo != null)
        {
            // 如果和日志文件相同则停止
            if (string.Equals(directoryInfo.FullName, stopDirectory, StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            // 检查目录是否为空
            if (!directoryInfo.EnumerateFileSystemInfos()
                    .Any())
            {
                // 上级目录
                var parent = directoryInfo.Parent;
                directoryInfo.Delete();
                directoryInfo = parent;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous Start operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var logPath = Path.Combine(Environment.CurrentDirectory, "logs");

        if (Directory.Exists(logPath))
        {
            // 空文件数量
            var emptyFileNum = 0;
            var oldFileNum = 0;

            _ = Task.Run(() =>
            {
                try
                {
                    // 查找当前目录下所有的文件
                    var files = Directory.GetFiles(logPath, "*.*", SearchOption.AllDirectories);

                    // 过滤符合条件的文件
                    var matchedFiles = files.Select(sl =>
                        {
                            var relativePath = sl.Substring(logPath.Length);
                            if (!relativePath.StartsWith("\\"))
                            {
                                relativePath = "\\" + relativePath;
                            }

                            return new { FullPath = sl, RelativePath = relativePath };
                        })
                        .Where(wh => LogRegex.IsMatch(wh.RelativePath))
                        .ToList();

                    foreach (var item in matchedFiles)
                    {
                        var fileInfo = new FileInfo(item.FullPath);

                        // 删除空文件
                        if (fileInfo.Length == 0)
                        {
                            try
                            {
                                File.Delete(item.FullPath);
                                HandleEmptyDirectory(logPath, new DirectoryInfo(Path.GetDirectoryName(item.FullPath)));
                                emptyFileNum++;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        // 删除超过最大保留天数的文件
                        else if ((DateTime.Now.Date - fileInfo.LastWriteTime.Date).TotalDays > MaxRetainDay)
                        {
                            try
                            {
                                File.Delete(item.FullPath);
                                HandleEmptyDirectory(logPath, new DirectoryInfo(Path.GetDirectoryName(item.FullPath)));
                                oldFileNum++;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Delete log error...");
                }

                var logSb = new StringBuilder();
                logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
                logSb.Append("info");
                logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                logSb.Append(": ");
                logSb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
                logSb.Append(Environment.NewLine);
                logSb.Append("\u001b[40m\u001b[90m");
                logSb.Append("      ");
                logSb.Append($"删除日志文件，空文件 {emptyFileNum} 个，超过最长保留{MaxRetainDay}天文件 {oldFileNum} 个。");
                logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                Console.WriteLine(logSb.ToString());
            }, cancellationToken);
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous Stop operation.</returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}