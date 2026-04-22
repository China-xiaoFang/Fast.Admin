using Microsoft.AspNetCore.SignalR;

namespace Fast.Deploy.Server.Hubs;

/// <summary>
/// 部署 SignalR Hub，支持实时日志推送
/// </summary>
public class DeployHub : Hub
{
    /// <summary>加入部署日志组，接收指定部署的实时日志</summary>
    public async Task JoinDeployment(string deploymentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"deployment-{deploymentId}");
    }

    /// <summary>离开部署日志组</summary>
    public async Task LeaveDeployment(string deploymentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"deployment-{deploymentId}");
    }
}
