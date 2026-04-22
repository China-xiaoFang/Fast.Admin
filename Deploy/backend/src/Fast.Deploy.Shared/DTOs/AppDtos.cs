using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.DTOs;

// ========== Application DTOs ==========

public class CreateAppInput
{
    public string Name { get; set; }
    public string Description { get; set; }
    public AppType AppType { get; set; }
}

public class UpdateAppInput
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class AppOutput
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AppType AppType { get; set; }
    public DateTime CreatedAt { get; set; }
}
