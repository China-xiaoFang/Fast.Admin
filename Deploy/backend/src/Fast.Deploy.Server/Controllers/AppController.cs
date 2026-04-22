using Fast.Deploy.Server.Data;
using Fast.Deploy.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fast.Deploy.Server.Controllers;

[ApiController]
[Route("api/apps")]
public class AppController : ControllerBase
{
    private readonly DeployDbContext _db;

    public AppController(DeployDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var apps = await _db.Apps
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AppOutput
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                AppType = a.AppType,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
        return Ok(apps);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            return BadRequest(new { message = "应用名称不能为空" });

        var app = new Fast.Deploy.Shared.Models.AppModel
        {
            Name = input.Name.Trim(),
            Description = input.Description?.Trim(),
            AppType = input.AppType,
            CreatedAt = DateTime.UtcNow
        };
        _db.Apps.Add(app);
        await _db.SaveChangesAsync();
        return Ok(new AppOutput { Id = app.Id, Name = app.Name, Description = app.Description, AppType = app.AppType, CreatedAt = app.CreatedAt });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppInput input)
    {
        var app = await _db.Apps.FindAsync(id);
        if (app == null) return NotFound(new { message = "应用不存在" });
        app.Name = input.Name?.Trim() ?? app.Name;
        app.Description = input.Description?.Trim();
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var app = await _db.Apps.FindAsync(id);
        if (app == null) return NotFound(new { message = "应用不存在" });
        _db.Apps.Remove(app);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
