﻿// ------------------------------------------------------------------------
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

using System.Reflection;
using Dm.util;
using Fast.DynamicApplication;
using Fast.FastCloud.Entity;
using Fast.JwtBearer;
using Fast.SqlSugar;
using Fast.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="SyncApiHostedService"/> 同步 Api 托管服务
/// </summary>
[SuppressSniffer]
public class SyncApiHostedService : IHostedService
{
    /// <summary>
    /// Swagger 配置
    /// </summary>
    private readonly SwaggerSettingsOptions _swaggerSettings;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public SyncApiHostedService(IOptions<SwaggerSettingsOptions> options, ILogger<SyncApiHostedService> logger)
    {
        _swaggerSettings = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous Start operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var dateTime = DateTime.Now;

        var serviceName = Assembly.GetEntryAssembly()!.GetName()
            .Name;
        var addApiInfoList = new List<ApiInfoModel>();
        var updateApiInfoList = new List<ApiInfoModel>();
        var apiUrlList = new List<string>();

        _logger.LogInformation("开始同步接口信息...");

        try
        {
            var iDynamicApplicationType = typeof(IDynamicApplication);
            var httpMethodAttributeType = typeof(HttpMethodAttribute);

            // 查找所有带 IDynamicApplication 特性的类型
            var allApplicationTypeList = MAppContext.EffectiveTypes
                .Where(wh => iDynamicApplicationType.IsAssignableFrom(wh) && !wh.IsInterface)
                .Select(sl => new {ApiDescriptionSettings = sl.GetCustomAttribute<ApiDescriptionSettingsAttribute>(), Type = sl})
                .Where(wh => !wh.ApiDescriptionSettings.IgnoreApi)
                .ToList();

            var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));

            var apiInfoList = await db.Queryable<ApiInfoModel>()
                .Where(wh => wh.ServiceName == serviceName)
                .ToListAsync(cancellationToken);

            // 循环所有类型
            foreach (var applicationInfo in allApplicationTypeList)
            {
                // 获取分组名称
                var groupName = applicationInfo.ApiDescriptionSettings.Groups.FirstOrDefault() ?? "Default";
                var moduleName = applicationInfo.ApiDescriptionSettings.Name ?? applicationInfo.Type.Name;
                var sort = applicationInfo.ApiDescriptionSettings.Order;
                var groupOpenApiInfo = _swaggerSettings.GroupOpenApiInfos.FirstOrDefault(f => f.Group == groupName);

                // 获取所有方法
                foreach (var methodInfo in applicationInfo.Type.GetMethods()
                             .ToList())
                {
                    var attributes = methodInfo.GetCustomAttributes()
                        .Where(httpMethodAttributeType.IsInstanceOfType)
                        .ToList();
                    foreach (var attribute in attributes)
                    {
                        if (attribute is not HttpMethodAttribute httpMethodAttribute)
                            continue;

                        var allowAnonymousAttribute = methodInfo.GetCustomAttribute<AllowAnonymousAttribute>();
                        var allowForbiddenAttribute = methodInfo.GetCustomAttribute<AllowForbiddenAttribute>();
                        var permissionAttribute = methodInfo.GetCustomAttribute<PermissionAttribute>();
                        var apiInfoAttribute = methodInfo.GetCustomAttribute<ApiInfoAttribute>();

                        var apiUrl = httpMethodAttribute.Template.StartsWith("/")
                            ? httpMethodAttribute.Template
                            : $"/{moduleName}/{httpMethodAttribute.Template}";

                        // 判断原有的Url是否存在
                        var apiInfo = apiInfoList.SingleOrDefault(s => s.ApiUrl == apiUrl);

                        var method = System.Enum.Parse<HttpRequestMethodEnum>(httpMethodAttribute.HttpMethods.FirstOrDefault()
                                                                              ?? "Get",
                            ignoreCase: true);
                        var action = apiInfoAttribute?.Action ?? HttpRequestActionEnum.None;
                        var hasPermission = allowForbiddenAttribute == null && permissionAttribute?.TagList?.Count > 0;

                        var apiInfoModel = new ApiInfoModel
                        {
                            ServiceName = serviceName,
                            GroupName = groupName,
                            GroupTitle = groupOpenApiInfo?.Title,
                            Version = groupOpenApiInfo?.Version,
                            Description = groupOpenApiInfo?.Description,
                            ModuleName = moduleName,
                            ApiUrl = apiUrl,
                            ApiName = apiInfoAttribute?.Name,
                            Method = method,
                            Action = action,
                            HasAuth = allowAnonymousAttribute == null,
                            HasPermission = hasPermission,
                            Tags = string.Join(",", permissionAttribute?.TagList ?? []),
                            Sort = sort
                        };
                        apiUrlList.add(apiInfoModel.ApiUrl);

                        if (apiInfo != null)
                        {
                            apiInfoModel.Id = apiInfo.Id;
                            // 不相同才修改
                            if (!apiInfo.Equals(apiInfoModel))
                            {
                                apiInfoModel.Adapt(apiInfo);
                                apiInfo.UpdatedTime = dateTime;
                                updateApiInfoList.Add(apiInfo);
                            }
                        }
                        else
                        {
                            apiInfoModel.Id = YitIdHelper.NextId();
                            apiInfoModel.CreatedTime = dateTime;
                            addApiInfoList.Add(apiInfoModel);
                        }
                    }
                }
            }

            // 加载Aop
            SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(),
                db,
                SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds,
                false,
                true,
                null);

            var deleteApiInfoList = apiInfoList.Where(wh => !apiUrlList.Contains(wh.ApiUrl))
                .ToList();

            if (deleteApiInfoList.Count > 0)
            {
                await db.Deleteable(deleteApiInfoList)
                    .ExecuteCommandAsync(cancellationToken);
            }

            await db.Updateable(updateApiInfoList)
                .ExecuteCommandAsync(cancellationToken);
            await db.Insertable(addApiInfoList)
                .ExecuteCommandAsync(cancellationToken);

            CacheContext.ApiInfoList = apiInfoList;
            CacheContext.ApiInfoList.AddRange(addApiInfoList);

            _logger.LogInformation(
                $"同步接口信息成功。新增 {addApiInfoList.Count} 个，更新 {updateApiInfoList.Count} 个，删除 {deleteApiInfoList.Count} 个。");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sync api error...");
        }
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