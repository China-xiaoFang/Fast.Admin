using System.Diagnostics;
using System.Reflection;
using Fast.CenterLog.Entity;
using Fast.SqlSugar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Yitter.IdGenerator;


namespace Fast.Core;

/// <summary>
/// <see cref="RequestActionFilter"/> 请求日志拦截
/// </summary>
public class RequestActionFilter : IAsyncActionFilter
{
    /// <summary>
    /// SqlSugar实体服务
    /// </summary>
    private readonly ISqlSugarEntityService _sqlSugarEntityService;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="RequestActionFilter"/> 请求日志拦截
    /// </summary>
    /// <param name="sqlSugarEntityService"><see cref="ISqlSugarEntityService"/> SqlSugar实体服务</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public RequestActionFilter(ISqlSugarEntityService sqlSugarEntityService, ILogger<IAsyncActionFilter> logger)
    {
        _sqlSugarEntityService = sqlSugarEntityService;
        _logger = logger;
    }

    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
    /// <param name="next">
    /// The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.
    /// </param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion indicates the filter has executed.</returns>
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

        var actionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;
        var apiInfoAttribute = actionDescriptor?.MethodInfo.GetCustomAttribute<ApiInfoAttribute>();
        if (!Enum.TryParse(httpRequest.Method, true, out HttpRequestMethodEnum requestMethod))
        {
            // 默认 Get 请求
            requestMethod = HttpRequestMethodEnum.Get;
        }

        // 获取 CenterLog 库的连接字符串配置
        var connectionSetting = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
            CommonConst.Default.TenantNo, DatabaseTypeEnum.CenterLog);
        var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

        var operateLogModel = new OperateLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user?.AccountId,
            Account = _user?.Account,
            Mobile = _user?.Mobile,
            NickName = _user?.NickName,
            Success = actionContext.Exception == null ? YesOrNotEnum.Y : YesOrNotEnum.N,
            OperationAction = apiInfoAttribute?.Action ?? HttpRequestActionEnum.None,
            OperationName = apiInfoAttribute?.Name,
            ClassName = context.Controller.ToString(),
            MethodName = actionDescriptor?.ActionName,
            Location = httpRequest.Path,
            RequestMethod = requestMethod,
            Param = context.ActionArguments.Count < 1 ? "" : context.ActionArguments.ToJsonString(),
            Result = actionContext.Result?.GetType() == typeof(JsonResult) ? actionContext.Result.ToJsonString() : "",
            ElapsedTime = stopwatch.ElapsedMilliseconds,
            DepartmentId = _user?.DepartmentId,
            DepartmentName = _user?.DepartmentName,
            CreatedUserId = _user?.UserId,
            CreatedUserName = _user?.EmployeeName,
            CreatedTime = dateTime,
            TenantId = _user?.TenantId ?? 0
        };
        operateLogModel.RecordCreate(httpContext);

        _ = Task.Run(async () =>
        {
            try
            {
                // 这里不能使用Aop
                var db = new SqlSugarClient(connectionConfig);

                // 异步不等待
                await db.Insertable(operateLogModel)
                    .SplitTable()
                    .ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Request Action 执行Sql，保存失败；{ex.Message}");
            }
        });
    }
}