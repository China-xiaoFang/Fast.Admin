using System.Reflection;
using System.Runtime.InteropServices;
using CSRedis;
using Fast.Cache;
using Fast.DependencyInjection;
using Fast.DynamicApplication;
using Fast.IaaS;
using Fast.JwtBearer;
using Fast.Logging;
using Fast.NET.Core;
using Fast.OpenApi;
using Fast.SqlSugar;
using Fast.Swagger;
using Fast.UnifyResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Newtonsoft.Json;
using SqlSugar;
using Swashbuckle.AspNetCore.Swagger;
using UAParser;
using Yitter.IdGenerator;

namespace Fast.Scheduler.Applications;

/// <summary>
/// <see cref="ProgramApplication"/> 程序信息
/// </summary>
[ApiDescriptionSettings(false)]
public class ProgramApplication : IDynamicApplication
{
    /// <summary>
    /// 程序信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/program"), HttpGet("/program/index")]
    [ApiInfo("程序信息", HttpRequestActionEnum.Other)]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var dateTime = DateTime.Now;

        var entryAssembly = Assembly.GetEntryAssembly();

        var swaggerAssembly = typeof(SwaggerOptions).Assembly.GetName();

        var csRedisCoreAssembly = typeof(CSRedisClient).Assembly.GetName();

        var sqlSugarAssembly = typeof(ISqlSugarClient).Assembly.GetName();
        var yitterIdGeneratorAssembly = typeof(YitIdHelper).Assembly.GetName();
        var uaParserAssembly = typeof(UserAgent).Assembly.GetName();

        var newtonsoftJsonAssembly = typeof(JsonConvert).Assembly.GetName();

        var fastRuntimeAssembly = typeof(MAppContext).Assembly.GetName();
        var fastCacheAssembly = typeof(ICache).Assembly.GetName();
        var fastDependencyAssembly = typeof(IDependency).Assembly.GetName();
        var fastDynamicApplicationAssembly = typeof(IDynamicApplication).Assembly.GetName();
        var fastIaaSAssembly = typeof(GlobalConstant).Assembly.GetName();
        var fastJwtBearerAssembly = typeof(IJwtBearerHandle).Assembly.GetName();
        var fastLoggingAssembly = typeof(Log).Assembly.GetName();
        var fastOpenApiAssembly = typeof(OpenApiUtil).Assembly.GetName();
        var fastNetCoreAssembly = typeof(FastContext).Assembly.GetName();
        var fastSqlSugarAssembly = typeof(ISqlSugarEntityHandler).Assembly.GetName();
        var fastSwaggerAssembly = typeof(SwaggerSettingsOptions).Assembly.GetName();
        var fastUnifyResultAssembly = typeof(IGlobalExceptionHandler).Assembly.GetName();

        var openXmlAssembly = typeof(DocumentFormat.OpenXml.OpenXmlElement).Assembly.GetName();
        var mailKitAssembly = typeof(MailKit.Net.Smtp.SmtpClient).Assembly.GetName();
        var miniExcelAssembly = typeof(MiniExcel).Assembly.GetName();
        var skitWechatApiAssembly = typeof(SKIT.FlurlHttpClient.Wechat.Api.WechatApiClient).Assembly.GetName();
        var knife4jUIAssembly = typeof(IGeekFan.AspNetCore.Knife4jUI.Knife4UIOptions).Assembly.GetName();
        var imageSharpAssembly = typeof(SixLabors.ImageSharp.Image).Assembly.GetName();

        var quartzAssembly = typeof(Quartz.SchedulerBuilder).Assembly.GetName();

        var cpuUsage = await MachineUtil.GetProgramCpuUsage();

        var (working, peakWorking, virtualMemory, peakVirtualMemory, pagedMemory, peakPagedMemory) =
            MachineUtil.GetProgramMemoryInfo();

        const decimal relation = 1024;

