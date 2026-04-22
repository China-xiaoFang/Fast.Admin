using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.DTOs;

// ========== Agent Communication DTOs ==========

public class DeployCommandDto
{
    public int DeploymentId { get; set; }
    public int NodeId { get; set; }
    public string ServerCallbackUrl { get; set; }
    public string PackageDownloadUrl { get; set; }
    public string TargetDirectory { get; set; }
    public string HealthCheckUrl { get; set; }
    public AppType AppType { get; set; }
    public OsType OsType { get; set; }
    public string Token { get; set; }
}

public class AgentLogDto
{
    public int DeploymentId { get; set; }
    public int? NodeId { get; set; }
    public string Level { get; set; } = "Info";
    public string Message { get; set; }
}

public class AgentHealthResult
{
    public bool IsHealthy { get; set; }
    public string Message { get; set; }
}
