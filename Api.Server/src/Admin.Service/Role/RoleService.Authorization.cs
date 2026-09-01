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
using Fast.Admin.Service.Role.Dto;
using Fast.AdminLog.Domain;
using Fast.Center.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Role;

public partial class RoleService
{
    /// <summary>
    /// 角色授权
    /// </summary>
    [HttpPost]
    [ApiInfo("角色授权", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Role.Edit)]
    public async Task RoleAuth(RoleAuthInput input)
    {
        var currentRoleIds = _user.RoleIdList ?? [];
        List<long> assignableRoleIds = null;
        if (!_user.IsSuperAdmin && !_user.IsAdmin)
        {
            var currentRoleList = await _repository.Queryable<RoleModel>()
                .Where(wh => currentRoleIds.Contains(wh.RoleId))
                .Select(sl => new {sl.AssignableRoleIds})
                .ToListAsync();
            assignableRoleIds = currentRoleList.Where(wh => wh.AssignableRoleIds?.Count > 0)
                .SelectMany(sl => sl.AssignableRoleIds)
                .Except(currentRoleIds)
                .Distinct()
                .ToList();
        }

        var roleModel = await _repository.Entities.Where(wh => wh.RoleId == input.RoleId)
            .WhereIF(assignableRoleIds != null, wh => assignableRoleIds.Contains(wh.RoleId))
            .SingleAsync();
        if (roleModel == null)
        {
            throw new UserFriendlyException("角色不存在或无权操作！");
        }

        var menuIds = (input.MenuIds ?? []).Distinct()
            .ToList();
        var buttonIds = (input.ButtonIds ?? []).Distinct()
            .ToList();
        var (applicationModel, tenantModel) = await GetAuthorizationContext();

        var menuList = await _centerRepository.Queryable<MenuModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .Where(wh => tenantModel.Edition >= wh.Edition)
            .Where(wh => wh.MenuType != MenuTypeEnum.Catalog)
            .Where(wh => menuIds.Contains(wh.MenuId))
            .Select(sl => new {sl.MenuId})
            .ToListAsync();
        if (menuList.Count != menuIds.Count)
        {
            throw new UserFriendlyException("授权菜单不属于当前应用、已禁用或超出租户版本！");
        }

        var buttonList = await _centerRepository.Queryable<ButtonModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .Where(wh => tenantModel.Edition >= wh.Edition)
            .Where(wh => buttonIds.Contains(wh.ButtonId))
            .Select(sl => new {sl.ButtonId, sl.MenuId})
            .ToListAsync();
        if (buttonList.Count != buttonIds.Count)
        {
            throw new UserFriendlyException("授权按钮不属于当前应用、已禁用或超出租户版本！");
        }

        if (buttonList.Any(button => !menuIds.Contains(button.MenuId)))
        {
            throw new UserFriendlyException("授权按钮必须属于已选择的菜单！");
        }

        if (!_user.IsSuperAdmin && !_user.IsAdmin)
        {
            var authorizedMenuIds = await _repository.Queryable<RoleMenuModel>()
                .Where(wh => currentRoleIds.Contains(wh.RoleId))
                .Select(sl => sl.MenuId)
                .Distinct()
                .ToListAsync();
            var authorizedButtonIds = await _repository.Queryable<RoleButtonModel>()
                .Where(wh => currentRoleIds.Contains(wh.RoleId))
                .Select(sl => sl.ButtonId)
                .Distinct()
                .ToListAsync();
            if (menuIds.Except(authorizedMenuIds)
                    .Any()
                || buttonIds.Except(authorizedButtonIds)
                    .Any())
            {
                throw new UserFriendlyException("无权授予超出自身权限范围的菜单或按钮！");
            }
        }

        var applicationMenuIds = await _centerRepository.Queryable<MenuModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Select(sl => sl.MenuId)
            .ToListAsync();
        var applicationButtonIds = await _centerRepository.Queryable<ButtonModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Select(sl => sl.ButtonId)
            .ToListAsync();

        roleModel.RowVersion = input.RowVersion;

        await _repository.Ado.UseTranAsync(async () =>
        {
            // 使用角色版本锁定本次授权，避免并发授权互相覆盖
            await _repository.UpdateAsync(roleModel);

            // 只替换当前应用权限，保留角色在其他应用的授权
            if (applicationMenuIds.Count > 0)
            {
                await _repository.Deleteable<RoleMenuModel>()
                    .Where(wh => wh.RoleId == roleModel.RoleId && applicationMenuIds.Contains(wh.MenuId))
                    .ExecuteCommandAsync();
            }

            // 添加新的菜单权限
            if (menuIds.Any())
            {
                await _repository.Insertable(menuIds
                        .Select(menuId => new RoleMenuModel {RoleId = roleModel.RoleId, MenuId = menuId})
                        .ToList())
                    .ExecuteCommandAsync();
            }

            if (applicationButtonIds.Count > 0)
            {
                await _repository.Deleteable<RoleButtonModel>()
                    .Where(wh => wh.RoleId == roleModel.RoleId && applicationButtonIds.Contains(wh.ButtonId))
                    .ExecuteCommandAsync();
            }

            // 添加新的按钮权限
            if (buttonIds.Any())
            {
                await _repository.Insertable(buttonIds
                        .Select(buttonId => new RoleButtonModel {RoleId = roleModel.RoleId, ButtonId = buttonId})
                        .ToList())
                    .ExecuteCommandAsync();
            }
        }, ex => throw ex);

        await RevokeRoleEmployees(roleModel.RoleId);

        // 操作日志
        await LogContext.OperateLog(new OperateLogDto
        {
            Title = "角色授权",
            OperateType = OperateLogTypeEnum.Organization,
            BizId = roleModel.RoleId,
            BizNo = null,
            Description = $"角色授权：{roleModel.RoleName}"
        });
    }

