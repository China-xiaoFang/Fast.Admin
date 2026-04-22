namespace Fast.Deploy.Shared.Models;

/// <summary>部署日志模型</summary>
public class DeployLogModel
{
    public int Id { get; set; }
    public int DeploymentId { get; set; }
    public int? NodeId { get; set; }
    public string Level { get; set; } = "Info";  // Info / Warn / Error
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DeploymentModel Deployment { get; set; }
}
