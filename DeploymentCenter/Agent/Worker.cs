using System.Diagnostics;
using DeploymentCenter.Shared;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeploymentCenter.Agent;

public interface IAgentTaskExecutor
{
    Task ExecuteAsync(AgentTask task, CancellationToken cancellationToken);
}

public sealed class ScriptAgentTaskExecutor(ILogger<ScriptAgentTaskExecutor> logger) : IAgentTaskExecutor
{
    public Task ExecuteAsync(AgentTask task, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received {Operation} for deployment {DeploymentId} on node {NodeId}.", task.Operation, task.DeploymentId, task.NodeId);
        return Task.CompletedTask;
    }
}

public sealed class Worker(ILogger<Worker> logger, IConfiguration configuration, IAgentTaskExecutor executor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Deployment agent started on {Platform}.", Environment.OSVersion.Platform);
        var nodeId = Guid.Parse(configuration["Agent:NodeId"] ?? throw new InvalidOperationException("Agent:NodeId is required."));
        var connection = new HubConnectionBuilder()
            .WithUrl(new Uri(new Uri(configuration["Agent:ServerUrl"] ?? "http://localhost:5000"), "/hubs/agent"), options =>
                options.AccessTokenProvider = () => Task.FromResult(configuration["Agent:Token"]))
            .WithAutomaticReconnect()
            .Build();
        connection.On<AgentTask>("task", task => executor.ExecuteAsync(task, stoppingToken));
        connection.On<Guid>("rollback", deploymentId =>
            executor.ExecuteAsync(new AgentTask(deploymentId, nodeId, string.Empty, string.Empty, "rollback", new Dictionary<string, string>()), stoppingToken));
        await connection.StartAsync(stoppingToken);
        await connection.InvokeAsync("Register", nodeId, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        await connection.DisposeAsync();
    }
}
