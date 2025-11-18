using Fast.OpenApi;
using Fast.Swagger;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Fast.Core;

/// <summary>
/// <see cref="GenerateApiFileHostedService"/> 生成Api文件托管服务
/// </summary>
[Order(999)]
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

    /// <summary>
    /// <see cref="GenerateApiFileHostedService"/> 生成Api文件托管服务
    /// </summary>
    /// <param name="hostApplicationLifetime"><see cref="IHostApplicationLifetime"/> 托管应用程序生命周期</param>
    /// <param name="server"><see cref="IServer"/> 服务器</param>
    /// <param name="apiDescriptionGroupCollectionProvider"><see cref="IApiDescriptionGroupCollectionProvider"/> 接口描述提供程序</param>
    /// <param name="options"><see cref="IOptions{TOptions}"/> Swagger 配置</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public GenerateApiFileHostedService(IHostApplicationLifetime hostApplicationLifetime, IServer server,
        IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider,
        IOptions<SwaggerSettingsOptions> options,
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
            _ = Task.Run(async () =>
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
                    await OpenApiUtil.GenerateOpenApi(address, _apiDescriptionGroupCollectionProvider, groupList);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Generate api file error...");
                }
            }, cancellationToken);
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