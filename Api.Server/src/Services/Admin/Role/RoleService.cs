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
using Fast.Admin.Service.Role.Dto;
using Fast.Center.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Role;

/// <summary>
/// <see cref="RoleService"/> 角色服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "role")]
public class RoleService : IDynamicApplication
{
    private readonly ISqlSugarRepository<RoleModel> _repository;

    public RoleService(ISqlSugarRepository<RoleModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 角色选择器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("角色选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> RoleSelector()
    {
        var data = await _repository.Entities.OrderBy(ob => ob.RoleName)
            .Select(sl => new {sl.RoleId, sl.RoleName, sl.RoleCode})
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long> {Value = sl.RoleId, Label = sl.RoleName, Data = new {sl.RoleCode}})
            .ToList();
    }

    /// <summary>
    /// 获取角色分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取角色分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Role.Paged)]
    public async Task<PagedResult<QueryRolePagedOutput>> QueryRolePaged(QueryRolePagedInput input)
    {
        return await _repository.Entities.WhereIF(input.RoleType != null, wh => wh.RoleType == input.RoleType)
            .WhereIF(input.DataScopeType != null, wh => wh.DataScopeType == input.DataScopeType)
            .OrderBy(ob => ob.Sort)
            .ToPagedListAsync(input,
                sl => new QueryRolePagedOutput
                {
                    RoleId = sl.RoleId,
                    RoleType = sl.RoleType,
                    RoleName = sl.RoleName,
                    RoleCode = sl.RoleCode,
                    Sort = sl.Sort,
                    DataScopeType = sl.DataScopeType,
                    Remark = sl.Remark,
                    DepartmentName = sl.DepartmentName,
                    CreatedUserName = sl.CreatedUserName,
                    CreatedTime = sl.CreatedTime,
                    UpdatedUserName = sl.UpdatedUserName,
                    UpdatedTime = sl.UpdatedTime,
                    RowVersion = sl.RowVersion
                });
    }

    /// <summary>
    /// 获取角色详情
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取角色详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Role.Detail)]
    public async Task<QueryRoleDetailOutput> QueryRoleDetail([Required(ErrorMessage = "角色Id不能为空")] long? roleId)
    {
        var result = await _repository.Entities.Where(wh => wh.RoleId == roleId)
            .Select(sl => new QueryRoleDetailOutput
            {
                RoleId = sl.RoleId,
                RoleType = sl.RoleType,
                RoleName = sl.RoleName,
                RoleCode = sl.RoleCode,
                Sort = sl.Sort,
                DataScopeType = sl.DataScopeType,
                Remark = sl.Remark,
                DepartmentName = sl.DepartmentName,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime,
                RowVersion = sl.RowVersion
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }

    /// <summary>
    /// 添加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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

        var roleModel = new RoleModel
        {
            RoleType = RoleTypeEnum.Normal,
            RoleName = input.RoleName,
            RoleCode = input.RoleCode,
            Sort = input.Sort,
            DataScopeType = input.DataScopeType,
            Remark = input.Remark
        };

        await _repository.InsertAsync(roleModel);
    }

    /// <summary>
    /// 编辑角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑角色", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Role.Edit)]
    public async Task EditRole(EditRoleInput input)
    {
        if (await _repository.AnyAsync(a => a.RoleName == input.RoleName && a.RoleId != input.RoleId))
        {
            throw new UserFriendlyException("角色名称重复！");
        }

        if (await _repository.AnyAsync(a => a.RoleCode == input.RoleCode && a.RoleId != input.RoleId))
        {
            throw new UserFriendlyException("角色编码重复！");
        }

        var roleModel = await _repository.SingleOrDefaultAsync(input.RoleId);
        if (roleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (roleModel.RoleType == RoleTypeEnum.Admin)
        {
            throw new UserFriendlyException("禁止修改管理员角色！");
        }

        roleModel.RoleName = input.RoleName;
        roleModel.RoleCode = input.RoleCode;
        roleModel.Sort = input.Sort;
        roleModel.DataScopeType = input.DataScopeType;
        roleModel.Remark = input.Remark;
        roleModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(roleModel);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除角色", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Role.Delete)]
    public async Task DeleteRole(RoleIdInput input)
    {
        // 检查是否有职员关联
        if (await _repository.Queryable<EmployeeRoleModel>()
                .AnyAsync(a => a.RoleId == input.RoleId))
        {
            throw new UserFriendlyException("角色存在职员关联，无法删除！");
        }

        var roleModel = await _repository.SingleOrDefaultAsync(input.RoleId);
        if (roleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
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
    }

    /// <summary>
    /// 角色授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("角色授权", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Role.Edit)]
    public async Task RoleAuthMenu(RoleAuthInput input)
    {
        var roleModel = await _repository.SingleOrDefaultAsync(input.RoleId);
        if (roleModel == null)
        {
            throw new UserFriendlyException("角色不存在！");
        }

        // 验证菜单是否都存在
        var menuIds = input.MenuIds ?? [];
        if (menuIds.Any())
        {
            if (await _repository.Queryable<MenuModel>()
                    .Where(wh => menuIds.Contains(wh.MenuId))
                    .Select(sl => sl.MenuId)
                    .CountAsync()
                != menuIds.Count)
            {
                throw new UserFriendlyException("授权菜单数据不存在！");
            }
        }

        // 验证按钮是否都存在
        var buttonIds = input.ButtonIds ?? [];
        if (buttonIds.Any())
        {
            var existButtonIds = await _repository.Queryable<ButtonModel>()
                .Where(wh => buttonIds.Contains(wh.ButtonId))
                .Select(sl => sl.ButtonId)
                .ToListAsync();

            if (existButtonIds.Count != buttonIds.Count)
            {
                throw new UserFriendlyException("授权按钮数据不存在！");
            }
        }

        await _repository.Ado.UseTranAsync(async () =>
        {
            // 删除旧的菜单权限
            await _repository.Deleteable<RoleMenuModel>()
                .Where(wh => wh.RoleId == input.RoleId)
                .ExecuteCommandAsync();

            // 添加新的菜单权限
            if (menuIds.Any())
            {
                await _repository.Insertable(menuIds.Select(menuId => new RoleMenuModel {RoleId = input.RoleId, MenuId = menuId})
                        .ToList())
                    .ExecuteCommandAsync();
            }

            // 删除旧的按钮权限
            await _repository.Deleteable<RoleButtonModel>()
                .Where(wh => wh.RoleId == input.RoleId)
                .ExecuteCommandAsync();

            // 添加新的按钮权限
            if (buttonIds.Any())
            {
                await _repository.Insertable(buttonIds
                        .Select(buttonId => new RoleButtonModel {RoleId = input.RoleId, ButtonId = buttonId})
                        .ToList())
                    .ExecuteCommandAsync();
            }
        }, ex => throw ex);
    }
}