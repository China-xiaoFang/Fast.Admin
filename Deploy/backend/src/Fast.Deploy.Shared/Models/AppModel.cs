using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.Models;

/// <summary>应用模型</summary>
public class AppModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AppType AppType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<VersionModel> Versions { get; set; } = new List<VersionModel>();
    public ICollection<DeploymentModel> Deployments { get; set; } = new List<DeploymentModel>();
}
