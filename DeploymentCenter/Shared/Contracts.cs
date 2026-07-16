namespace DeploymentCenter.Shared;

public enum ApplicationType { DotNet, Vue }
public enum DeploymentStrategyType { Single, Rolling, BlueGreen }
public enum DeploymentStatus { Pending, Running, Succeeded, Failed, RolledBack }
public enum AgentPlatform { Windows, Linux, MacOS }
public enum HealthCheckType { Http, Https, Tcp }

public sealed record ApplicationDefinition(Guid Id, string TenantId, string Name, ApplicationType Type, int RetainedVersions = 20);
public sealed record EnvironmentDefinition(Guid Id, string TenantId, string Name, bool IsProduction);
public sealed record NodeDefinition(Guid Id, string TenantId, string Name, AgentPlatform Platform, IReadOnlySet<string> Tags);
public sealed record ReleaseDefinition(Guid Id, Guid ApplicationId, string Version, string PackageKey, string Sha256, bool IsLts, bool IsPrerelease);
public sealed record HealthCheckDefinition(HealthCheckType Type, Uri? Url, string? Host, int? Port, TimeSpan Timeout);
public sealed record DeploymentRequest(
    Guid Id,
    Guid ApplicationId,
    Guid EnvironmentId,
    Guid ReleaseId,
    DeploymentStrategyType Strategy,
    IReadOnlyList<Guid> NodeIds,
    IReadOnlyList<int>? RollingPercentages,
    HealthCheckDefinition HealthCheck);
public sealed record DeploymentPlan(IReadOnlyList<IReadOnlyList<Guid>> Batches, bool RequiresTrafficSwitch);
public sealed record DeploymentEvent(Guid DeploymentId, DateTimeOffset Timestamp, DeploymentStatus Status, string Message, Guid? NodeId = null);
public sealed record AgentRegistration(string AgentName, AgentPlatform Platform, string AccessKey, string SecretProof, IReadOnlySet<string> Tags);
public sealed record AgentHeartbeat(Guid NodeId, double CpuPercent, long MemoryBytes, long DiskFreeBytes, DateTimeOffset Timestamp);
public sealed record AgentTask(Guid DeploymentId, Guid NodeId, string PackageUrl, string Sha256, string Operation, IReadOnlyDictionary<string, string> Parameters);

public interface IDeploymentStrategy
{
    DeploymentStrategyType Type { get; }
    DeploymentPlan CreatePlan(DeploymentRequest request);
}

public interface IDeploymentOrchestrator
{
    Task<DeploymentPlan> StartAsync(DeploymentRequest request, CancellationToken cancellationToken);
    Task RollbackAsync(Guid deploymentId, CancellationToken cancellationToken);
}

public interface IStorageProvider
{
    Task<string> SaveAsync(string key, Stream content, CancellationToken cancellationToken);
    Task<Stream> OpenReadAsync(string key, CancellationToken cancellationToken);
    Task DeleteAsync(string key, CancellationToken cancellationToken);
}

public interface IAgentControlClient
{
    Task DispatchAsync(AgentTask task, CancellationToken cancellationToken);
    Task RequestRollbackAsync(Guid deploymentId, Guid nodeId, CancellationToken cancellationToken);
}

public interface IDeploymentEventPublisher
{
    Task PublishAsync(DeploymentEvent deploymentEvent, CancellationToken cancellationToken);
}

public interface IDeploymentRepository
{
    Task AddAsync(DeploymentRequest request, CancellationToken cancellationToken);
    Task<DeploymentRequest?> FindAsync(Guid deploymentId, CancellationToken cancellationToken);
}
