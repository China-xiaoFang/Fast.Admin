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

using Fast.Admin.Entity;
using Fast.Admin.Enum;
using Fast.Admin.Service.OperateLog.Dto;
using Fast.AdminLog.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.OperateLog;

/// <summary>
/// <see cref="OperateLogService"/> 操作日志服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "operateLog")]
public class OperateLogService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<DepartmentModel> _adminRepository;
    private readonly ISqlSugarRepository<OperateLogModel> _repository;

    public OperateLogService(IUser user, ISqlSugarRepository<DepartmentModel> adminRepository,
        ISqlSugarRepository<OperateLogModel> repository)
    {
        _user = user;
        _adminRepository = adminRepository;
        _repository = repository;
    }

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取操作日志分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.OperateLogPaged)]
    public async Task<PagedResult<OperateLogModel>> QueryOperateLogPaged(QueryOperateLogPagedInput input)
    {
        if (input.SearchTimeList is not {Count: > 1})
        {
            throw new UserFriendlyException("请选择具体的时间范围！");
        }

        var queryable = _repository.Entities.WhereIF(input.OperateType != null, wh => wh.OperateType == input.OperateType)
            .WhereIF(input.EmployeeId != null, wh => wh.EmployeeId == input.EmployeeId)
            .WhereIF(input.BizId != null, wh => wh.BizId == input.BizId);

        // 仅本人数据
        if (_user.DataScopeType == (int) DataScopeTypeEnum.Self)
        {
            queryable = queryable.Where(wh => wh.EmployeeId == _user.UserId);
        }
        // 本部门数据
        else if (_user.DataScopeType == (int) DataScopeTypeEnum.Dept)
        {
            queryable = queryable.Where(wh => wh.DepartmentId == _user.DepartmentId);
        }
        // 本机构及以下数据
        else if (_user.DataScopeType == (int) DataScopeTypeEnum.OrgWithChild)
        {
            var departmentIds = await _adminRepository.Queryable<DepartmentModel>()
                .Where(wh => wh.OrgId
                             == SqlFunc.Subqueryable<EmployeeOrgModel>()
                                 // 主部门
                                 .Where(e => e.EmployeeId == _user.UserId && e.IsPrimary)
                                 .Where(e => e.OrgId == wh.OrgId)
                                 .Select(e => e.OrgId))
                .Select(sl => sl.DepartmentId)
                .ToListAsync();
            queryable = queryable.Where(wh => departmentIds.Contains(wh.DepartmentId ?? 0));
        } // 本部门及以下数据
        else if (_user.DataScopeType == (int) DataScopeTypeEnum.DeptWithChild)
        {
            var departmentIds = await _adminRepository.Queryable<DepartmentModel>()
                .Where(wh => wh.DepartmentId == _user.DepartmentId || wh.ParentIds.Contains(_user.DepartmentId ?? 0))
                .Select(sl => sl.DepartmentId)
                .ToListAsync();
            queryable = queryable.Where(wh => departmentIds.Contains(wh.DepartmentId ?? 0));
        }

        return await queryable.SplitTable()
            .OrderByIF(input.IsOrderBy, ob => ob.CreatedTime, OrderByType.Desc)
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 删除90天前的操作日志
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除90天前的操作日志", HttpRequestActionEnum.Delete)]
    public async Task DeleteOperateLog()
    {
        if (!_user.IsSuperAdmin && !_user.IsAdmin)
        {
            throw new UserFriendlyException("非管理员，禁止操作！");
        }

        var dateTime = DateTime.Now.AddDays(-90);

        var tableNames = _repository.SplitHelper<OperateLogModel>()
            .GetTables();

        // 删除空数据的表
        foreach (var tableInfo in tableNames)
        {
            await _repository.Deleteable<OperateLogModel>()
                .AS(tableInfo.TableName)
                .Where(wh => wh.CreatedTime < dateTime)
                .ExecuteCommandAsync();

            if (!await _repository.Entities.AS(tableInfo.TableName)
                    .AnyAsync())
            {
                _repository.DbMaintenance.DropTable(tableInfo.TableName);
            }
        }
    }
}