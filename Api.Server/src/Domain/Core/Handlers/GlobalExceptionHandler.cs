using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Fast.CenterLog.Entity;
using Fast.SqlSugar;
using Fast.UnifyResult;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="GlobalExceptionHandler"/> 全局异常处理
/// </summary>
public class GlobalExceptionHandler : IGlobalExceptionHandler
{
    /// <summary>
    /// SqlSugar实体服务
    /// </summary>
    private readonly ISqlSugarEntityService _sqlSugarEntityService;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="GlobalExceptionHandler"/> 全局异常处理
    /// </summary>
    /// <param name="sqlSugarEntityService"><see cref="ISqlSugarEntityService"/> SqlSugar实体服务</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public GlobalExceptionHandler(ISqlSugarEntityService sqlSugarEntityService, ILogger<IGlobalExceptionHandler> logger)
    {
        _sqlSugarEntityService = sqlSugarEntityService;
        _logger = logger;
    }

    /// <summary>异常拦截</summary>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" /></param>
    /// <param name="isUserFriendlyException"><see cref="T:System.Boolean" /> 是否友好异常</param>
    /// <param name="isValidationException"><see cref="T:System.Boolean" /> 是否验证异常</param>
    /// <returns></returns>
    public async Task OnExceptionAsync(ExceptionContext context, bool isUserFriendlyException,
        bool isValidationException)
    {
        var httpContext = context.HttpContext;
        var message = new StringBuilder();

        try
        {
            //// 判断请求是否已经取消
            //if (!httpContext.RequestAborted.IsCancellationRequested)
            //{
            //}
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
            if (httpContext.Items.TryGetValue($"{nameof(Fast)}.RequestParams", out var requestParams))
            {
                message.AppendLine($"请求参数: {requestParams}");
            }
            else
            {
                message.AppendLine("请求参数: 无");
            }

            if (httpContext.RequestAborted.IsCancellationRequested)
            {
                message.AppendLine("连接被客户端强制关闭。");
                // 写入警告日志
                _logger.LogWarning(message.ToString());
                return;
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
            var firstLine = context.Exception.StackTrace
                ?.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            // 如果有匹配的堆栈信息，选择第一条（最深的那一条）
            if (!string.IsNullOrWhiteSpace(firstLine))
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
            var className = context.Exception.TargetSite?.DeclaringType?.FullName;
            var methodName = "";
            var groupCollection = Regex.Match(className, "<(.*?)>")
                .Groups;
            if (groupCollection.Count > 1)
                methodName = groupCollection[1].Value;

            // 获取 CenterLog 库的连接字符串配置
            var connectionSetting = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
                CommonConst.Default.TenantNo, DatabaseTypeEnum.CenterLog);
            var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

            var _user = httpContext.RequestServices.GetService<IUser>();
            var exceptionLogModel = new ExceptionLogModel
            {
                RecordId = YitIdHelper.NextId(),
                AccountId = _user?.AccountId,
                Account = _user?.Account,
                Mobile = _user?.Mobile,
                NickName = _user?.NickName,
                ClassName = context.Exception.TargetSite?.DeclaringType?.FullName,
                MethodName = methodName,
                Message = context.Exception.Message,
                Source = context.Exception.Source,
                StackTrace = context.Exception.StackTrace,
                ParamsObj = context.Exception.TargetSite?.GetParameters()
                    .ToString(),
                DepartmentId = _user?.DepartmentId,
                DepartmentName = _user?.DepartmentName,
                CreatedUserId = _user?.UserId,
                CreatedUserName = _user?.EmployeeName,
                CreatedTime = DateTime.Now,
                TenantId = _user?.TenantId ?? 0
            };
            exceptionLogModel.RecordCreate(httpContext);

            _ = Task.Run(async () =>
            {
                try
                {
                    // 这里不能使用Aop
                    var db = new SqlSugarClient(connectionConfig);

                    // 异步不等待
                    await db.Insertable(exceptionLogModel)
                        .ExecuteCommandAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Global Exception 执行Sql，保存失败；{ex.Message}");
                }
            });
            // 写入错误日志
            _logger.LogError(context.Exception, message.ToString());
        }

        await Task.CompletedTask;
    }
}