// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称"软件"）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using Fast.CenterLog.Service.ExceptionLog.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.CenterLog.Service.ExceptionLog;

/// <summary>
/// <see cref="ExceptionLogService"/> 异常日志服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.CenterLog, Name = "exceptionLog")]
public class ExceptionLogService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<ExceptionLogModel> _repository;

    public ExceptionLogService(IUser user, ISqlSugarRepository<ExceptionLogModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 获取异常日志分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取异常日志分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.ExceptionLog.Paged)]
    public async Task<PagedResult<QueryExceptionLogPagedOutput>> QueryExceptionLogPaged(QueryExceptionLogPagedInput input)
    {
        return await _repository.Entities
            .WhereIF(!string.IsNullOrEmpty(input.Account), wh => wh.Account.Contains(input.Account))
            .WhereIF(!string.IsNullOrEmpty(input.ClassName), wh => wh.ClassName.Contains(input.ClassName))
            .WhereIF(!string.IsNullOrEmpty(input.Message), wh => wh.Message.Contains(input.Message))
            .WhereIF(input.StartTime != null, wh => wh.CreatedTime >= input.StartTime)
            .WhereIF(input.EndTime != null, wh => wh.CreatedTime <= input.EndTime)
            .OrderByDescending(ob => ob.CreatedTime)
            .ToPagedListAsync(input, sl => new QueryExceptionLogPagedOutput
            {
                RecordId = sl.RecordId,
                Account = sl.Account,
                NickName = sl.NickName,
                ClassName = sl.ClassName,
                MethodName = sl.MethodName,
                Message = sl.Message,
                CreatedTime = sl.CreatedTime
            });
    }

    /// <summary>
    /// 获取异常日志详情
    /// </summary>
    /// <param name="recordId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取异常日志详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.ExceptionLog.Detail)]
    public async Task<QueryExceptionLogDetailOutput> QueryExceptionLogDetail([Required(ErrorMessage = "异常日志Id不能为空")] long? recordId)
    {
        var result = await _repository.Entities
            .Where(wh => wh.RecordId == recordId)
            .Select(sl => new QueryExceptionLogDetailOutput
            {
                RecordId = sl.RecordId,
                AccountId = sl.AccountId,
                Account = sl.Account,
                Mobile = sl.Mobile,
                NickName = sl.NickName,
                ClassName = sl.ClassName,
                MethodName = sl.MethodName,
                Message = sl.Message,
                Source = sl.Source,
                StackTrace = sl.StackTrace,
                ParamsObj = sl.ParamsObj,
                CreatedTime = sl.CreatedTime,
                TenantId = sl.TenantId
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }
}