        return new JsonResult(new
        {
            // 当前时间
            CurrentTime = dateTime,
            // 启动时间
            StartTime = MachineUtil.GetProgramStartTime(),
            // 运行时间
            RunTimes = MachineUtil.GetProgramRunTimes(),
            // 程序名称
            ProgramName = entryAssembly?.GetName()
                .Name,
            // 程序版本
            ProgramVersion = entryAssembly?.GetName()
                .Version,
            // ReSharper disable once RedundantNameQualifier
            // 框架版本
            FrameworkVersion = $"{nameof(NET)} {typeof(MAppContext).Assembly.GetName().Version}",
            // 运行时版本
            RuntimeVersion = RuntimeInformation.FrameworkDescription,
            // CPU使用率(%)
            CpuUsage = cpuUsage,
            // CPU使用率(%)
            CpuUsagePercent = $"{cpuUsage} %",
            // 物理内存(MB)
            WorkingMemory = Math.Round(working, 2, MidpointRounding.AwayFromZero),
            // 物理内存(MB)
            WorkingMemoryMB = $"{working:F2} MB",
            // 最大物理内存(MB)
            PeakWorkingMemory = Math.Round(peakWorking, 2, MidpointRounding.AwayFromZero),
            // 最大物理内存(MB)
            PeakWorkingMemoryMB = $"{peakWorking:F2} MB",
            // 物理内存使用率(%)
            WorkingMemoryRate = Math.Round(working / peakWorking, 2, MidpointRounding.AwayFromZero),
            // 物理内存使用率(%)
            WorkingMemoryRatePercent = $"{working / peakWorking:F2} %",
            // 分页内存(MB)
            PagedMemoryMemory = Math.Round(pagedMemory, 2, MidpointRounding.AwayFromZero),
            // 分页内存(MB)
            PagedMemoryMemoryMB = $"{pagedMemory:F2} MB",
            // 最大分页内存(MB)
            PeakPagedMemoryMemory = Math.Round(peakPagedMemory, 2, MidpointRounding.AwayFromZero),
            // 最大分页内存(MB)
            PeakPagedMemoryMemoryMB = $"{peakPagedMemory:F2} MB",
            // 虚拟内存(GB)
            VirtualMemory = Math.Round(virtualMemory / relation, 2, MidpointRounding.AwayFromZero),
            // 虚拟内存(GB)
            VirtualMemoryGB = $"{virtualMemory / relation:F2} GB",
            // 最大虚拟内存(GB)
            PeakVirtualMemoryMemory = Math.Round(peakVirtualMemory / relation, 2, MidpointRounding.AwayFromZero),
            // 最大虚拟内存(GB)
            PeakVirtualMemoryMemoryGB = $"{peakVirtualMemory / relation:F2} GB",
            // 主要程序集
            Assemblys = new[]
            {
                new { swaggerAssembly.Name, swaggerAssembly.Version },
                new { csRedisCoreAssembly.Name, csRedisCoreAssembly.Version },
                new { sqlSugarAssembly.Name, sqlSugarAssembly.Version },
                new { yitterIdGeneratorAssembly.Name, yitterIdGeneratorAssembly.Version },
                new { uaParserAssembly.Name, uaParserAssembly.Version },
                new { newtonsoftJsonAssembly.Name, newtonsoftJsonAssembly.Version },
                new { fastRuntimeAssembly.Name, fastRuntimeAssembly.Version },
                new { fastCacheAssembly.Name, fastCacheAssembly.Version },
                new { fastDependencyAssembly.Name, fastDependencyAssembly.Version },
                new { fastDynamicApplicationAssembly.Name, fastDynamicApplicationAssembly.Version },
                new { fastIaaSAssembly.Name, fastIaaSAssembly.Version },
                new { fastJwtBearerAssembly.Name, fastJwtBearerAssembly.Version },
                new { fastLoggingAssembly.Name, fastLoggingAssembly.Version },
                new { fastOpenApiAssembly.Name, fastOpenApiAssembly.Version },
                new { fastNetCoreAssembly.Name, fastNetCoreAssembly.Version },
                new { fastSqlSugarAssembly.Name, fastSqlSugarAssembly.Version },
                new { fastSwaggerAssembly.Name, fastSwaggerAssembly.Version },
                new { fastUnifyResultAssembly.Name, fastUnifyResultAssembly.Version },
                new { openXmlAssembly.Name, openXmlAssembly.Version },
                new { mailKitAssembly.Name, mailKitAssembly.Version },
                new { miniExcelAssembly.Name, miniExcelAssembly.Version },
                new { skitWechatApiAssembly.Name, skitWechatApiAssembly.Version },
                new { knife4jUIAssembly.Name, knife4jUIAssembly.Version },
                new { imageSharpAssembly.Name, imageSharpAssembly.Version },
                new { quartzAssembly.Name, quartzAssembly.Version }
            }
        });
    }
}