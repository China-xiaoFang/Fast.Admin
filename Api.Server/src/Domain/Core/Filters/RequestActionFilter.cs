// ------------------------------------------------------------------------
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

using System.Diagnostics;
using Fast.CenterLog.Entity;
using Fast.SqlSugar;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Yitter.IdGenerator;

namespace Fast.Core;

/// <summary>
/// 请求日志过滤器
/// </summary>
public class RequestActionFilter : IAsyncActionFilter
{
    /// <summary>
    /// SqlSugar 实体服务
    /// </summary>
    private readonly ISqlSugarEntityService _sqlSugarEntityService;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 请求日志拦截
    /// </summary>
    public RequestActionFilter(ISqlSugarEntityService sqlSugarEntityService, ILogger<IAsyncActionFilter> logger)
    {
        _sqlSugarEntityService = sqlSugarEntityService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var dateTime = DateTime.Now;
        var httpContext = context.HttpContext;
        var httpRequest = httpContext.Request;
        var _user = httpContext.RequestServices.GetService<IUser>();

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var actionContext = await next();
        stopwatch.Stop();

        try
        {
            var actionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;
            var endpointMetadata = actionContext.ActionDescriptor.EndpointMetadata;

            // 判断是否存在禁用请求日志特性，支持 Controller 和 Action
            if (endpointMetadata.OfType<DisabledRequestLogAttribute>()
                .Any())
                return;

            // 获取 ApiInfo 特性，Controller 和 Action 同时存在时优先使用 Action
            var apiInfoAttribute = endpointMetadata.OfType<ApiInfoAttribute>()
                .LastOrDefault();
            if (!Enum.TryParse(httpRequest.Method, true, out HttpRequestMethodEnum requestMethod))
            {
                // 默认 Get 请求
                requestMethod = HttpRequestMethodEnum.Get;
            }

            // 获取 CenterLog 库的连接字符串配置
            var connectionSetting = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
                CommonConst.Default.TenantNo, DatabaseTypeEnum.CenterLog);
            var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

            var requestLogModel = new RequestLogModel
            {
                RecordId = YitIdHelper.NextId(),
                AccountId = _user?.AccountId,
                Mobile = _user?.Mobile,
                NickName = _user?.NickName,
                IsSuccess = actionContext.Exception == null,
                OperationAction = apiInfoAttribute?.Action ?? HttpRequestActionEnum.None,
                OperationName = apiInfoAttribute?.Name,
                ClassName = context.Controller.ToString(),
                MethodName = actionDescriptor?.ActionName,
                Location = httpRequest.Path,
                RequestMethod = requestMethod,
                Param = context.ActionArguments.Count < 1 ? "" : context.ActionArguments.ToJsonString(),
                // 成功响应可能包含敏感数据且体积不受控，请求审计仅保存异常详情
                Result = actionContext.Exception?.ToString(),
                ElapsedTime = stopwatch.ElapsedMilliseconds,
                DepartmentId = _user?.DepartmentId,
                DepartmentName = _user?.DepartmentName,
                CreatedUserId = _user?.EmployeeId,
                CreatedUserName = _user?.EmployeeName,
                CreatedTime = dateTime,
                TenantId = _user?.TenantId,
                TenantName = _user?.TenantName
            };
            requestLogModel.RecordCreate(httpContext);

            // 独立客户端不加载 AOP，防止写审计日志再次触发审计；等待写入完成后才结束请求
            using var db = new SqlSugarClient(connectionConfig);
            await db.Insertable(requestLogModel)
                .SplitTable()
                .ExecuteCommandAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "保存请求审计日志失败。");
            throw;
        }
    }
}