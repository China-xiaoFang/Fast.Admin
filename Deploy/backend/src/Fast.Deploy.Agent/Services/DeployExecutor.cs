using Fast.Deploy.Shared.DTOs;
using Fast.Deploy.Shared.Enums;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Fast.Deploy.Agent.Services;

/// <summary>
/// 部署执行器 — 下载包、解压、执行启动脚本，并将进度上报给 Server
/// </summary>
public class DeployExecutor
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DeployExecutor> _logger;

    public DeployExecutor(IConfiguration config, IHttpClientFactory httpClientFactory, ILogger<DeployExecutor> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task ExecuteAsync(DeployCommandDto command)
    {
        try
        {
            await ReportAsync(command, "Info", "Agent 开始处理部署指令");

            // 1. Resolve target directory
            var deployRoot = _config["DeployRoot"] ?? "deployments";
            var targetDir = Path.IsPathRooted(command.TargetDirectory)
                ? command.TargetDirectory
                : Path.Combine(deployRoot, command.TargetDirectory);
            Directory.CreateDirectory(targetDir);

            // 2. Download package
            await ReportAsync(command, "Info", $"正在下载包: {command.PackageDownloadUrl}");
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            var packageBytes = await client.GetByteArrayAsync(command.PackageDownloadUrl);

            var ext = Path.GetExtension(new Uri(command.PackageDownloadUrl).LocalPath).ToLowerInvariant();
            var tempFile = Path.Combine(Path.GetTempPath(), $"fastdeploy_{command.DeploymentId}_{Guid.NewGuid()}{ext}");
            await File.WriteAllBytesAsync(tempFile, packageBytes);
            await ReportAsync(command, "Info", $"下载完成，大小: {packageBytes.Length / 1024} KB");

            // 3. Stop running app
            await StopAppAsync(command, targetDir);

            // 4. Extract package
            await ReportAsync(command, "Info", "正在解压包...");
            if (Directory.Exists(targetDir))
            {
                foreach (var f in Directory.GetFiles(targetDir))
                    File.Delete(f);
                foreach (var d in Directory.GetDirectories(targetDir))
                    Directory.Delete(d, true);
            }

            if (ext == ".zip")
                ZipFile.ExtractToDirectory(tempFile, targetDir, overwriteFiles: true);
            else
                await ExtractTarAsync(tempFile, targetDir);

            File.Delete(tempFile);
            await ReportAsync(command, "Info", "解压完成");

            // 5. Run start script
            await RunStartScriptAsync(command, targetDir);

            // 6. Health check
            if (!string.IsNullOrEmpty(command.HealthCheckUrl))
            {
                await ReportAsync(command, "Info", "等待健康检查...");
                var healthy = await WaitForHealthAsync(command.HealthCheckUrl, 60);
                if (healthy)
                    await ReportAsync(command, "Info", "健康检查通过");
                else
                    await ReportAsync(command, "Warn", "健康检查超时，请手动验证");
            }

            await ReportAsync(command, "Info", "部署成功完成");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "部署执行失败");
            await ReportAsync(command, "Error", $"部署失败: {ex.Message}");
            throw;
        }
    }

    private async Task StopAppAsync(DeployCommandDto command, string targetDir)
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var pidFile = Path.Combine(targetDir, "app.pid");
        if (isWindows)
        {
            var stopScript = Path.Combine(targetDir, "stop.bat");
            if (File.Exists(stopScript))
            {
                await ReportAsync(command, "Info", "执行停止脚本...");
                await RunProcessAsync("cmd.exe", $"/c \"{stopScript}\"", targetDir);
            }
        }
        else
        {
            var stopScript = Path.Combine(targetDir, "stop.sh");
            if (File.Exists(stopScript))
            {
                await ReportAsync(command, "Info", "执行停止脚本...");
                await RunProcessAsync("bash", stopScript, targetDir);
            }
            else if (File.Exists(pidFile))
            {
                var pid = await File.ReadAllTextAsync(pidFile);
                await RunProcessAsync("kill", pid.Trim(), targetDir);
            }
        }
    }

    private async Task RunStartScriptAsync(DeployCommandDto command, string targetDir)
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        if (isWindows)
        {
            var script = Path.Combine(targetDir, "start.bat");
            if (File.Exists(script))
            {
                await ReportAsync(command, "Info", "执行启动脚本 (Windows)...");
                await RunProcessAsync("cmd.exe", $"/c \"{script}\"", targetDir);
            }
            else
            {
                await ReportAsync(command, "Warn", "未找到 start.bat，尝试直接启动 .dll...");
                await StartDotNetDllAsync(command, targetDir);
            }
        }
        else
        {
            var script = Path.Combine(targetDir, "start.sh");
            if (File.Exists(script))
            {
                // Ensure executable
                await RunProcessAsync("chmod", $"+x \"{script}\"", targetDir);
                await ReportAsync(command, "Info", "执行启动脚本 (Linux)...");
                await RunProcessAsync("bash", script, targetDir);
            }
            else
            {
                await ReportAsync(command, "Warn", "未找到 start.sh，尝试直接启动 .dll...");
                await StartDotNetDllAsync(command, targetDir);
            }
        }
    }

    private async Task StartDotNetDllAsync(DeployCommandDto command, string targetDir)
    {
        if (command.AppType != AppType.DotNet) return;
        var dll = Directory.GetFiles(targetDir, "*.dll").FirstOrDefault();
        if (dll == null)
        {
            await ReportAsync(command, "Error", "未找到可执行 .dll 文件");
            return;
        }
        await ReportAsync(command, "Info", $"启动: dotnet {Path.GetFileName(dll)}");
        // Fire-and-forget detached process
        var psi = new System.Diagnostics.ProcessStartInfo("dotnet", $"\"{dll}\"")
        {
            WorkingDirectory = targetDir,
            UseShellExecute = true,
            CreateNoWindow = false
        };
        System.Diagnostics.Process.Start(psi);
    }

    private static Task RunProcessAsync(string fileName, string args, string workDir)
    {
        var psi = new System.Diagnostics.ProcessStartInfo(fileName, args)
        {
            WorkingDirectory = workDir,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            UseShellExecute = true
        };
        var proc = System.Diagnostics.Process.Start(psi);
        return proc?.WaitForExitAsync() ?? Task.CompletedTask;
    }

    private static async Task ExtractTarAsync(string file, string targetDir)
    {
        // Use system tar
        var psi = new System.Diagnostics.ProcessStartInfo("tar", $"-xf \"{file}\" -C \"{targetDir}\"")
        {
            UseShellExecute = true
        };
        var proc = System.Diagnostics.Process.Start(psi);
        if (proc != null) await proc.WaitForExitAsync();
    }

    private async Task<bool> WaitForHealthAsync(string url, int timeoutSeconds)
    {
        var client = _httpClientFactory.CreateClient();
        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
        while (DateTime.UtcNow < deadline)
        {
            try
            {
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode) return true;
            }
            catch { /* retry */ }
            await Task.Delay(3000);
        }
        return false;
    }

    private async Task ReportAsync(DeployCommandDto command, string level, string message)
    {
        _logger.LogInformation("[{Level}] {Message}", level, message);
        if (string.IsNullOrEmpty(command.ServerCallbackUrl)) return;
        try
        {
            var client = _httpClientFactory.CreateClient();
            await client.PostAsJsonAsync(command.ServerCallbackUrl, new AgentLogDto
            {
                DeploymentId = command.DeploymentId,
                NodeId = command.NodeId,
                Level = level,
                Message = message
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning("上报日志失败: {Msg}", ex.Message);
        }
    }
}
