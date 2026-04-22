using Fast.Deploy.Server.Data;
using Fast.Deploy.Shared.DTOs;
using Fast.Deploy.Shared.Enums;
using Fast.Deploy.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fast.Deploy.Server.Controllers;

[ApiController]
[Route("api/nodes")]
public class NodeController : ControllerBase
{
    private readonly DeployDbContext _db;
    public NodeController(DeployDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var nodes = await _db.Nodes.OrderByDescending(n => n.CreatedAt)
            .Select(n => new NodeOutput { Id = n.Id, Name = n.Name, Ip = n.Ip, Port = n.Port, OsType = n.OsType, Status = n.Status, LastHeartbeat = n.LastHeartbeat, CreatedAt = n.CreatedAt })
            .ToListAsync();
        return Ok(nodes);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterNodeInput input)
    {
        var existing = await _db.Nodes.FirstOrDefaultAsync(n => n.Ip == input.Ip && n.Port == input.Port);
        if (existing != null)
        {
            existing.Name = input.Name;
            existing.Token = input.Token;
            existing.OsType = input.OsType;
            existing.Status = NodeStatus.Online;
            existing.LastHeartbeat = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new NodeOutput { Id = existing.Id, Name = existing.Name, Ip = existing.Ip, Port = existing.Port, OsType = existing.OsType, Status = existing.Status, LastHeartbeat = existing.LastHeartbeat, CreatedAt = existing.CreatedAt });
        }
        var node = new NodeModel { Name = input.Name, Ip = input.Ip, Port = input.Port, Token = input.Token, OsType = input.OsType, Status = NodeStatus.Online, LastHeartbeat = DateTime.UtcNow, CreatedAt = DateTime.UtcNow };
        _db.Nodes.Add(node);
        await _db.SaveChangesAsync();
        return Ok(new NodeOutput { Id = node.Id, Name = node.Name, Ip = node.Ip, Port = node.Port, OsType = node.OsType, Status = node.Status, LastHeartbeat = node.LastHeartbeat, CreatedAt = node.CreatedAt });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var node = await _db.Nodes.FindAsync(id);
        if (node == null) return NotFound();
        _db.Nodes.Remove(node);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{id:int}/heartbeat")]
    public async Task<IActionResult> Heartbeat(int id)
    {
        var node = await _db.Nodes.FindAsync(id);
        if (node == null) return NotFound();
        node.Status = NodeStatus.Online;
        node.LastHeartbeat = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok();
    }
}
