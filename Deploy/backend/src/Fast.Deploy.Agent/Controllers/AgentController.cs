using Fast.Deploy.Agent.Services;
using Fast.Deploy.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Deploy.Agent.Controllers;

[ApiController]
[Route("api/agent")]
public class AgentController : ControllerBase
{
    private readonly DeployExecutor _executor;
    private readonly HealthChecker _healthChecker;
    private readonly IConfiguration _config;

    public AgentController(DeployExecutor executor, HealthChecker healthChecker, IConfiguration config)
    {
        _executor = executor;
        _healthChecker = healthChecker;
        _config = config;
    }

    /// <summary>接受部署指令，异步执行后立即返回（进度通过回调上报）</summary>
    [HttpPost("deploy")]
    public IActionResult Deploy([FromBody] DeployCommandDto command)
    {
        // Token validation
        var expectedToken = _config["AgentToken"];
        if (!string.IsNullOrEmpty(expectedToken) && command.Token != expectedToken)
            return Unauthorized(new { message = "Token 无效" });

        // Fire-and-forget: deploy runs in background
        _ = Task.Run(() => _executor.ExecuteAsync(command));

        return Ok(new { message = "部署指令已接受，正在后台执行" });
    }

    /// <summary>检查指定 URL 的健康状态</summary>
    [HttpGet("health")]
    public async Task<IActionResult> Health([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return Ok(new AgentHealthResult { IsHealthy = true, Message = "Agent 运行正常" });

        var healthy = await _healthChecker.CheckAsync(url);
        return Ok(new AgentHealthResult { IsHealthy = healthy, Message = healthy ? "健康" : "不健康" });
    }
}
