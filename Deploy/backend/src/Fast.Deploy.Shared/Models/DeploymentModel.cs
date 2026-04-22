using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.Models;

/// <summary>部署任务模型</summary>
public class DeploymentModel
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public int VersionId { get; set; }
    public DeployStrategy Strategy { get; set; }
    public DeployStatus Status { get; set; } = DeployStatus.Pending;
    public string Operator { get; set; }
    public string HealthCheckUrl { get; set; }
    public string NodeIds { get; set; }  // JSON array of node IDs
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AppModel App { get; set; }
    public VersionModel AppVersion { get; set; }
    public ICollection<DeployLogModel> Logs { get; set; } = new List<DeployLogModel>();
}
