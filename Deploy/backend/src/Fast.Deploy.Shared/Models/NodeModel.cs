using Fast.Deploy.Shared.Enums;

namespace Fast.Deploy.Shared.Models;

/// <summary>节点模型（Agent 节点）</summary>
public class NodeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Ip { get; set; }
    public int Port { get; set; }
    public string Token { get; set; }
    public OsType OsType { get; set; }
    public NodeStatus Status { get; set; } = NodeStatus.Offline;
    public DateTime? LastHeartbeat { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
