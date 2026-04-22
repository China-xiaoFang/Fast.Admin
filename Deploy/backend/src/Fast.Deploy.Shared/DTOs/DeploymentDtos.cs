using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.DTOs;

// ========== Deployment DTOs ==========

public class StartDeployInput
{
    public int AppId { get; set; }
    public int VersionId { get; set; }
    public DeployStrategy Strategy { get; set; }
    public List<int> NodeIds { get; set; } = new();
    public string HealthCheckUrl { get; set; }
}

public class DeploymentOutput
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string AppName { get; set; }
    public int VersionId { get; set; }
    public string VersionName { get; set; }
    public DeployStrategy Strategy { get; set; }
    public DeployStatus Status { get; set; }
    public string Operator { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DeployLogOutput
{
    public int Id { get; set; }
    public int DeploymentId { get; set; }
    public int? NodeId { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
