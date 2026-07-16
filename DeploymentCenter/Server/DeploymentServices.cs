using System.Collections.Concurrent;
using DeploymentCenter.Server.Controllers;
using DeploymentCenter.Shared;
using Microsoft.AspNetCore.SignalR;

namespace DeploymentCenter.Server;

public sealed class InMemoryDeploymentRepository : IDeploymentRepository
{
    private readonly ConcurrentDictionary<Guid, DeploymentRequest> _requests = new();
    public Task AddAsync(DeploymentRequest request, CancellationToken cancellationToken) { _requests[request.Id] = request; return Task.CompletedTask; }
    public Task<DeploymentRequest?> FindAsync(Guid deploymentId, CancellationToken cancellationToken) =>
        Task.FromResult(_requests.TryGetValue(deploymentId, out var request) ? request : null);
}

public sealed class DeploymentOrchestrator(
    IEnumerable<IDeploymentStrategy> strategies,
    IDeploymentRepository repository,
    IAgentControlClient agents,
    IDeploymentEventPublisher events) : IDeploymentOrchestrator
{
    public async Task<DeploymentPlan> StartAsync(DeploymentRequest request, CancellationToken cancellationToken)
    {
        var strategy = strategies.SingleOrDefault(item => item.Type == request.Strategy)
            ?? throw new InvalidOperationException($"No strategy is registered for {request.Strategy}.");
        var plan = strategy.CreatePlan(request);
        await repository.AddAsync(request, cancellationToken);
        await events.PublishAsync(new(request.Id, DateTimeOffset.UtcNow, DeploymentStatus.Running, "Deployment started."), cancellationToken);
        try
        {
            foreach (var batch in plan.Batches)
            {
                foreach (var nodeId in batch)
                    await agents.DispatchAsync(new(request.Id, nodeId, $"/api/packages/{request.ReleaseId}", request.PackageSha256, "deploy", new Dictionary<string, string>()), cancellationToken);
                await events.PublishAsync(new(request.Id, DateTimeOffset.UtcNow, DeploymentStatus.Running, $"Batch of {batch.Count} node(s) dispatched."), cancellationToken);
            }
            return plan;
        }
        catch
        {
            await RollbackAsync(request.Id, cancellationToken);
            throw;
        }
    }

    public async Task RollbackAsync(Guid deploymentId, CancellationToken cancellationToken)
    {
        var request = await repository.FindAsync(deploymentId, cancellationToken) ?? throw new KeyNotFoundException("Deployment was not found.");
        foreach (var nodeId in request.NodeIds)
            await agents.RequestRollbackAsync(deploymentId, nodeId, cancellationToken);
        await events.PublishAsync(new(deploymentId, DateTimeOffset.UtcNow, DeploymentStatus.RolledBack, "Rollback requested."), cancellationToken);
    }
}

public sealed class LocalStorageProvider : IStorageProvider
{
    private readonly string _root;
    private readonly string _rootPrefix;

    public LocalStorageProvider(string root)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        _root = Path.GetFullPath(root);
        _rootPrefix = _root.EndsWith(Path.DirectorySeparatorChar) ? _root : _root + Path.DirectorySeparatorChar;
    }

    public async Task<string> SaveAsync(string key, Stream content, CancellationToken cancellationToken)
    {
        var path = GetPath(key);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var destination = File.Create(path);
        await content.CopyToAsync(destination, cancellationToken);
        return key;
    }
    public Task<Stream> OpenReadAsync(string key, CancellationToken cancellationToken) => Task.FromResult<Stream>(File.OpenRead(GetPath(key)));
    public Task DeleteAsync(string key, CancellationToken cancellationToken) { File.Delete(GetPath(key)); return Task.CompletedTask; }
    private string GetPath(string key)
    {
        var normalizedKey = Uri.UnescapeDataString(key).Replace('/', Path.DirectorySeparatorChar);
        if (Path.IsPathRooted(normalizedKey) || normalizedKey.Split(Path.DirectorySeparatorChar).Any(segment => segment is "." or ".."))
            throw new ArgumentException("Invalid storage key.", nameof(key));
        var path = Path.GetFullPath(Path.Combine(_root, normalizedKey));
        return path.StartsWith(_rootPrefix, StringComparison.Ordinal) ? path : throw new ArgumentException("Invalid storage key.", nameof(key));
    }
}

public sealed class SignalRAgentControlClient(IHubContext<AgentHub> hub) : IAgentControlClient
{
    public Task DispatchAsync(AgentTask task, CancellationToken cancellationToken) =>
        hub.Clients.Group(task.NodeId.ToString()).SendAsync("task", task, cancellationToken);
    public Task RequestRollbackAsync(Guid deploymentId, Guid nodeId, CancellationToken cancellationToken) =>
        hub.Clients.Group(nodeId.ToString()).SendAsync("rollback", deploymentId, cancellationToken);
}

public sealed class SignalRDeploymentEventPublisher(IHubContext<DeploymentHub> hub) : IDeploymentEventPublisher
{
    public Task PublishAsync(DeploymentEvent deploymentEvent, CancellationToken cancellationToken) =>
        hub.Clients.All.SendAsync("deploymentEvent", deploymentEvent, cancellationToken);
}
