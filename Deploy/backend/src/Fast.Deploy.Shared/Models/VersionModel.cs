using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.Models;

/// <summary>版本模型</summary>
public class VersionModel
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Version { get; set; }
    public string PackagePath { get; set; }
    public string Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AppModel App { get; set; }
}
