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

using Fast.Common;
using Fast.NET.Core;
using Fast.OpenApi;
using Fast.Swagger;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Fast.Kernel;

/// <summary>
/// <see cref="GenerateApiFileHostedService"/> 生成Api文件托管服务
/// </summary>
[Order(2)]
public class GenerateApiFileHostedService : IHostedService
{
    /// <summary>
    /// 托管应用程序生命周期
    /// </summary>
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    /// <summary>
    /// 服务器
    /// </summary>
    private readonly IServer _server;

    /// <summary>
    /// 接口描述提供程序
    /// </summary>
    private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionGroupCollectionProvider;

    /// <summary>
    /// Swagger 配置
    /// </summary>
    private readonly SwaggerSettingsOptions _swaggerSettings;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public GenerateApiFileHostedService(IHostApplicationLifetime hostApplicationLifetime, IServer server,
        IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider, IOptions<SwaggerSettingsOptions> options,
        ILogger<DeleteLogHostedService> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _server = server;
        _apiDescriptionGroupCollectionProvider = apiDescriptionGroupCollectionProvider;
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
        // 只有开发环境才会生成
        if (!FastContext.HostEnvironment.IsDevelopment())
            return;

        // 只有启用了 Swagger 才会生成
        if (_swaggerSettings.Enable != true)
            return;

        // 订阅 ApplicationStarted 事件
        _hostApplicationLifetime?.ApplicationStarted.Register(() =>
        {
            try
            {
                var feature = _server.Features.Get<IServerAddressesFeature>();
                // 默认获取第一个地址，并且处理 [::]
                var address = feature?.Addresses.FirstOrDefault()
                    ?.Replace("[::]", "127.0.0.1");
                if (string.IsNullOrWhiteSpace(address))
                    return;

                // 获取 Swagger 分组
                var groupList = _swaggerSettings.GroupOpenApiInfos?.Select(sl => sl.Group)
                    .ToList();

                // 生成Api
                OpenApiUtil.GenerateOpenApi(address, _apiDescriptionGroupCollectionProvider, groupList)
                    .Wait(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Generate api file error...");
            }
        });

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