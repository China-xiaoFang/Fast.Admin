using System.Runtime.InteropServices;
using Fast.DynamicApplication;
using Fast.NET.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Api.Applications;

/// <summary>
/// <see cref="MachineApplication"/> 服务器信息
/// </summary>
[ApiDescriptionSettings(false)]
public class MachineApplication : IDynamicApplication
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/machine"), HttpGet("/machine/index")]
    [ApiInfo("服务器信息", HttpRequestActionEnum.Other)]
    [AllowAnonymous]
    public IActionResult Index()
    {
        var dateTime = DateTime.Now;

        var cpuRate = MachineUtil.GetSystemCpuRate();

        var (ramTotal, ramUsed, ramFree) = MachineUtil.GetSystemRamInfo();

        const decimal relation = 1024;

        return new JsonResult(new
        {
            // 当前时间
            CurrentTime = dateTime,
            // 运行时间
            RunTimes = MachineUtil.GetSystemRunTimes(),
            // 主机名称
            HostName = Environment.MachineName,
            // 操作系统
            OSName = MachineUtil.GetOSDescription(),
            // 系统架构
            OSArchitecture =
                $"{Environment.OSVersion.Platform.ToString()} {RuntimeInformation.OSArchitecture.ToString()}",
            // CPU核数
            CpuCount = $"{Environment.ProcessorCount} 核",
            // CPU使用率(%)
            CpuRate = cpuRate,
            // CPU使用率(%)
            CpuRatePercent = cpuRate.Select(sl => $"{sl} %")
                .ToList(),
            // 总内存(GB)
            RamTotal = Math.Round(ramTotal / relation, 2, MidpointRounding.AwayFromZero),
            // 总内存(GB)
            RamTotalGB = $"{ramTotal / relation:F2} GB",
            // 已用内存(GB)
            RamUsed = Math.Round(ramUsed / relation, 2, MidpointRounding.AwayFromZero),
            // 已用内存(GB)
            RamUsedGB = $"{ramUsed / relation:F2} GB",
            // 可用内存(GB)
            RamFree = Math.Round(ramFree / relation, 2, MidpointRounding.AwayFromZero),
            // 可用内存(GB)
            RamFreeGB = $"{ramFree / relation:F2} GB",
            // 内存使用率(%)
            RamRate = Math.Round(ramUsed / ramTotal * 100, 2, MidpointRounding.AwayFromZero),
            // 内存使用率(%)
            RamRatePercent = $"{ramUsed / ramTotal * 100:F2} %",
            // 硬盘信息
            DiskInfos = MachineUtil.GetDiskInfos()
        });
    }
}