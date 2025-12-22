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
using Fast.Admin.Service.Department.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Department;

/// <summary>
/// <see cref="DepartmentService"/> 部门服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "department")]
public class DepartmentService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<DepartmentModel> _repository;

    public DepartmentService(IUser user, ISqlSugarRepository<DepartmentModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 部门选择器
    /// </summary>
    /// <param name="orgId">组织Id</param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("部门选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> DepartmentSelector(long? orgId)
    {
        var data = await _repository.Entities
            .WhereIF(orgId != null, wh => wh.OrgId == orgId)
            .OrderBy(ob => ob.Sort)
            .Select(sl => new { sl.DepartmentId, sl.DepartmentName, sl.ParentId, sl.OrgId })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
        {
            Value = sl.DepartmentId,
            Label = sl.DepartmentName,
            Data = new { sl.ParentId, sl.OrgId }
        }).ToList();
    }

    /// <summary>
    /// 获取部门树形列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取部门树形列表", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Department.Tree)]
    public async Task<List<QueryDepartmentTreeOutput>> QueryDepartmentTree(QueryDepartmentTreeInput input)
    {
        var data = await _repository.Entities
            .WhereIF(input.OrgId != null, wh => wh.OrgId == input.OrgId)
            .WhereIF(!string.IsNullOrEmpty(input.DepartmentName), wh => wh.DepartmentName.Contains(input.DepartmentName))
            .OrderBy(ob => ob.Sort)
            .Select(sl => new QueryDepartmentTreeOutput
            {
                DepartmentId = sl.DepartmentId,
                ParentId = sl.ParentId,
                OrgId = sl.OrgId,
                DepartmentName = sl.DepartmentName,
                DepartmentCode = sl.DepartmentCode,
                Contacts = sl.Contacts,
                Phone = sl.Phone,
                Sort = sl.Sort,
                Remark = sl.Remark,
                CreatedTime = sl.CreatedTime,
                RowVersion = sl.RowVersion
            })
            .ToListAsync();

        // 构建树形结构
        return BuildDepartmentTree(data, 0);
    }

    /// <summary>
    /// 获取部门详情
    /// </summary>
    /// <param name="departmentId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取部门详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Department.Detail)]
    public async Task<QueryDepartmentDetailOutput> QueryDepartmentDetail([Required(ErrorMessage = "部门Id不能为空")] long? departmentId)
    {
        var result = await _repository.Entities
            .LeftJoin<OrganizationModel>((t1, t2) => t1.OrgId == t2.OrgId)
            .Where(t1 => t1.DepartmentId == departmentId)
            .Select((t1, t2) => new QueryDepartmentDetailOutput
            {
                DepartmentId = t1.DepartmentId,
                ParentId = t1.ParentId,
                OrgId = t1.OrgId,
                OrgName = t2.OrgName,
                DepartmentName = t1.DepartmentName,
                DepartmentCode = t1.DepartmentCode,
                Contacts = t1.Contacts,
                Phone = t1.Phone,
                Sort = t1.Sort,
                Remark = t1.Remark,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion
            })
            .SingleOrDefaultAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        // 获取父部门名称
        if (result.ParentId > 0)
        {
            var parentDept = await _repository.Entities
                .Where(wh => wh.DepartmentId == result.ParentId)
                .Select(sl => sl.DepartmentName)
                .FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(parentDept))
            {
                result.ParentName = parentDept;
            }
        }

        return result;
    }

    /// <summary>
    /// 添加部门
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加部门", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Department.Add)]
    public async Task AddDepartment(AddDepartmentInput input)
    {
        // 检查组织是否存在
        if (!await _repository.Queryable<OrganizationModel>()
            .AnyAsync(a => a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("组织不存在！");
        }

        // 检查父部门是否存在
        if (input.ParentId > 0)
        {
            if (!await _repository.AnyAsync(a => a.DepartmentId == input.ParentId))
            {
                throw new UserFriendlyException("父部门不存在！");
            }
        }

        if (await _repository.AnyAsync(a => a.DepartmentName == input.DepartmentName && a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("部门名称重复！");
        }

        if (await _repository.AnyAsync(a => a.DepartmentCode == input.DepartmentCode))
        {
            throw new UserFriendlyException("部门编码重复！");
        }

        var departmentModel = new DepartmentModel
        {
            ParentId = input.ParentId ?? 0,
            OrgId = input.OrgId,
            DepartmentName = input.DepartmentName,
            DepartmentCode = input.DepartmentCode,
            Contacts = input.Contacts,
            Phone = input.Phone,
            Sort = input.Sort,
            Remark = input.Remark
        };

        await _repository.InsertAsync(departmentModel);
    }

    /// <summary>
    /// 编辑部门
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑部门", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Department.Edit)]
    public async Task EditDepartment(EditDepartmentInput input)
    {
        // 检查组织是否存在
        if (!await _repository.Queryable<OrganizationModel>()
            .AnyAsync(a => a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("组织不存在！");
        }

        // 检查父部门是否存在
        if (input.ParentId > 0)
        {
            if (!await _repository.AnyAsync(a => a.DepartmentId == input.ParentId))
            {
                throw new UserFriendlyException("父部门不存在！");
            }

            // 不能将自己设为父部门
            if (input.ParentId == input.DepartmentId)
            {
                throw new UserFriendlyException("不能将自己设为父部门！");
            }

            // 检查是否循环引用
            if (await IsCircularReference(input.DepartmentId, input.ParentId.Value))
            {
                throw new UserFriendlyException("存在循环引用！");
            }
        }

        var departmentModel = await _repository.SingleOrDefaultAsync(input.DepartmentId);
        if (departmentModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.DepartmentName == input.DepartmentName && a.OrgId == input.OrgId && a.DepartmentId != input.DepartmentId))
        {
            throw new UserFriendlyException("部门名称重复！");
        }

        if (await _repository.AnyAsync(a => a.DepartmentCode == input.DepartmentCode && a.DepartmentId != input.DepartmentId))
        {
            throw new UserFriendlyException("部门编码重复！");
        }

        departmentModel.ParentId = input.ParentId ?? 0;
        departmentModel.OrgId = input.OrgId;
        departmentModel.DepartmentName = input.DepartmentName;
        departmentModel.DepartmentCode = input.DepartmentCode;
        departmentModel.Contacts = input.Contacts;
        departmentModel.Phone = input.Phone;
        departmentModel.Sort = input.Sort;
        departmentModel.Remark = input.Remark;
        departmentModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(departmentModel);
    }

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除部门", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Department.Delete)]
    public async Task DeleteDepartment(DepartmentIdInput input)
    {
        // 检查是否有子部门
        if (await _repository.AnyAsync(a => a.ParentId == input.DepartmentId))
        {
            throw new UserFriendlyException("部门存在子部门，无法删除！");
        }

        // 检查是否有员工关联
        if (await _repository.Queryable<EmployeeOrgModel>()
            .AnyAsync(a => a.DepartmentId == input.DepartmentId))
        {
            throw new UserFriendlyException("部门存在员工关联，无法删除！");
        }

        var departmentModel = await _repository.SingleOrDefaultAsync(input.DepartmentId);
        if (departmentModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(departmentModel);
    }

    /// <summary>
    /// 构建部门树形结构
    /// </summary>
    /// <param name="allDepartments">所有部门</param>
    /// <param name="parentId">父级Id</param>
    /// <returns></returns>
    private List<QueryDepartmentTreeOutput> BuildDepartmentTree(List<QueryDepartmentTreeOutput> allDepartments, long parentId)
    {
        var children = allDepartments.Where(d => d.ParentId == parentId).ToList();
        
        foreach (var child in children)
        {
            child.Children = BuildDepartmentTree(allDepartments, child.DepartmentId);
        }

        return children;
    }

    /// <summary>
    /// 检查是否循环引用
    /// </summary>
    /// <param name="currentId">当前部门Id</param>
    /// <param name="targetParentId">目标父部门Id</param>
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
                .Where(wh => wh.DepartmentId == parentId)
                .Select(sl => sl.ParentId)
                .FirstOrDefaultAsync();

            // If parent not found or no parent, break the loop
            if (parent == 0)
            {
                break;
            }

            parentId = parent;
        }

        return false;
    }
}