    /// <summary>
    /// 获取角色授权菜单
    /// </summary>
    [HttpPost]
    [ApiInfo("获取角色授权菜单", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Role.Edit)]
    public async Task<RoleAuthInput> QueryRoleAuthMenu(RoleIdInput input)
    {
        var roleModel = await _repository.SingleOrDefaultAsync(input.RoleId);
        if (roleModel == null)
        {
            throw new UserFriendlyException("角色不存在或无权操作！");
        }

        var (applicationModel, tenantModel) = await GetAuthorizationContext();
        var validMenuIds = await _centerRepository.Queryable<MenuModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .Where(wh => tenantModel.Edition >= wh.Edition)
            .Where(wh => wh.MenuType != MenuTypeEnum.Catalog)
            .Select(sl => sl.MenuId)
            .ToListAsync();
        var validButtonIds = await _centerRepository.Queryable<ButtonModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .Where(wh => tenantModel.Edition >= wh.Edition)
            .Where(wh => validMenuIds.Contains(wh.MenuId))
            .Select(sl => sl.ButtonId)
            .ToListAsync();

        var result = new RoleAuthInput
        {
            RoleId = roleModel.RoleId,
            RoleName = roleModel.RoleName,
            RowVersion = roleModel.RowVersion,
            MenuIds = await _repository.Queryable<RoleMenuModel>()
                .Where(wh => wh.RoleId == roleModel.RoleId && validMenuIds.Contains(wh.MenuId))
                .Select(sl => sl.MenuId)
                .ToListAsync(),
            ButtonIds = await _repository.Queryable<RoleButtonModel>()
                .Where(wh => wh.RoleId == roleModel.RoleId && validButtonIds.Contains(wh.ButtonId))
                .Select(sl => sl.ButtonId)
                .ToListAsync()
        };

        return result;
    }

    /// <summary>
    /// 获取授权菜单
    /// </summary>
    [HttpGet]
    [ApiInfo("获取授权菜单", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Role.Edit, PermissionConst.Employee.Edit)]
    public async Task<List<ElSelectorOutput<long>>> QueryAuthMenu()
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 查询租户信息
        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);

        // 查询当前用户角色。RoleType 只作为初始化模板，运行时授权统一读取关联表
        var roleIds = _user.RoleIdList ?? [];

        var menuQueryable = _centerRepository.Queryable<MenuModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .Where(wh => tenantModel.Edition >= wh.Edition)
            .Where(wh => wh.MenuType != MenuTypeEnum.Catalog);

        var buttonQueryable = _centerRepository.Queryable<ButtonModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .Where(wh => tenantModel.Edition >= wh.Edition);

        if (!_user.IsSuperAdmin && !_user.IsAdmin)
        {
            // 查询当前用户角色对应的菜单Id
            var roleMenuIds = await _repository.Queryable<RoleMenuModel>()
                .Where(wh => roleIds.Contains(wh.RoleId))
                .Select(sl => sl.MenuId)
                .ToListAsync();
            menuQueryable = menuQueryable.Where(wh => roleMenuIds.Contains(wh.MenuId));

            // 查询当前用户角色对应的按钮Id
            var roleButtonIds = await _repository.Queryable<RoleButtonModel>()
                .Where(wh => roleIds.Contains(wh.RoleId))
                .Select(sl => sl.ButtonId)
                .ToListAsync();
            buttonQueryable = buttonQueryable.Where(wh => roleButtonIds.Contains(wh.ButtonId));
        }

        // 查询所有菜单
        var menuList = await menuQueryable.Clone()
            .OrderBy(ob => ob.Sort)
            .Select(sl => new
            {
                sl.MenuId,
                sl.MenuName,
                sl.HasMobile,
                sl.HasWeb,
                sl.HasDesktop
            })
            .ToListAsync();

        // 查询所有按钮
        var buttonList = await buttonQueryable.Clone()
            .InnerJoin(menuQueryable.Clone(), (t1, t2) => t1.MenuId == t2.MenuId)
            .OrderBy(t1 => t1.Sort)
            .Select(t1 => new
            {
                t1.ButtonId,
                t1.MenuId,
                t1.ButtonName,
                t1.HasMobile,
                t1.HasWeb,
                t1.HasDesktop
            })
            .ToListAsync();

        var result = new List<ElSelectorOutput<long>>();

        foreach (var menuInfo in menuList.ToList())
        {
            var item = new ElSelectorOutput<long>
            {
                Value = menuInfo.MenuId,
                Label = menuInfo.MenuName,
                Data = new {menuInfo.HasMobile, menuInfo.HasWeb, menuInfo.HasDesktop},
                Children = []
            };
            foreach (var buttonInfo in buttonList.Where(wh => wh.MenuId == menuInfo.MenuId)
                         .ToList())
            {
                item.Children.Add(new ElSelectorOutput<long>
                {
                    Value = buttonInfo.ButtonId,
                    Label = buttonInfo.ButtonName,
                    Data = new {buttonInfo.HasMobile, buttonInfo.HasWeb, buttonInfo.HasDesktop}
                });
            }

            result.Add(item);
        }

        return result;
    }
}