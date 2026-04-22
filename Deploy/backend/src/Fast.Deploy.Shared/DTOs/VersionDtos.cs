namespace Fast.Deploy.Shared.DTOs;

// ========== Version DTOs ==========

public class VersionOutput
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Version { get; set; }
    public string Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
