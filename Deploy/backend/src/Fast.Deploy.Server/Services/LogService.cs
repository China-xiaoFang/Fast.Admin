using Fast.Deploy.Server.Data;
using Fast.Deploy.Server.Hubs;
using Fast.Deploy.Shared.DTOs;
using Fast.Deploy.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Fast.Deploy.Server.Services;

public class LogService
{
    private readonly DeployDbContext _db;
    private readonly IHubContext<DeployHub> _hub;

    public LogService(DeployDbContext db, IHubContext<DeployHub> hub)
    {
        _db = db;
        _hub = hub;
    }

    public async Task<DeployLogModel> WriteAsync(int deploymentId, int? nodeId, string level, string message)
    {
        var log = new DeployLogModel
        {
            DeploymentId = deploymentId,
            NodeId = nodeId,
            Level = level,
            Message = message,
            CreatedAt = DateTime.UtcNow
        };
        _db.DeployLogs.Add(log);
        await _db.SaveChangesAsync();

        // Push to SignalR group
        await _hub.Clients
            .Group($"deployment-{deploymentId}")
            .SendAsync("DeployLog", new DeployLogOutput
            {
                Id = log.Id,
                DeploymentId = log.DeploymentId,
                NodeId = log.NodeId,
                Level = log.Level,
                Message = log.Message,
                CreatedAt = log.CreatedAt
            });

        return log;
    }

    public async Task<List<DeployLogOutput>> GetLogsAsync(int deploymentId)
    {
        return await _db.DeployLogs
            .Where(l => l.DeploymentId == deploymentId)
            .OrderBy(l => l.CreatedAt)
            .Select(l => new DeployLogOutput
            {
                Id = l.Id,
                DeploymentId = l.DeploymentId,
                NodeId = l.NodeId,
                Level = l.Level,
                Message = l.Message,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();
    }
}
