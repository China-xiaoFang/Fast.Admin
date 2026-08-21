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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Role;

public partial class RoleService
{
    /// <summary>
    /// 角色选择器
    /// </summary>
    [HttpGet]
    [ApiInfo("角色选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> RoleSelector()
    {
        var queryable = _repository.Entities;
        if (!_user.IsSuperAdmin && !_user.IsAdmin)
        {
            var roleIds = _user.RoleIdList ?? [];
            var roleList = await _repository.Queryable<RoleModel>()
                .Where(wh => roleIds.Contains(wh.RoleId))
                .Select(sl => new {sl.AssignableRoleIds})
                .ToListAsync();
            var assignableRoleIds = roleList.Where(wh => wh.AssignableRoleIds?.Count > 0)
                .SelectMany(sl => sl.AssignableRoleIds)
                .Except(roleIds)
                .Distinct()
                .ToList();

            if (assignableRoleIds.Count == 0)
            {
                return [];
            }

            queryable = queryable.Where(wh => assignableRoleIds.Contains(wh.RoleId));
        }

        var data = await queryable.OrderBy(ob => ob.Sort)
            .Select(sl => new {sl.RoleId, sl.RoleName, sl.RoleCode})
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long> {Value = sl.RoleId, Label = sl.RoleName, Data = new {sl.RoleCode}})
            .ToList();
    }

    /// <summary>
    /// 获取角色分页列表
    /// </summary>
    [HttpPost]
    [ApiInfo("获取角色分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Role.Paged)]
    public async Task<PagedResult<QueryRolePagedOutput>> QueryRolePaged(QueryRolePagedInput input)
    {
        return await _repository.Entities.WhereIF(input.RoleType != null, wh => wh.RoleType == input.RoleType)
            .WhereIF(input.DataScopeType != null, wh => wh.DataScopeType == input.DataScopeType)
            .OrderByIF(input.IsOrderBy, ob => ob.Sort)
            .Select(sl => new QueryRolePagedOutput
            {
                RoleId = sl.RoleId,
                RoleType = sl.RoleType,
                IsSystemMenu = sl.IsSystemMenu,
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
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取角色详情
    /// </summary>
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
                IsSystemMenu = sl.IsSystemMenu,
                RoleName = sl.RoleName,
                RoleCode = sl.RoleCode,
                Sort = sl.Sort,
                DataScopeType = sl.DataScopeType,
                DataScopeDepartmentIds = sl.DataScopeDepartmentIds,
                AssignableRoleIds = sl.AssignableRoleIds,
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
}