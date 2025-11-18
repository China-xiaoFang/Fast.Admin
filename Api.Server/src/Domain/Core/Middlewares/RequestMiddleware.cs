using System.Text;
using Fast.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Fast.Core;

/// <summary>
/// <see cref="RequestMiddleware"/> 请求中间件
/// </summary>
[SuppressSniffer]
public class RequestMiddleware
{
    /// <summary>
    /// 请求委托
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="RequestMiddleware"/> 请求中间件
    /// </summary>
    /// <param name="next"><see cref="RequestDelegate"/> 请求委托</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// 中间件执行方法
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var httpRequest = httpContext.Request;

        // 排除 WebSocket
        if (httpContext.IsWebSocketRequest())
        {
            await _next(httpContext);
            return;
        }

        // 排除 multipart/form-data 格式
        if (httpRequest.HasFormContentType)
        {
            // 写入 HttpContext.Items
            httpContext.Items[$"{nameof(Fast)}.RequestParams"] = "文件上传...";

            await _next(httpContext);
            return;
        }

        var isReadBody =
            !(httpRequest.Method == HttpMethod.Get.Method || httpRequest.Method == HttpMethod.Delete.Method);

        // Url请求参数
        IDictionary<string, string> queryParamDic = null;
        // Body请求参数
        var bodyParam = "";
        // 解密数据
        var decryptedData = "";

        if (!isReadBody)
        {
            queryParamDic = httpRequest.Query.ToDictionary(e => e.Key, e => e.Value.ToString());
        }
        else
        {
            // 允许读取请求的Body
            httpContext.Request.EnableBuffering();
            using var streamReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
            bodyParam = await streamReader.ReadToEndAsync();
            // 重置指针
            httpContext.Request.Body.Position = 0;
        }

        // 请求解密
        var requestEncipher = false;

        // 判断是否存在加密头部标识
        if (httpRequest.Headers.TryGetValue($"{nameof(Fast)}-request-Encipher", out var requestEncipherStr))
        {
            bool.TryParse(requestEncipherStr, out requestEncipher);
        }

        try
        {
            if (requestEncipher)
            {
                if (!isReadBody && queryParamDic?.Count > 0)
                {
                    // 解密数据
                    decryptedData = CryptoUtil.AESDecrypt(queryParamDic["data"], queryParamDic["timestamp"],
                        $"FIV{queryParamDic["timestamp"]}");

                    // 反序列化成键值对
                    var model = decryptedData.ToObject<Dictionary<string, string>>();

                    // 替换 QueryString
                    httpRequest.QueryString = QueryString.Create(model);
                }
                else if (isReadBody && !string.IsNullOrWhiteSpace(bodyParam))
                {
                    // 解析Json，取出 data 和 timestamp 字段
                    var encryptedData = bodyParam.ToObject<RestfulResult<string>>();

                    // 解密数据
                    decryptedData = CryptoUtil.AESDecrypt(encryptedData.Data, encryptedData.Timestamp.ToString(),
                        $"FIV{encryptedData.Timestamp}");

                    // 写入 Body
                    httpRequest.Body = new MemoryStream(Encoding.UTF8.GetBytes(decryptedData));

                    // 重置指针
                    httpContext.Request.Body.Position = 0;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "请求数据格式错误！");
            throw new HttpRequestException("请求数据格式错误！", ex);
        }
        finally
        {
            // 写入 HttpContext.Items
            httpContext.Items[$"{nameof(Fast)}.RequestParams"] = decryptedData;
        }

        await _next(httpContext);
    }
}