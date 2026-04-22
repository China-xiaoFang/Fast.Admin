using Fast.Deploy.Server.Data;
using Fast.Deploy.Server.Services;
using Fast.Deploy.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fast.Deploy.Server.Controllers;

[ApiController]
[Route("api/deployments")]
public class DeployController : ControllerBase
{
    private readonly DeployDbContext _db;
    private readonly DeployService _deployService;
    private readonly LogService _logService;

    public DeployController(DeployDbContext db, DeployService deployService, LogService logService)
    {
        _db = db;
        _deployService = deployService;
        _logService = logService;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var list = await _db.Deployments
            .Include(d => d.App)
            .Include(d => d.AppVersion)
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new DeploymentOutput
            {
                Id = d.Id, AppId = d.AppId, AppName = d.App.Name,
                VersionId = d.VersionId, VersionName = d.AppVersion.Version,
                Strategy = d.Strategy, Status = d.Status, Operator = d.Operator,
                StartedAt = d.StartedAt, FinishedAt = d.FinishedAt, CreatedAt = d.CreatedAt
            })
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await _db.Deployments.Include(x => x.App).Include(x => x.AppVersion).FirstOrDefaultAsync(x => x.Id == id);
        if (d == null) return NotFound();
        return Ok(new DeploymentOutput
        {
            Id = d.Id, AppId = d.AppId, AppName = d.App?.Name,
            VersionId = d.VersionId, VersionName = d.AppVersion?.Version,
            Strategy = d.Strategy, Status = d.Status, Operator = d.Operator,
            StartedAt = d.StartedAt, FinishedAt = d.FinishedAt, CreatedAt = d.CreatedAt
        });
    }

    [HttpPost]
    public async Task<IActionResult> Start([FromBody] StartDeployInput input)
    {
        var result = await _deployService.StartDeployAsync(input, User.Identity?.Name ?? "system");
        return Ok(result);
    }

    [HttpPost("{id:int}/rollback")]
    public async Task<IActionResult> Rollback(int id)
    {
        var result = await _deployService.RollbackAsync(id);
        return Ok(result);
    }

    [HttpGet("{deploymentId:int}/logs")]
    public async Task<IActionResult> GetLogs(int deploymentId)
    {
        var logs = await _logService.GetLogsAsync(deploymentId);
        return Ok(logs);
    }

    [HttpPost("{deploymentId:int}/logs")]
    public async Task<IActionResult> AddLog(int deploymentId, [FromBody] AgentLogDto dto)
    {
        await _logService.WriteAsync(deploymentId, dto.NodeId, dto.Level ?? "Info", dto.Message);
        return Ok();
    }
}
