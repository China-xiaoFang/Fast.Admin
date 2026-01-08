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

using Fast.CenterLog.Service.LoginLog.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.CenterLog.Service.LoginLog;

/// <summary>
/// <see cref="LoginLogService"/> 登录日志服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.CenterLog, Name = "loginLog")]
public class LoginLogService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<LoginLogModel> _repository;

    public LoginLogService(IUser user, ISqlSugarRepository<LoginLogModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 获取登录日志分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取登录日志分页列表", HttpRequestActionEnum.Paged)]
    public async Task<PagedResult<QueryLoginLogPagedOutput>> QueryLoginLogPaged(QueryLoginLogPagedInput input)
    {
        return await _repository.Entities
            .WhereIF(!string.IsNullOrEmpty(input.UserName), wh => wh.UserName.Contains(input.UserName))
            .WhereIF(!string.IsNullOrEmpty(input.Ip), wh => wh.Ip.Contains(input.Ip))
            .WhereIF(input.Success != null, wh => wh.Success == input.Success)
            .WhereIF(input.StartTime != null, wh => wh.LoginTime >= input.StartTime)
            .WhereIF(input.EndTime != null, wh => wh.LoginTime <= input.EndTime)
            .OrderByDescending(ob => ob.LoginTime)
            .ToPagedListAsync(input, sl => new QueryLoginLogPagedOutput
            {
                LoginLogId = sl.LoginLogId,
                UserName = sl.UserName,
                Ip = sl.Ip,
                Location = sl.Location,
                Browser = sl.Browser,
                Os = sl.Os,
                Success = sl.Success,
                Message = sl.Message,
                LoginTime = sl.LoginTime
            });
    }

    /// <summary>
    /// 获取登录日志详情
    /// </summary>
    /// <param name="loginLogId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取登录日志详情", HttpRequestActionEnum.Query)]
    public async Task<QueryLoginLogDetailOutput> QueryLoginLogDetail([Required(ErrorMessage = "登录日志Id不能为空")] long? loginLogId)
    {
        var result = await _repository.Entities
            .Where(wh => wh.LoginLogId == loginLogId)
            .Select(sl => new QueryLoginLogDetailOutput
            {
                LoginLogId = sl.LoginLogId,
                UserName = sl.UserName,
                Ip = sl.Ip,
                Location = sl.Location,
                Browser = sl.Browser,
                Os = sl.Os,
                Success = sl.Success,
                Message = sl.Message,
                UserAgent = sl.UserAgent,
                LoginTime = sl.LoginTime
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }
}
