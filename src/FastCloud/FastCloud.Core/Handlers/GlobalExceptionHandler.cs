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

using System.Net.Sockets;
using System.Text;
using Fast.UnifyResult;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="GlobalExceptionHandler"/> 全局异常处理
/// </summary>
public class GlobalExceptionHandler : IGlobalExceptionHandler
{
    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public GlobalExceptionHandler(ILogger<IGlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>异常拦截</summary>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" /></param>
    /// <param name="isUserFriendlyException"><see cref="T:System.Boolean" /> 是否友好异常</param>
    /// <param name="isValidationException"><see cref="T:System.Boolean" /> 是否验证异常</param>
    /// <returns></returns>
    public async Task OnExceptionAsync(ExceptionContext context, bool isUserFriendlyException, bool isValidationException)
    {
        var message = new StringBuilder();

        try
        {
            var httpContext = context.HttpContext;
            // 判断请求是否已经取消
            if (!httpContext.RequestAborted.IsCancellationRequested)
            {
                message.AppendLine(context.Exception.Message);
                message.AppendLine(
                    $"请求Url：{httpContext.Request.Method}, {httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}");

                var deviceType = httpContext.Request.Headers[HttpHeaderConst.DeviceType]
                    .ToString()
                    .UrlDecode();
                var deviceId = httpContext.Request.Headers[HttpHeaderConst.DeviceId]
                    .ToString()
                    .UrlDecode();

                message.AppendLine($"device: {deviceType}, {deviceId}");

                switch (httpContext.Request.Method)
                {
                    case "GET":
                    case "DELETE":
                        if (httpContext.Request.Path.HasValue)
                        {
                            var queryParam = httpContext.Request.QueryString;
                            message.AppendLine($"请求参数: {queryParam}");
                        }

                        break;
                    case "POST":
                    case "PUT":
                        // 允许读取请求的Body
                        httpContext.Request.EnableBuffering();

                        // 重置指针
                        httpContext.Request.Body.Position = 0;

                        if (httpContext.Request.Path.HasValue)
                        {
                            if (httpContext.Request.ContentType?.Contains("multipart/form-data") == true)
                            {
                                // 文件上传
                                message.AppendLine("请求参数: 文件上传...");
                            }
                            else
                            {
                                // 请求参数
                                using var streamReader = new StreamReader(httpContext.Request.Body,
                                    Encoding.UTF8,
                                    leaveOpen: true);
                                var requestParam = await streamReader.ReadToEndAsync();
                                message.AppendLine($"请求参数: {requestParam}");

                                // 重置指针
                                httpContext.Request.Body.Position = 0;
                            }
                        }

                        break;
                }
            }
        }
        // 客户端中途断开
        catch (ConnectionResetException)
        {
            message.AppendLine("连接被客户端强制关闭。");
            // 写入警告日志
            _logger.LogWarning(message.ToString());
            return;
        }
        catch (SocketException socketException) when (socketException.SocketErrorCode == SocketError.ConnectionReset)
        {
            message.AppendLine("连接被客户端强制关闭。");
            // 写入警告日志
            _logger.LogWarning(message.ToString());
            return;
        }
        // Kestrel 封装的管道读写抛出 IOException
        catch (IOException ioException) when (ioException.InnerException is SocketException
                                              {
                                                  SocketErrorCode: SocketError.ConnectionReset
                                              })
        {
            message.AppendLine("连接被客户端强制关闭。");
            // 写入警告日志
            _logger.LogWarning(message.ToString());
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(context.Exception, "全局异常原始错误。");
            _logger.LogError(ex, "全局异常拦截失败。");
        }

        // 判断是否为友好异常
        if (isUserFriendlyException)
        {
            // 只写入最深的一条堆栈信息
            var firstLine = context.Exception.StackTrace?.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            // 如果有匹配的堆栈信息，选择第一条（最深的那一条）
            if (!string.IsNullOrEmpty(firstLine))
            {
                message.AppendLine($"{firstLine}");
            }
            else
            {
                message.AppendLine("未找到堆栈信息...");
            }

            // 写入警告日志
            _logger.LogWarning(message.ToString());
        }
        // 判断是否为验证异常
        else if (isValidationException)
        {
            // 写入警告日志
            _logger.LogWarning(message.ToString());
        }
        else
        {
            // 写入错误日志
            _logger.LogError(context.Exception, message.ToString());
        }

        await Task.CompletedTask;
    }
}