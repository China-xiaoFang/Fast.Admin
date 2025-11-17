using System.Net.Http.Headers;
using System.Web;
using Fast.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

// ReSharper disable once CheckNamespace
namespace Fast.Scheduler;

/// <summary>
/// <see cref="UrlJob"/> Url调度作业
/// </summary>
internal class UrlJob : JobBase<SchedulerJobUrlLogInfo>
{
    public UrlJob(IServiceProvider serviceProvider, IMailService mailService,
        IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
        ILogger<IJob> logger) : base(serviceProvider, mailService, jsonOptions, logger, new SchedulerJobUrlLogInfo())
    {
    }

    /// <summary>
    /// 执行调度作业
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> 服务提供者</param>
    /// <param name="db"><see cref="ISqlSugarClient"/> SqlSugar上下文</param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override async Task JobExecute(IServiceProvider serviceProvider, ISqlSugarClient db,
        IJobExecutionContext context)
    {
        // 作业类型
        var jobType = context.JobDetail.JobDataMap.GetEnum<SchedulerJobTypeEnum>(nameof(SchedulerJobInfo.JobType));

        if ((jobType & (SchedulerJobTypeEnum.IntranetUrl | SchedulerJobTypeEnum.OuterNetUrl)) == 0)
        {
            throw new UserFriendlyException("当前调度作业只支持【内网Url】【外网Url】！");
        }

        // 请求方式，默认Get请求
        var requestMethod =
            context.JobDetail.JobDataMap.GetNullableEnum<HttpRequestMethodEnum>(nameof(SchedulerJobInfo.RequestMethod))
            ?? HttpRequestMethodEnum.Get;
        _logInfo.RequestMethod = requestMethod.ToString();

        // 请求超时时间，默认不超时
        var requestTimeout = context.JobDetail.JobDataMap.GetNullableInt(nameof(SchedulerJobInfo.RequestTimeout));
        _logInfo.RequestTimeout = requestTimeout;

        // 请求参数
        var requestParams = new Dictionary<string, object>();
        var jobRequestParams =
            context.JobDetail.JobDataMap[nameof(SchedulerJobInfo.RequestParams)] as IDictionary<string, object>;
        jobRequestParams ??= new Dictionary<string, object>();
        // 请求参数处理
        foreach (var (key, value) in jobRequestParams)
        {
            requestParams.TryAdd(key, value);
        }

        _logInfo.RequestParams = $"<span class='params'>{HttpUtility.HtmlEncode(requestParams.ToJsonString())}</span>";

        // 请求头部
        var requestHeader = new Dictionary<string, string>();
        var jobRequestHeader =
            context.JobDetail.JobDataMap[nameof(SchedulerJobInfo.RequestHeader)] as IDictionary<string, string>;
        jobRequestHeader ??= new Dictionary<string, string>();
        // 请求头部处理
        foreach (var (key, value) in jobRequestHeader)
        {
            requestHeader.TryAdd(key, value);
        }

        _logInfo.RequestHeader = $"<span class='headers'>{HttpUtility.HtmlEncode(requestHeader.ToJsonString())}</span>";

        // 请求Url
        var requestUrl = context.JobDetail.JobDataMap.GetString(nameof(SchedulerJobInfo.RequestUrl));

        if (_logInfo.TenantId != null)
        {
            var user = serviceProvider.GetService<IUser>();

            requestHeader.TryAdd(HttpHeaderConst.DeviceType, user.DeviceType.ToString());
            requestHeader.TryAdd(HttpHeaderConst.DeviceId, user.DeviceId);

            // 重新赋值请求参数
            _logInfo.RequestHeader =
                $"<span class='headers'>{HttpUtility.HtmlEncode(requestHeader.ToJsonString())}</span>";

            // 安全期间，只有内网才添加授权信息
            if (jobType == SchedulerJobTypeEnum.IntranetUrl)
            {
                var accessToken = await user.RobotLogin();
                requestHeader.TryAdd("Authorization", accessToken);
            }
        }

        _logInfo.RequestUrl = $"<span class='url'>{requestUrl}</span>";

        // 响应数据
        string responseData;
        HttpResponseHeaders responseHeaders;

        // 发送请求
        switch (requestMethod)
        {
            case HttpRequestMethodEnum.Get:
                (responseData, responseHeaders) =
                    await RemoteRequestUtil.GetAsync(requestUrl, requestParams, requestHeader, timeout: requestTimeout);
                break;
            case HttpRequestMethodEnum.Post:
                (responseData, responseHeaders) =
                    await RemoteRequestUtil.PostAsync(requestUrl, requestParams, requestHeader,
                        timeout: requestTimeout);
                break;
            case HttpRequestMethodEnum.Put:
                (responseData, responseHeaders) =
                    await RemoteRequestUtil.PutAsync(requestUrl, requestParams, requestHeader, timeout: requestTimeout);
                break;
            case HttpRequestMethodEnum.Delete:
                (responseData, responseHeaders) =
                    await RemoteRequestUtil.DeleteAsync(requestUrl, requestHeader, timeout: requestTimeout);
                break;
            default:
                throw new UserFriendlyException("请求方式不支持！");
        }

        _logInfo.Result =
            $"<span class='result'>{HttpUtility.HtmlEncode(responseData).GetSubStringWithEllipsis(1000, true)}</span>";

        var responseHeader = new Dictionary<string, string>();
        foreach (var header in responseHeaders)
        {
            responseHeader.TryAdd(header.Key, string.Join(",", header.Value));
        }

        _logInfo.ResponseHeader = responseHeader;
    }
}