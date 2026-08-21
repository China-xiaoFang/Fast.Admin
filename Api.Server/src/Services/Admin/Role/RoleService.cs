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
using Fast.Center.Entity;

namespace Fast.Admin.Service.Role;

/// <summary>
/// 角色服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "role")]
public partial class RoleService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<RoleModel> _repository;
    private readonly ISqlSugarClient _centerRepository;

    public RoleService(IUser user, ISqlSugarRepository<RoleModel> repository, ISqlSugarClient centerRepository)
    {
        _user = user;
        _repository = repository;
        _centerRepository = centerRepository;
    }

    /// <summary>
    /// 规范并校验可分配角色Id集合
    /// </summary>
    /// <param name="roleId">当前角色Id</param>
    /// <param name="roleIds">可分配角色Id集合</param>
    /// <returns>去重并验证后的可分配角色Id集合</returns>
    private async Task<List<long>> NormalizeAssignableRoleIds(long roleId, List<long> roleIds)
    {
        var result = (roleIds ?? []).Distinct()
            .ToList();
        if (result.Contains(roleId))
        {
            throw new UserFriendlyException("可分配角色不能包含角色自身！");
        }

        if (result.Count > 0
            && await _repository.Queryable<RoleModel>()
                .Where(wh => result.Contains(wh.RoleId))
                .CountAsync()
            != result.Count)
        {
            throw new UserFriendlyException("可分配角色数据不存在！");
        }

        return result;
    }

    /// <summary>
    /// 规范并校验自定义数据范围部门Id集合
    /// </summary>
    /// <returns>去重并验证后的自定义数据范围部门Id集合</returns>
    private async Task<List<long>> NormalizeDataScopeDepartmentIds(DataScopeTypeEnum dataScopeType, List<long> departmentIds)
    {
        if (dataScopeType != DataScopeTypeEnum.CustomDept)
        {
            return [];
        }

        var result = (departmentIds ?? []).Distinct()
            .ToList();
        if (result.Count > 0
            && await _repository.Queryable<DepartmentModel>()
                .Where(wh => result.Contains(wh.DepartmentId))
                .CountAsync()
            != result.Count)
        {
            throw new UserFriendlyException("自定义数据范围部门不存在！");
        }

        return result;
    }

    /// <summary>
    /// 获取角色授权所需的当前应用与租户上下文
    /// </summary>
    /// <returns>角色授权所需的当前应用和租户</returns>
    private async Task<(ApplicationOpenIdModel Application, TenantModel Tenant)> GetAuthorizationContext()
    {
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);
        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);
        return (applicationModel, tenantModel);
    }

    /// <summary>
    /// 清除所有关联指定角色职员的授权缓存
    /// </summary>
    private async Task RevokeRoleEmployees(long roleId)
    {
        var employeeIds = await _repository.Queryable<EmployeeRoleModel>()
            .Where(wh => wh.RoleId == roleId)
            .Select(sl => sl.EmployeeId)
            .Distinct()
            .ToListAsync();
        if (employeeIds.Count == 0)
        {
            return;
        }

        var employeeNos = await _repository.Queryable<EmployeeModel>()
            .Where(wh => employeeIds.Contains(wh.EmployeeId))
            .Select(sl => sl.EmployeeNo)
            .ToListAsync();
        foreach (var employeeNo in employeeNos.Where(wh => !string.IsNullOrWhiteSpace(wh)))
        {
            await _user.RevokeEmployee(_user.TenantNo, employeeNo);
        }
    }
}