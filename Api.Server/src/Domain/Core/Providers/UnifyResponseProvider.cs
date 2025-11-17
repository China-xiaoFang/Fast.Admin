using Fast.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="UnifyResponseProvider"/> 规范化响应数据提供器
/// </summary>
public class UnifyResponseProvider : IUnifyResponseProvider
{
    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="UnifyResponseProvider"/> 规范化响应数据提供器
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public UnifyResponseProvider(ILogger<IUnifyResponseProvider> logger)
    {
        _logger = logger;
    }

    /// <summary>响应异常处理</summary>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" /></param>
    /// <param name="metadata"><see cref="T:Fast.UnifyResult.ExceptionMetadata" /> 异常元数据</param>
    /// <param name="httpContext"><see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> 请求上下文</param>
    /// <returns></returns>
    public async Task<(int statusCode, string message)> ResponseExceptionAsync(ExceptionContext context,
        ExceptionMetadata metadata, HttpContext httpContext)
    {
        // 默认 500 错误
        var statusCode = StatusCodes.Status500InternalServerError;

        var message = context.Exception.Message;

        switch (context.Exception)
        {
            // 友好异常处理
            case UserFriendlyException userFriendlyException:
            {
                if (userFriendlyException.OriginErrorCode != null)
                {
                    statusCode = userFriendlyException.OriginErrorCode.ToString()
                        .ParseToInt();
                }
                else if (userFriendlyException.ErrorCode != null)
                {
                    statusCode = userFriendlyException.ErrorCode.ToString()
                        .ParseToInt();
                }
                else
                {
                    statusCode = userFriendlyException.StatusCode;
                }

                // 处理可能为0的情况
                statusCode = statusCode == 0 ? StatusCodes.Status400BadRequest : statusCode;
                break;
            }
            // SqlSugar 并发处理
            case VersionExceptions versionExceptions:
                statusCode = StatusCodes.Status400BadRequest;
                message = "数据已更改，请刷新后重试！\r\n" + versionExceptions.Message;
                break;
            // 操作异常
            case InvalidOperationException invalidOperationException:
                statusCode = StatusCodes.Status500InternalServerError;
                if (invalidOperationException.InnerException is SqlException sqlException)
                {
                    message = sqlException.Message;
                }
                else
                {
                    message = invalidOperationException.Message;
                }

                break;
        }

        return await Task.FromResult((statusCode, message));
    }

    /// <summary>响应数据验证异常处理</summary>
    /// <param name="context"><see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" /></param>
    /// <param name="metadata"><see cref="T:Fast.UnifyResult.ValidationMetadata" /> 验证信息元数据</param>
    /// <param name="httpContext"><see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> 请求上下文</param>
    /// <returns></returns>
    public async Task ResponseValidationExceptionAsync(ActionExecutingContext context, ValidationMetadata metadata,
        HttpContext httpContext)
    {
        await Task.CompletedTask;
    }

    /// <summary>响应数据处理</summary>
    /// <remarks>只有响应成功且为正常返回才会调用</remarks>
    /// <param name="timestamp"><see cref="T:System.Int64" /> 响应时间戳</param>
    /// <param name="data"><see cref="T:System.Object" /> 数据</param>
    /// <param name="httpContext"><see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> 请求上下文</param>
    /// <returns></returns>
    public async Task<object> ResponseDataAsync(long timestamp, object data, HttpContext httpContext)
    {
        // 响应加密特性
        var responseEncipher = httpContext.GetEndpoint()
                                   ?.Metadata.GetMetadata<ResponseEncipherAttribute>()
                               != null;

        if (!responseEncipher)
        {
            // 获取应用信息
            var applicationOpenIdModel = await ApplicationContext.GetApplication(GlobalContext.Origin);
            responseEncipher = applicationOpenIdModel.RequestEncipher;
        }

        // 判断是否开启响应加密
        if (!responseEncipher)
            return await Task.FromResult(data);

        // 添加加密头部标识
        httpContext.Response.Headers.TryAdd($"{nameof(Fast)}-Response-Encipher", "True");

        var dataStr = data.ToJsonString();

        // 加密数据
        var encryptedStr = CryptoUtil.AESEncrypt(dataStr, timestamp.ToString(), $"FIV{timestamp}");

        return await Task.FromResult(encryptedStr);
    }
}