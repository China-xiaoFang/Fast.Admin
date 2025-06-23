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

using System.Reflection;
using System.Text;
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
/// <see cref="SyncApiHostedService"/> 同步Api托管服务
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

    public SyncApiHostedService(IOptions<SwaggerSettingsOptions> options, ILogger<DeleteLogHostedService> logger)
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

        try
        {
            var iDynamicApplicationType = typeof(IDynamicApplication);
            var httpMethodAttributeType = typeof(HttpMethodAttribute);

            // 查找所有带 IDynamicApplication 特性的类型
            var allApplicationTypeList = MAppContext.EffectiveTypes
                .Where(wh => iDynamicApplicationType.IsAssignableFrom(wh) && !wh.IsInterface)
                .Select(sl => new {ApiDescriptionSettings = sl.GetCustomAttribute<ApiDescriptionSettingsAttribute>(), Type = sl})
                .ToList();

            // 这里不能使用Aop
            var db = new SqlSugarClient(SqlSugarContext.DefaultConnectionConfig);

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

                        // 判断原有的Url是否存在
                        var apiInfo = apiInfoList.SingleOrDefault(s => s.ApiUrl == httpMethodAttribute.Template);

                        var method = System.Enum.Parse<HttpRequestMethodEnum>(httpMethodAttribute.HttpMethods.FirstOrDefault()
                                                                              ?? "Get");
                        var action = apiInfoAttribute?.Action ?? HttpRequestActionEnum.None;
                        var hasPermission = allowForbiddenAttribute == null && permissionAttribute?.TagList?.Count > 0;

                        if (apiInfo != null)
                        {
                            apiInfo.GroupName = groupName;
                            apiInfo.GroupTitle = groupOpenApiInfo?.Title;
                            apiInfo.Version = groupOpenApiInfo?.Version;
                            apiInfo.Description = groupOpenApiInfo?.Description;
                            apiInfo.ModuleName = moduleName;
                            apiInfo.ApiName = apiInfoAttribute?.Name;
                            apiInfo.Method = method;
                            apiInfo.Action = action;
                            apiInfo.HasAuth = allowAnonymousAttribute == null;
                            apiInfo.HasPermission = hasPermission;
                            apiInfo.Tags = string.Join(",", permissionAttribute?.TagList ?? new List<string>());
                            apiInfo.Sort = sort;
                            apiInfo.UpdatedTime = dateTime;
                        }
                        else
                        {
                            addApiInfoList.Add(new ApiInfoModel
                            {
                                Id = YitIdHelper.NextId(),
                                ServiceName = serviceName,
                                GroupName = groupName,
                                GroupTitle = groupOpenApiInfo?.Title,
                                Version = groupOpenApiInfo?.Version,
                                Description = groupOpenApiInfo?.Description,
                                ModuleName = moduleName,
                                ApiUrl = httpMethodAttribute.Template,
                                ApiName = apiInfoAttribute?.Name,
                                Method = method,
                                Action = action,
                                HasAuth = allowAnonymousAttribute == null,
                                HasPermission = hasPermission,
                                Tags = string.Join(",", permissionAttribute?.TagList ?? new List<string>()),
                                Sort = sort,
                                CreatedTime = dateTime
                            });
                        }
                    }
                }
            }

            await db.Updateable(apiInfoList)
                .ExecuteCommandAsync(cancellationToken);
            await db.Insertable(addApiInfoList)
                .ExecuteCommandAsync(cancellationToken);

            CacheContext.ApiInfoList = apiInfoList;
            CacheContext.ApiInfoList.AddRange(addApiInfoList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sync api error...");
        }

        var logSb = new StringBuilder();
        logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
        logSb.Append("system_notify");
        logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
        logSb.Append(": ");
        logSb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
        logSb.Append(Environment.NewLine);
        logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
        logSb.Append("              ");
        logSb.Append("同步接口信息成功。");
        logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
        Console.WriteLine(logSb.ToString());
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