﻿// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

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
using Fast.SqlSugar;
using Fast.Swagger;
using Fast.UnifyResult;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar;
using Swashbuckle.AspNetCore.Swagger;
using UAParser;
using Yitter.IdGenerator;
using IServiceCollectionExtension = Fast.Mapster.IServiceCollectionExtension;

// ReSharper disable once CheckNamespace
namespace Fast.Kernel;

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
    [HttpGet("/program"), HttpGet("/program/index"), AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var dateTime = DateTime.Now;

        var entryAssembly = Assembly.GetEntryAssembly();

        var swaggerAssembly = typeof(SwaggerOptions).Assembly.GetName();

        var csRedisCoreAssembly = typeof(CSRedisClient).Assembly.GetName();

        var sqlSugarAssembly = typeof(ISqlSugarClient).Assembly.GetName();
        var yitterIdGeneratorAssembly = typeof(YitIdHelper).Assembly.GetName();
        var uaParserAssembly = typeof(UserAgent).Assembly.GetName();

        var mapsterAssembly = typeof(TypeAdapterConfig).Assembly.GetName();

        var newtonsoftJsonAssembly = typeof(JsonConvert).Assembly.GetName();

        var fastRuntimeAssembly = typeof(MAppContext).Assembly.GetName();
        var fastCacheAssembly = typeof(ICache).Assembly.GetName();
        var fastDependencyAssembly = typeof(IDependency).Assembly.GetName();
        var fastDynamicApplicationAssembly = typeof(IDynamicApplication).Assembly.GetName();
        var fastIaaSAssembly = typeof(GlobalConstant).Assembly.GetName();
        var fastJwtBearerAssembly = typeof(IJwtBearerHandle).Assembly.GetName();
        var fastLoggingAssembly = typeof(Log).Assembly.GetName();
        var fastMapsterAssembly = typeof(IServiceCollectionExtension).Assembly.GetName();
        var fastNetCoreAssembly = typeof(FastContext).Assembly.GetName();
        var fastSqlSugarAssembly = typeof(ISqlSugarEntityHandler).Assembly.GetName();
        var fastSwaggerAssembly = typeof(SwaggerSettingsOptions).Assembly.GetName();
        var fastUnifyResultAssembly = typeof(IGlobalExceptionHandler).Assembly.GetName();

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
                new {swaggerAssembly.Name, swaggerAssembly.Version},
                new {csRedisCoreAssembly.Name, csRedisCoreAssembly.Version},
                new {sqlSugarAssembly.Name, sqlSugarAssembly.Version},
                new {yitterIdGeneratorAssembly.Name, yitterIdGeneratorAssembly.Version},
                new {uaParserAssembly.Name, uaParserAssembly.Version}, new {mapsterAssembly.Name, mapsterAssembly.Version},
                new {newtonsoftJsonAssembly.Name, newtonsoftJsonAssembly.Version},
                new {fastRuntimeAssembly.Name, fastRuntimeAssembly.Version},
                new {fastCacheAssembly.Name, fastCacheAssembly.Version},
                new {fastDependencyAssembly.Name, fastDependencyAssembly.Version},
                new {fastDynamicApplicationAssembly.Name, fastDynamicApplicationAssembly.Version},
                new {fastIaaSAssembly.Name, fastIaaSAssembly.Version},
                new {fastJwtBearerAssembly.Name, fastJwtBearerAssembly.Version},
                new {fastLoggingAssembly.Name, fastLoggingAssembly.Version},
                new {fastMapsterAssembly.Name, fastMapsterAssembly.Version},
                new {fastNetCoreAssembly.Name, fastNetCoreAssembly.Version},
                new {fastSqlSugarAssembly.Name, fastSqlSugarAssembly.Version},
                new {fastSwaggerAssembly.Name, fastSwaggerAssembly.Version},
                new {fastUnifyResultAssembly.Name, fastUnifyResultAssembly.Version}
            }
        });
    }
}