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

using Fast.Admin.Entity;
using Fast.Admin.Service.Organization.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Organization;

/// <summary>
/// <see cref="OrganizationService"/> 组织机构服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "organization")]
public class OrganizationService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<OrganizationModel> _repository;

    public OrganizationService(IUser user, ISqlSugarRepository<OrganizationModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 组织机构选择器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("组织机构选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> OrganizationSelector()
    {
        var data = await _repository.Entities
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .OrderBy(ob => ob.Sort)
            .Select(sl => new { sl.OrgId, sl.OrgName, sl.ParentId })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
        {
            Value = sl.OrgId,
            Label = sl.OrgName,
            Data = new { sl.ParentId }
        }).ToList();
    }

    /// <summary>
    /// 获取组织机构树形列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取组织机构树形列表", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Organization.Tree)]
    public async Task<List<QueryOrganizationTreeOutput>> QueryOrganizationTree(QueryOrganizationTreeInput input)
    {
        var data = await _repository.Entities
            .WhereIF(!string.IsNullOrEmpty(input.OrgName), wh => wh.OrgName.Contains(input.OrgName))
            .WhereIF(input.Status != null, wh => wh.Status == input.Status)
            .OrderBy(ob => ob.Sort)
            .Select(sl => new QueryOrganizationTreeOutput
            {
                OrgId = sl.OrgId,
                ParentId = sl.ParentId,
                OrgName = sl.OrgName,
                OrgCode = sl.OrgCode,
                Leader = sl.Leader,
                Phone = sl.Phone,
                Email = sl.Email,
                Sort = sl.Sort,
                Status = sl.Status,
                Remark = sl.Remark,
                CreatedTime = sl.CreatedTime,
                RowVersion = sl.RowVersion
            })
            .ToListAsync();

        // 构建树形结构
        return BuildOrganizationTree(data, 0);
    }

    /// <summary>
    /// 获取组织机构详情
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取组织机构详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Organization.Detail)]
    public async Task<QueryOrganizationDetailOutput> QueryOrganizationDetail([Required(ErrorMessage = "组织机构Id不能为空")] long? orgId)
    {
        var result = await _repository.Entities
            .Where(wh => wh.OrgId == orgId)
            .Select(sl => new QueryOrganizationDetailOutput
            {
                OrgId = sl.OrgId,
                ParentId = sl.ParentId,
                OrgName = sl.OrgName,
                OrgCode = sl.OrgCode,
                Leader = sl.Leader,
                Phone = sl.Phone,
                Email = sl.Email,
                Sort = sl.Sort,
                Status = sl.Status,
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

        // 获取父组织名称
        if (result.ParentId > 0)
        {
            var parentOrg = await _repository.Entities
                .Where(wh => wh.OrgId == result.ParentId)
                .Select(sl => sl.OrgName)
                .FirstAsync();
            result.ParentName = parentOrg;
        }

        return result;
    }

    /// <summary>
    /// 添加组织机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加组织机构", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Organization.Add)]
    public async Task AddOrganization(AddOrganizationInput input)
    {
        // 检查父组织是否存在
        if (input.ParentId > 0)
        {
            if (!await _repository.AnyAsync(a => a.OrgId == input.ParentId))
            {
                throw new UserFriendlyException("父组织不存在！");
            }
        }

        if (await _repository.AnyAsync(a => a.OrgName == input.OrgName))
        {
            throw new UserFriendlyException("组织机构名称重复！");
        }

        if (await _repository.AnyAsync(a => a.OrgCode == input.OrgCode))
        {
            throw new UserFriendlyException("组织机构编码重复！");
        }

        var organizationModel = new OrganizationModel
        {
            ParentId = input.ParentId ?? 0,
            OrgName = input.OrgName,
            OrgCode = input.OrgCode,
            Leader = input.Leader,
            Phone = input.Phone,
            Email = input.Email,
            Sort = input.Sort,
            Status = CommonStatusEnum.Enable,
            Remark = input.Remark
        };

        await _repository.InsertAsync(organizationModel);
    }

    /// <summary>
    /// 编辑组织机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑组织机构", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Organization.Edit)]
    public async Task EditOrganization(EditOrganizationInput input)
    {
        // 检查父组织是否存在
        if (input.ParentId > 0)
        {
            if (!await _repository.AnyAsync(a => a.OrgId == input.ParentId))
            {
                throw new UserFriendlyException("父组织不存在！");
            }

            // 不能将自己设为父组织
            if (input.ParentId == input.OrgId)
            {
                throw new UserFriendlyException("不能将自己设为父组织！");
            }

            // 检查是否循环引用
            if (await IsCircularReference(input.OrgId, input.ParentId.Value))
            {
                throw new UserFriendlyException("存在循环引用！");
            }
        }

        var organizationModel = await _repository.SingleOrDefaultAsync(input.OrgId);
        if (organizationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.OrgName == input.OrgName && a.OrgId != input.OrgId))
        {
            throw new UserFriendlyException("组织机构名称重复！");
        }

        if (await _repository.AnyAsync(a => a.OrgCode == input.OrgCode && a.OrgId != input.OrgId))
        {
            throw new UserFriendlyException("组织机构编码重复！");
        }

        organizationModel.ParentId = input.ParentId ?? 0;
        organizationModel.OrgName = input.OrgName;
        organizationModel.OrgCode = input.OrgCode;
        organizationModel.Leader = input.Leader;
        organizationModel.Phone = input.Phone;
        organizationModel.Email = input.Email;
        organizationModel.Sort = input.Sort;
        organizationModel.Status = input.Status;
        organizationModel.Remark = input.Remark;
        organizationModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(organizationModel);
    }

    /// <summary>
    /// 删除组织机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除组织机构", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Organization.Delete)]
    public async Task DeleteOrganization(OrganizationIdInput input)
    {
        // 检查是否有子组织
        if (await _repository.AnyAsync(a => a.ParentId == input.OrgId))
        {
            throw new UserFriendlyException("组织机构存在子组织，无法删除！");
        }

        // 检查是否有部门关联
        if (await _repository.Queryable<DepartmentModel>()
            .AnyAsync(a => a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("组织机构存在部门关联，无法删除！");
        }

        // 检查是否有职位关联
        if (await _repository.Queryable<PositionModel>()
            .AnyAsync(a => a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("组织机构存在职位关联，无法删除！");
        }

        var organizationModel = await _repository.SingleOrDefaultAsync(input.OrgId);
        if (organizationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(organizationModel);
    }

    /// <summary>
    /// 构建组织机构树形结构
    /// </summary>
    /// <param name="allOrganizations">所有组织</param>
    /// <param name="parentId">父级Id</param>
    /// <returns></returns>
    private List<QueryOrganizationTreeOutput> BuildOrganizationTree(List<QueryOrganizationTreeOutput> allOrganizations, long parentId)
    {
        var children = allOrganizations.Where(o => o.ParentId == parentId).ToList();
        
        foreach (var child in children)
        {
            child.Children = BuildOrganizationTree(allOrganizations, child.OrgId);
        }

        return children;
    }

    /// <summary>
    /// 检查是否循环引用
    /// </summary>
    /// <param name="currentId">当前组织Id</param>
    /// <param name="targetParentId">目标父组织Id</param>
    /// <returns></returns>
    private async Task<bool> IsCircularReference(long currentId, long targetParentId)
    {
        var parentId = targetParentId;
        
        while (parentId > 0)
        {
            if (parentId == currentId)
            {
                return true;
            }

            var parent = await _repository.Entities
                .Where(wh => wh.OrgId == parentId)
                .Select(sl => sl.ParentId)
                .FirstAsync();

            parentId = parent;
        }

        return false;
    }
}
