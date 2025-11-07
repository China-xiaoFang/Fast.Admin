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

using System.Text;
using Fast.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
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

        // Url请求参数
        var queryParam = "";
        // Body请求参数
        var bodyParam = "";

        if (httpRequest.Method == HttpMethod.Get.Method || httpRequest.Method == HttpMethod.Delete.Method)
        {
            queryParam = httpRequest.QueryString.ToString();
        }
        else if (httpRequest.Method == HttpMethod.Post.Method || httpRequest.Method == HttpMethod.Put.Method)
        {
            // 允许读取请求的Body
            httpContext.Request.EnableBuffering();
            using var streamReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
            bodyParam = await streamReader.ReadToEndAsync();
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
                if (httpRequest.Method == HttpMethod.Get.Method || httpRequest.Method == HttpMethod.Delete.Method)
                {
                    if (!string.IsNullOrWhiteSpace(queryParam))
                    {
                        // 解析Json，取出 data 和 timestamp 字段
                        var encryptedData = queryParam.ToObject<RestfulResult<string>>();

                        // 解密数据
                        queryParam = CryptoUtil.AESDecrypt(encryptedData.Data, encryptedData.Timestamp.ToString(),
                            $"FIV{encryptedData.Timestamp}");

                        // 反序列化成键值对
                        var model = queryParam.ToObject<Dictionary<string, string>>();

                        // 替换 QueryString
                        httpRequest.QueryString = QueryString.Create(model);
                    }
                }
                else if (httpRequest.Method == HttpMethod.Post.Method || httpRequest.Method == HttpMethod.Put.Method)
                {
                    if (!string.IsNullOrWhiteSpace(bodyParam))
                    {
                        // 解析Json，取出 data 和 timestamp 字段
                        var encryptedData = bodyParam.ToObject<RestfulResult<string>>();

                        // 解密数据
                        bodyParam = CryptoUtil.AESDecrypt(encryptedData.Data, encryptedData.Timestamp.ToString(),
                            $"FIV{encryptedData.Timestamp}");

                        // 写入 Body
                        httpRequest.Body = new MemoryStream(Encoding.UTF8.GetBytes(bodyParam));

                        // 重置指针
                        httpContext.Request.Body.Position = 0;
                    }
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
            httpContext.Items[$"{nameof(Fast)}.RequestParams"] = string.IsNullOrWhiteSpace(queryParam) ? bodyParam : queryParam;
        }

        await _next(httpContext);
    }
}