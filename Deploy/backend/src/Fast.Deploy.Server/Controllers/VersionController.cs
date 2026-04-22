using Fast.Deploy.Server.Data;
using Fast.Deploy.Shared.DTOs;
using Fast.Deploy.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fast.Deploy.Server.Controllers;

[ApiController]
[Route("api")]
public class VersionController : ControllerBase
{
    private readonly DeployDbContext _db;
    private readonly IWebHostEnvironment _env;

    private static readonly string[] AllowedExtensions = [".zip", ".tar", ".gz", ".tgz"];
    private const long MaxFileSize = 500 * 1024 * 1024; // 500 MB

    public VersionController(DeployDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [HttpGet("apps/{appId:int}/versions")]
    public async Task<IActionResult> List(int appId)
    {
        var versions = await _db.Versions
            .Where(v => v.AppId == appId)
            .OrderByDescending(v => v.CreatedAt)
            .Select(v => new VersionOutput
            {
                Id = v.Id,
                AppId = v.AppId,
                Version = v.Version,
                Notes = v.Notes,
                IsActive = v.IsActive,
                CreatedAt = v.CreatedAt
            })
            .ToListAsync();
        return Ok(versions);
    }

    [HttpPost("apps/{appId:int}/versions/upload")]
    [RequestSizeLimit(500 * 1024 * 1024)]
    public async Task<IActionResult> Upload(int appId, IFormFile file, [FromForm] string version, [FromForm] string notes)
    {
        var app = await _db.Apps.FindAsync(appId);
        if (app == null) return NotFound(new { message = "应用不存在" });

        if (file == null || file.Length == 0)
            return BadRequest(new { message = "请选择文件" });

        if (file.Length > MaxFileSize)
            return BadRequest(new { message = "文件不能超过 500MB" });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            return BadRequest(new { message = "仅支持 .zip / .tar / .gz / .tgz 格式" });

        if (string.IsNullOrWhiteSpace(version))
            return BadRequest(new { message = "版本号不能为空" });

        var uploadDir = Path.Combine(_env.ContentRootPath, "uploads", appId.ToString(), version);
        Directory.CreateDirectory(uploadDir);

        var fileName = $"{version}{ext}";
        var filePath = Path.Combine(uploadDir, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var model = new VersionModel
        {
            AppId = appId,
            Version = version.Trim(),
            PackagePath = filePath,
            Notes = notes?.Trim(),
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };
        _db.Versions.Add(model);
        await _db.SaveChangesAsync();

        return Ok(new VersionOutput
        {
            Id = model.Id,
            AppId = model.AppId,
            Version = model.Version,
            Notes = model.Notes,
            IsActive = model.IsActive,
            CreatedAt = model.CreatedAt
        });
    }

    [HttpPost("apps/{appId:int}/versions/{id:int}/activate")]
    public async Task<IActionResult> Activate(int appId, int id)
    {
        var version = await _db.Versions.FindAsync(id);
        if (version == null || version.AppId != appId) return NotFound(new { message = "版本不存在" });

        // Deactivate all other versions for the same app
        var others = await _db.Versions.Where(v => v.AppId == appId && v.Id != id).ToListAsync();
        others.ForEach(v => v.IsActive = false);
        version.IsActive = true;
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("versions/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var version = await _db.Versions.FindAsync(id);
        if (version == null) return NotFound(new { message = "版本不存在" });

        // Remove file
        if (!string.IsNullOrEmpty(version.PackagePath) && System.IO.File.Exists(version.PackagePath))
            System.IO.File.Delete(version.PackagePath);

        _db.Versions.Remove(version);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
