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
using Fast.Admin.Service.Role.Dto;
using Fast.AdminLog.Enum;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yitter.IdGenerator;

namespace Fast.Admin.Service.Role;

public partial class RoleService
{
    /// <summary>
    /// 添加角色
    /// </summary>
    [HttpPost]
    [ApiInfo("添加角色", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Role.Add)]
    public async Task AddRole(AddRoleInput input)
    {
        if (await _repository.AnyAsync(a => a.RoleName == input.RoleName))
        {
            throw new UserFriendlyException("角色名称重复！");
        }

        if (await _repository.AnyAsync(a => a.RoleCode == input.RoleCode))
        {
            throw new UserFriendlyException("角色编码重复！");
        }

        var roleId = YitIdHelper.NextId();
        var roleModel = new RoleModel
        {
            RoleId = roleId,
            RoleType = input.RoleType,
            IsSystemMenu = input.IsSystemMenu,
            RoleName = input.RoleName,
            RoleCode = input.RoleCode,
            Sort = input.Sort,
            DataScopeType = input.DataScopeType,
            DataScopeDepartmentIds = await NormalizeDataScopeDepartmentIds(input.DataScopeType, input.DataScopeDepartmentIds),
            AssignableRoleIds = await NormalizeAssignableRoleIds(roleId, input.AssignableRoleIds),
            Remark = input.Remark
        };

        var templateMenuIds = new List<long>();
        var templateButtonIds = new List<long>();
        if (roleModel.IsSystemMenu)
        {
            var (applicationModel, tenantModel) = await GetAuthorizationContext();
            templateMenuIds = await _centerRepository.Queryable<MenuModel>()
                .Where(wh => wh.AppId == applicationModel.AppId)
                .Where(wh => wh.Status == CommonStatusEnum.Enable)
                .Where(wh => tenantModel.Edition >= wh.Edition)
                .Where(wh => wh.MenuType != MenuTypeEnum.Catalog)
                .Where(wh => (wh.RoleType & roleModel.RoleType) != 0)
                .Select(sl => sl.MenuId)
                .ToListAsync();
            templateButtonIds = await _centerRepository.Queryable<ButtonModel>()
                .Where(wh => wh.AppId == applicationModel.AppId)
                .Where(wh => wh.Status == CommonStatusEnum.Enable)
                .Where(wh => tenantModel.Edition >= wh.Edition)
                .Where(wh => templateMenuIds.Contains(wh.MenuId))
                .Where(wh => (wh.RoleType & roleModel.RoleType) != 0)
                .Select(sl => sl.ButtonId)
                .ToListAsync();
        }

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.InsertAsync(roleModel);
            if (templateMenuIds.Count > 0)
            {
                await _repository.Insertable(templateMenuIds
                        .Select(menuId => new RoleMenuModel {RoleId = roleModel.RoleId, MenuId = menuId})
                        .ToList())
                    .ExecuteCommandAsync();
            }

            if (templateButtonIds.Count > 0)
            {
                await _repository.Insertable(templateButtonIds
                        .Select(buttonId => new RoleButtonModel {RoleId = roleModel.RoleId, ButtonId = buttonId})
                        .ToList())
                    .ExecuteCommandAsync();
            }
        }, ex => throw ex);

        // 操作日志
        await LogContext.OperateLog(new OperateLogDto
        {
            Title = "添加角色",
            OperateType = OperateLogTypeEnum.Organization,
            BizId = roleModel.RoleId,
            BizNo = null,
            Description = $"添加角色：{roleModel.RoleName}"
        });
    }

    /// <summary>
    /// 编辑角色
    /// </summary>
    [HttpPost]
    [ApiInfo("编辑角色", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Role.Edit)]
    public async Task EditRole(EditRoleInput input)
    {
        var roleModel = await _repository.SingleOrDefaultAsync(input.RoleId);
        if (roleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.RoleName == input.RoleName && a.RoleId != input.RoleId))
        {
            throw new UserFriendlyException("角色名称重复！");
        }

        if (await _repository.AnyAsync(a => a.RoleCode == input.RoleCode && a.RoleId != input.RoleId))
        {
            throw new UserFriendlyException("角色编码重复！");
        }

        roleModel.RoleType = input.RoleType;
        roleModel.IsSystemMenu = input.IsSystemMenu;
        roleModel.RoleName = input.RoleName;
        roleModel.RoleCode = input.RoleCode;
        roleModel.Sort = input.Sort;
        roleModel.DataScopeType = input.DataScopeType;
        roleModel.DataScopeDepartmentIds = await NormalizeDataScopeDepartmentIds(input.DataScopeType,
            input.DataScopeDepartmentIds);
        roleModel.AssignableRoleIds = await NormalizeAssignableRoleIds(roleModel.RoleId, input.AssignableRoleIds);
        roleModel.Remark = input.Remark;
        roleModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(roleModel);

        await _repository.Updateable<EmployeeRoleModel>()
            .SetColumns(_ => new EmployeeRoleModel {RoleName = roleModel.RoleName})
            .Where(wh => wh.RoleId == roleModel.RoleId)
            .ExecuteCommandAsync();

        await RevokeRoleEmployees(roleModel.RoleId);

        // 操作日志
        await LogContext.OperateLog(new OperateLogDto
        {
            Title = "编辑角色",
            OperateType = OperateLogTypeEnum.Organization,
            BizId = roleModel.RoleId,
            BizNo = null,
            Description = $"编辑角色：{roleModel.RoleName}"
        });
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    [HttpPost]
    [ApiInfo("删除角色", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Role.Delete)]
    public async Task DeleteRole(RoleIdInput input)
    {
        var roleModel = await _repository.SingleOrDefaultAsync(input.RoleId);
        if (roleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        // 检查是否有职员关联
        if (await _repository.Queryable<EmployeeRoleModel>()
                .AnyAsync(a => a.RoleId == input.RoleId))
        {
            throw new UserFriendlyException("角色存在职员关联，无法删除！");
        }

        await _repository.Ado.UseTranAsync(async () =>
        {
            // 删除角色菜单关联
            await _repository.Deleteable<RoleMenuModel>()
                .Where(wh => wh.RoleId == input.RoleId)
                .ExecuteCommandAsync();

            // 删除角色按钮关联
            await _repository.Deleteable<RoleButtonModel>()
                .Where(wh => wh.RoleId == input.RoleId)
                .ExecuteCommandAsync();

            // 删除角色
            await _repository.DeleteAsync(roleModel);
        }, ex => throw ex);

        // 操作日志
        await LogContext.OperateLog(new OperateLogDto
        {
            Title = "删除角色",
            OperateType = OperateLogTypeEnum.Organization,
            BizId = roleModel.RoleId,
            BizNo = null,
            Description = $"删除角色：{roleModel.RoleName}"
        });
    }
}