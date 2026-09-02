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

using Fast.Admin.Domain;
using Fast.Admin.Service.Employee.Dto;
using Microsoft.AspNetCore.SignalR;

namespace Fast.Admin.Service.Employee;

/// <summary>
/// 职员服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "employee")]
public partial class EmployeeService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<EmployeeModel> _repository;
    private readonly ISqlSugarClient _centerRepository;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public EmployeeService(IUser user, ISqlSugarRepository<EmployeeModel> repository, ISqlSugarClient centerRepository,
        IHubContext<ChatHub, IChatClient> hubContext)
    {
        _user = user;
        _repository = repository;
        _centerRepository = centerRepository;
        _hubContext = hubContext;
    }

    /// <summary>
    /// 按当前用户的数据权限获取职员
    /// </summary>
    /// <returns>当前用户数据权限范围内的职员</returns>
    private async Task<EmployeeModel> GetEmployeeWithinDataScope(long employeeId)
    {
        var hasAccess = await _repository.Entities
            .LeftJoin<EmployeeOrgModel>((t1, t2) => t1.EmployeeId == t2.EmployeeId && t2.IsPrimary)
            .SelectMergeTable((t1, t2) => new QueryEmployeeSelectorDto
            {
                EmployeeId = t1.EmployeeId, DepartmentId = t2.DepartmentId
            })
            .DataScope(e => e.DepartmentId, e => e.EmployeeId, allowPublicData: false)
            .AnyAsync(e => e.EmployeeId == employeeId);
        if (!hasAccess)
        {
            throw new UserFriendlyException("数据不存在或无权操作！");
        }

        var employeeModel = await _repository.SingleOrDefaultAsync(employeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在或无权操作！");
        }

        return employeeModel;
    }

    /// <summary>
    /// 校验当前用户是否有权分配请求中的角色
    /// </summary>
    /// <param name="requestedRoleIds">请求分配的角色Id集合</param>
    /// <param name="existingRoleIds">职员当前角色Id集合，编辑时用于计算角色变更范围</param>
    private async Task ValidateRoleAssignment(List<long> requestedRoleIds, List<long> existingRoleIds = null)
    {
        if (_user.IsSuperAdmin || _user.IsAdmin)
        {
            return;
        }

        var currentRoleIds = _user.RoleIdList ?? [];
        var roleList = await _repository.Queryable<RoleModel>()
            .Where(wh => currentRoleIds.Contains(wh.RoleId))
            .Select(sl => new {sl.AssignableRoleIds})
            .ToListAsync();
        var assignableRoleIds = roleList.Where(wh => wh.AssignableRoleIds?.Count > 0)
            .SelectMany(sl => sl.AssignableRoleIds)
            .Except(currentRoleIds)
            .Distinct()
            .ToList();
        var changedRoleIds = existingRoleIds == null
            ? requestedRoleIds
            : requestedRoleIds.Except(existingRoleIds)
                .Concat(existingRoleIds.Except(requestedRoleIds));
        if (changedRoleIds.Except(assignableRoleIds)
            .Any())
        {
            throw new UserFriendlyException("无权分配超出自身权限范围的角色！");
        }
    }
}