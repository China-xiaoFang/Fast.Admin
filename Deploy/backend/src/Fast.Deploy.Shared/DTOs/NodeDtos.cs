using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.DTOs;

// ========== Node DTOs ==========

public class RegisterNodeInput
{
    public string Name { get; set; }
    public string Ip { get; set; }
    public int Port { get; set; }
    public string Token { get; set; }
    public OsType OsType { get; set; }
}

public class NodeOutput
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Ip { get; set; }
    public int Port { get; set; }
    public OsType OsType { get; set; }
    public NodeStatus Status { get; set; }
    public DateTime? LastHeartbeat { get; set; }
    public DateTime CreatedAt { get; set; }
}
