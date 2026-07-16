using DeploymentCenter.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DeploymentCenter.Server.Controllers;

[ApiController]
[Route("api/deployments")]
public sealed class DeploymentController(IDeploymentOrchestrator orchestrator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<DeploymentPlan>> Create(DeploymentRequest request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty || request.NodeIds.Count == 0 || request.PackageSha256.Length != 64 ||
            !request.PackageSha256.All(Uri.IsHexDigit))
            return BadRequest("A deployment id, at least one node, and a SHA-256 checksum are required.");
        return Ok(await orchestrator.StartAsync(request, cancellationToken));
    }

    [HttpPost("{deploymentId:guid}/rollback")]
    public async Task<IActionResult> Rollback(Guid deploymentId, CancellationToken cancellationToken)
    {
        await orchestrator.RollbackAsync(deploymentId, cancellationToken);
        return Accepted();
    }
}

public sealed class DeploymentHub : Hub { }
public sealed class AgentHub : Hub
{
    public Task Register(Guid nodeId) => Groups.AddToGroupAsync(Context.ConnectionId, nodeId.ToString());
}
