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
using Fast.Admin.Service.Organization.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Organization;

/// <summary>
/// <see cref="OrganizationService"/> 机构服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "organization")]
public class OrganizationService : IDynamicApplication
{
    private readonly ISqlSugarRepository<OrganizationModel> _repository;

    public OrganizationService(ISqlSugarRepository<OrganizationModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 机构树形列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("机构树形列表", HttpRequestActionEnum.Query)]
    public async Task<List<ElTreeOutput<long>>> OrganizationTree()
    {
        var data = await _repository.Entities.OrderBy(ob => ob.Sort)
            .Select(sl => new
            {
                sl.OrgId,
                sl.ParentId,
                sl.ParentIds,
                sl.OrgName,
                sl.OrgCode,
                sl.Contacts,
                sl.Phone
            })
            .ToListAsync();

        return data.Select(sl => new ElTreeOutput<long>
            {
                Value = sl.OrgId,
                Label = sl.OrgName,
                ParentId = sl.ParentId,
                Data = new {sl.ParentIds, sl.OrgCode, sl.Contacts, sl.Phone}
            })
            .ToList();
    }

    /// <summary>
    /// 获取机构详情
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取机构详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Department.Detail)]
    public async Task<QueryOrganizationDetailOutput> QueryOrganizationDetail([Required(ErrorMessage = "机构Id不能为空")] long? orgId)
    {
        var result = await _repository.Entities.Where(wh => wh.OrgId == orgId)
            .Select(sl => new QueryOrganizationDetailOutput
            {
                OrgId = sl.OrgId,
                ParentId = sl.ParentId,
                ParentIds = sl.ParentIds,
                OrgName = sl.OrgName,
                OrgCode = sl.OrgCode,
                Contacts = sl.Contacts,
                Phone = sl.Phone,
                Email = sl.Email,
                Sort = sl.Sort,
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
    /// 添加机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加机构", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Department.Add)]
    public async Task AddOrganization(AddOrganizationInput input)
    {
        if (await _repository.AnyAsync(a => a.OrgName == input.OrgName))
        {
            throw new UserFriendlyException("机构名称重复！");
        }

        if (await _repository.AnyAsync(a => a.OrgCode == input.OrgCode))
        {
            throw new UserFriendlyException("机构编码重复！");
        }

        var organizationModel = new OrganizationModel
        {
            OrgName = input.OrgName,
            OrgCode = input.OrgCode,
            Contacts = input.Contacts,
            Phone = input.Phone,
            Email = input.Email,
            Sort = input.Sort,
            Remark = input.Remark
        };

        if (input.ParentId > 0)
        {
            var parentOrganization = await _repository.SingleOrDefaultAsync(s => s.OrgId == input.ParentId);
            if (parentOrganization == null)
            {
                throw new UserFriendlyException("数据不存在！");
            }

            organizationModel.ParentId = parentOrganization.OrgId;
            organizationModel.ParentIds = parentOrganization.ParentIds;
            organizationModel.ParentIds.Add(parentOrganization.OrgId);
        }
        else
        {
            organizationModel.ParentId = 0;
            organizationModel.ParentIds = [0];
        }

        await _repository.InsertAsync(organizationModel);
    }

    /// <summary>
    /// 编辑机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑机构", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Department.Edit)]
    public async Task EditOrganization(EditOrganizationInput input)
    {
        if (input.ParentId == input.OrgId)
        {
            throw new UserFriendlyException("不能将自己设为父机构！");
        }

        var organizationModel = await _repository.SingleOrDefaultAsync(input.OrgId);
        if (organizationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.OrgName == input.OrgName && a.OrgId != input.OrgId))
        {
            throw new UserFriendlyException("机构名称重复！");
        }

        if (await _repository.AnyAsync(a => a.OrgCode == input.OrgCode && a.OrgId != input.OrgId))
        {
            throw new UserFriendlyException("机构编码重复！");
        }

        organizationModel.OrgName = input.OrgName;
        organizationModel.OrgCode = input.OrgCode;
        organizationModel.Contacts = input.Contacts;
        organizationModel.Phone = input.Phone;
        organizationModel.Email = input.Email;
        organizationModel.Sort = input.Sort;
        organizationModel.Remark = input.Remark;
        organizationModel.RowVersion = input.RowVersion;

        if (input.ParentId > 0)
        {
            var parentOrganization = await _repository.SingleOrDefaultAsync(s => s.OrgId == input.ParentId);
            if (parentOrganization == null)
            {
                throw new UserFriendlyException("数据不存在！");
            }

            organizationModel.ParentId = parentOrganization.OrgId;
            organizationModel.ParentIds = parentOrganization.ParentIds;
            organizationModel.ParentIds.Add(parentOrganization.OrgId);
        }
        else
        {
            organizationModel.ParentId = 0;
            organizationModel.ParentIds = [0];
        }

        await _repository.UpdateAsync(organizationModel);
    }

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除机构", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Department.Delete)]
    public async Task DeleteOrganization(OrganizationIdInput input)
    {
        if (await _repository.AnyAsync(a => a.ParentId == input.OrgId))
        {
            throw new UserFriendlyException("机构存在子机构，无法删除！");
        }

        // 检查是否有部门关联
        if (await _repository.Queryable<DepartmentModel>()
                .AnyAsync(a => a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("机构存在部门关联，无法删除！");
        }

        var organizationModel = await _repository.SingleOrDefaultAsync(input.OrgId);
        if (organizationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(organizationModel);
    }
}