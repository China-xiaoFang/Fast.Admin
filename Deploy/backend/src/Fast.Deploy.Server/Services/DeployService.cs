using Fast.Deploy.Server.Data;
using Fast.Deploy.Shared.DTOs;
using Fast.Deploy.Shared.Enums;
using Fast.Deploy.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Fast.Deploy.Server.Services;

public class DeployService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public DeployService(IServiceScopeFactory scopeFactory, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _scopeFactory = scopeFactory;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<DeploymentOutput> StartDeployAsync(StartDeployInput input, string operatorName)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DeployDbContext>();

        var app = await db.Apps.FindAsync(input.AppId);
        var version = await db.Versions.FindAsync(input.VersionId);
        if (app == null) throw new Exception($"应用 {input.AppId} 不存在");
        if (version == null) throw new Exception($"版本 {input.VersionId} 不存在");

        var deployment = new DeploymentModel
        {
            AppId = input.AppId,
            VersionId = input.VersionId,
            Strategy = input.Strategy,
            Status = DeployStatus.Running,
            Operator = operatorName ?? "system",
            HealthCheckUrl = input.HealthCheckUrl,
            NodeIds = JsonSerializer.Serialize(input.NodeIds),
            StartedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
        db.Deployments.Add(deployment);
        await db.SaveChangesAsync();

        // Run deployment in background
        _ = Task.Run(() => ExecuteDeployAsync(deployment.Id, input));

        return MapDeployment(deployment, app.Name, version.Version);
    }

    private async Task ExecuteDeployAsync(int deploymentId, StartDeployInput input)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DeployDbContext>();
        var logService = scope.ServiceProvider.GetRequiredService<LogService>();

        try
        {
            var nodes = await db.Nodes
                .Where(n => input.NodeIds.Contains(n.Id))
                .ToListAsync();

            var serverUrl = _config["ServerUrl"] ?? "http://localhost:5200";
            var version = await db.Versions.FindAsync(input.VersionId);
            var app = await db.Apps.FindAsync(input.AppId);
            var packageUrl = $"{serverUrl}/uploads/{input.AppId}/{version.Version}/{System.IO.Path.GetFileName(version.PackagePath)}";

            await logService.WriteAsync(deploymentId, null, "Info", $"开始执行 {input.Strategy} 部署，共 {nodes.Count} 个节点");

            switch (input.Strategy)
            {
                case DeployStrategy.Single:
                    await DeploySingleAsync(deploymentId, nodes.FirstOrDefault(), input, app, version, packageUrl, logService, db);
                    break;
                case DeployStrategy.Rolling:
                    await DeployRollingAsync(deploymentId, nodes, input, app, version, packageUrl, logService, db);
                    break;
                case DeployStrategy.BlueGreen:
                    await DeployBlueGreenAsync(deploymentId, nodes, input, app, version, packageUrl, logService, db);
                    break;
            }

            await UpdateDeploymentStatus(db, deploymentId, DeployStatus.Success);
            await logService.WriteAsync(deploymentId, null, "Info", "部署完成");
        }
        catch (Exception ex)
        {
            using var errScope = _scopeFactory.CreateScope();
            var errDb = errScope.ServiceProvider.GetRequiredService<DeployDbContext>();
            var errLog = errScope.ServiceProvider.GetRequiredService<LogService>();
            await errLog.WriteAsync(deploymentId, null, "Error", $"部署失败: {ex.Message}");
            await UpdateDeploymentStatus(errDb, deploymentId, DeployStatus.Failed);
        }
    }

    private async Task DeploySingleAsync(int deploymentId, NodeModel node, StartDeployInput input, AppModel app, VersionModel version, string packageUrl, LogService logService, DeployDbContext db)
    {
        if (node == null)
        {
            await logService.WriteAsync(deploymentId, null, "Error", "未找到目标节点");
            return;
        }

        await logService.WriteAsync(deploymentId, node.Id, "Info", $"[{node.Name}] 开始单机部署");
        await SendDeployCommandAsync(deploymentId, node, input, app, version, packageUrl, logService);
    }

    private async Task DeployRollingAsync(int deploymentId, List<NodeModel> nodes, StartDeployInput input, AppModel app, VersionModel version, string packageUrl, LogService logService, DeployDbContext db)
    {
        foreach (var node in nodes)
        {
            await logService.WriteAsync(deploymentId, node.Id, "Info", $"[{node.Name}] 滚动发布 - 开始部署");
            await SendDeployCommandAsync(deploymentId, node, input, app, version, packageUrl, logService);

            if (!string.IsNullOrEmpty(input.HealthCheckUrl))
            {
                await logService.WriteAsync(deploymentId, node.Id, "Info", $"[{node.Name}] 健康检查中...");
                var healthy = await WaitForHealthAsync(input.HealthCheckUrl, 30);
                if (!healthy)
                {
                    await logService.WriteAsync(deploymentId, node.Id, "Error", $"[{node.Name}] 健康检查失败，停止滚动发布");
                    throw new Exception($"节点 {node.Name} 健康检查失败");
                }
                await logService.WriteAsync(deploymentId, node.Id, "Info", $"[{node.Name}] 健康检查通过");
            }

            await Task.Delay(2000); // 间隔 2s 再部署下一个节点
        }
    }

    private async Task DeployBlueGreenAsync(int deploymentId, List<NodeModel> nodes, StartDeployInput input, AppModel app, VersionModel version, string packageUrl, LogService logService, DeployDbContext db)
    {
        var half = nodes.Count / 2;
        var greenNodes = nodes.Take(half > 0 ? half : 1).ToList();
        var blueNodes = nodes.Skip(greenNodes.Count).ToList();

        await logService.WriteAsync(deploymentId, null, "Info", $"蓝绿发布: Green 组 {greenNodes.Count} 节点, Blue 组 {blueNodes.Count} 节点");

        // 部署 Green 组
        foreach (var node in greenNodes)
        {
            await logService.WriteAsync(deploymentId, node.Id, "Info", $"[Green][{node.Name}] 开始部署");
            await SendDeployCommandAsync(deploymentId, node, input, app, version, packageUrl, logService);
        }

        // 健康检查 Green 组
        if (!string.IsNullOrEmpty(input.HealthCheckUrl))
        {
            await logService.WriteAsync(deploymentId, null, "Info", "Green 组健康检查中...");
            var healthy = await WaitForHealthAsync(input.HealthCheckUrl, 60);
            if (!healthy)
            {
                await logService.WriteAsync(deploymentId, null, "Error", "Green 组健康检查失败，蓝绿发布中止");
                throw new Exception("Green 组健康检查失败");
            }
            await logService.WriteAsync(deploymentId, null, "Info", "Green 组健康检查通过，切换流量...");
        }

        // 部署 Blue 组（旧流量节点）
        foreach (var node in blueNodes)
        {
            await logService.WriteAsync(deploymentId, node.Id, "Info", $"[Blue][{node.Name}] 开始部署");
            await SendDeployCommandAsync(deploymentId, node, input, app, version, packageUrl, logService);
        }

        await logService.WriteAsync(deploymentId, null, "Info", "蓝绿发布完成，所有节点已切换至新版本");
    }

    private async Task SendDeployCommandAsync(int deploymentId, NodeModel node, StartDeployInput input, AppModel app, VersionModel version, string packageUrl, LogService logService)
    {
        var serverUrl = _config["ServerUrl"] ?? "http://localhost:5200";
        var command = new DeployCommandDto
        {
            DeploymentId = deploymentId,
            NodeId = node.Id,
            ServerCallbackUrl = $"{serverUrl}/api/deployments/{deploymentId}/logs",
            PackageDownloadUrl = packageUrl,
            TargetDirectory = System.IO.Path.Combine(_config["DeployRoot"] ?? "deployments", $"{app.Name}_{version.Version}"),
            HealthCheckUrl = input.HealthCheckUrl,
            AppType = app.AppType,
            OsType = node.OsType,
            Token = node.Token
        };

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(10);
            var agentUrl = $"http://{node.Ip}:{node.Port}/api/agent/deploy";
            var response = await client.PostAsJsonAsync(agentUrl, command);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                await logService.WriteAsync(deploymentId, node.Id, "Error", $"[{node.Name}] Agent 响应异常: {err}");
            }
        }
        catch (Exception ex)
        {
            await logService.WriteAsync(deploymentId, node.Id, "Error", $"[{node.Name}] 连接 Agent 失败: {ex.Message}");
            throw;
        }
    }

    private async Task<bool> WaitForHealthAsync(string url, int timeoutSeconds)
    {
        var client = _httpClientFactory.CreateClient();
        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
        while (DateTime.UtcNow < deadline)
        {
            try
            {
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode) return true;
            }
            catch { /* retry */ }
            await Task.Delay(3000);
        }
        return false;
    }

    public async Task<DeploymentOutput> RollbackAsync(int deploymentId)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DeployDbContext>();
        var logService = scope.ServiceProvider.GetRequiredService<LogService>();

        var current = await db.Deployments
            .Include(d => d.App)
            .Include(d => d.AppVersion)
            .FirstOrDefaultAsync(d => d.Id == deploymentId);
        if (current == null) throw new Exception("部署记录不存在");

        // Find previous successful deployment for same app
        var previous = await db.Deployments
            .Include(d => d.AppVersion)
            .Where(d => d.AppId == current.AppId && d.Status == DeployStatus.Success && d.Id < deploymentId)
            .OrderByDescending(d => d.Id)
            .FirstOrDefaultAsync();

        if (previous == null) throw new Exception("没有可回滚的历史版本");

        await logService.WriteAsync(deploymentId, null, "Info", $"开始回滚到版本 {previous.AppVersion?.Version}");
        await UpdateDeploymentStatus(db, deploymentId, DeployStatus.RollingBack);

        var nodeIds = JsonSerializer.Deserialize<List<int>>(current.NodeIds ?? "[]");
        var rollbackInput = new StartDeployInput
        {
            AppId = current.AppId,
            VersionId = previous.VersionId,
            Strategy = current.Strategy,
            NodeIds = nodeIds,
            HealthCheckUrl = current.HealthCheckUrl
        };

        _ = Task.Run(async () =>
        {
            try
            {
                await ExecuteDeployAsync(deploymentId, rollbackInput);
                using var s = _scopeFactory.CreateScope();
                var d = s.ServiceProvider.GetRequiredService<DeployDbContext>();
                var l = s.ServiceProvider.GetRequiredService<LogService>();
                await UpdateDeploymentStatus(d, deploymentId, DeployStatus.RolledBack);
                await l.WriteAsync(deploymentId, null, "Info", "回滚完成");
            }
            catch (Exception ex)
            {
                using var s = _scopeFactory.CreateScope();
                var d = s.ServiceProvider.GetRequiredService<DeployDbContext>();
                var l = s.ServiceProvider.GetRequiredService<LogService>();
                await l.WriteAsync(deploymentId, null, "Error", $"回滚失败: {ex.Message}");
                await UpdateDeploymentStatus(d, deploymentId, DeployStatus.Failed);
            }
        });

        return MapDeployment(current, current.App?.Name, current.AppVersion?.Version);
    }

    private async Task UpdateDeploymentStatus(DeployDbContext db, int deploymentId, DeployStatus status)
    {
        var d = await db.Deployments.FindAsync(deploymentId);
        if (d != null)
        {
            d.Status = status;
            if (status is DeployStatus.Success or DeployStatus.Failed or DeployStatus.RolledBack)
                d.FinishedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }

    private static DeploymentOutput MapDeployment(DeploymentModel d, string appName, string versionName) => new()
    {
        Id = d.Id,
        AppId = d.AppId,
        AppName = appName,
        VersionId = d.VersionId,
        VersionName = versionName,
        Strategy = d.Strategy,
        Status = d.Status,
        Operator = d.Operator,
        StartedAt = d.StartedAt,
        FinishedAt = d.FinishedAt,
        CreatedAt = d.CreatedAt
    };
}
