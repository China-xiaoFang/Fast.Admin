using Fast.DynamicApplication;
using Fast.NET.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Api.Applications;

/// <summary>
/// <see cref="HealthApplication"/> 健康检查
/// </summary>
[ApiDescriptionSettings(false)]
public class HealthApplication : IDynamicApplication
{
    /// <summary>
    /// 健康检查
    /// </summary>
    /// <returns></returns>
    [HttpGet("/health"), HttpGet("/health/index")]
    [ApiInfo("健康检查", HttpRequestActionEnum.Other)]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return new JsonResult(new
        {
            // 当前时间
            CurrentTime = DateTime.Now,
            // 运行时间
            RunTimes = MachineUtil.GetProgramRunTimes()
        });
    }
}