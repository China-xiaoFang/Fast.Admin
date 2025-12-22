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
using Fast.Admin.Service.Employee.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Employee;

/// <summary>
/// <see cref="EmployeeService"/> 职员服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "employee")]
public class EmployeeService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<EmployeeModel> _repository;

    public EmployeeService(IUser user, ISqlSugarRepository<EmployeeModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 职员选择器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("职员选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> EmployeeSelector()
    {
        var data = await _repository.Entities
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .OrderBy(ob => ob.EmployeeName)
            .Select(sl => new { sl.EmployeeId, sl.EmployeeName, sl.EmployeeNo, sl.Mobile })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
        {
            Value = sl.EmployeeId,
            Label = sl.EmployeeName,
            Data = new { sl.EmployeeNo, sl.Mobile }
        }).ToList();
    }

    /// <summary>
    /// 获取职员分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取职员分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Employee.Paged)]
    public async Task<PagedResult<QueryEmployeePagedOutput>> QueryEmployeePaged(QueryEmployeePagedInput input)
    {
        return await _repository.Entities
            .WhereIF(!string.IsNullOrEmpty(input.EmployeeName), wh => wh.EmployeeName.Contains(input.EmployeeName))
            .WhereIF(!string.IsNullOrEmpty(input.EmployeeNo), wh => wh.EmployeeNo.Contains(input.EmployeeNo))
            .WhereIF(!string.IsNullOrEmpty(input.Mobile), wh => wh.Mobile.Contains(input.Mobile))
            .WhereIF(input.Sex != null, wh => wh.Sex == input.Sex)
            .WhereIF(input.Status != null, wh => wh.Status == input.Status)
            .OrderByDescending(ob => ob.CreatedTime)
            .ToPagedListAsync(input, sl => new QueryEmployeePagedOutput
            {
                EmployeeId = sl.EmployeeId,
                EmployeeName = sl.EmployeeName,
                EmployeeNo = sl.EmployeeNo,
                Mobile = sl.Mobile,
                Email = sl.Email,
                Sex = sl.Sex,
                Avatar = sl.Avatar,
                Status = sl.Status,
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
    /// 获取职员详情
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取职员详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Employee.Detail)]
    public async Task<QueryEmployeeDetailOutput> QueryEmployeeDetail([Required(ErrorMessage = "职员Id不能为空")] long? employeeId)
    {
        var result = await _repository.Entities
            .Where(wh => wh.EmployeeId == employeeId)
            .Select(sl => new QueryEmployeeDetailOutput
            {
                EmployeeId = sl.EmployeeId,
                EmployeeName = sl.EmployeeName,
                EmployeeNo = sl.EmployeeNo,
                Mobile = sl.Mobile,
                Email = sl.Email,
                Sex = sl.Sex,
                Avatar = sl.Avatar,
                Birthday = sl.Birthday,
                Address = sl.Address,
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

        // 获取职员的部门列表
        result.DepartmentIds = await _repository.Queryable<EmployeeDepartmentModel>()
            .Where(wh => wh.EmployeeId == employeeId)
            .Select(sl => sl.DepartmentId)
            .ToListAsync();

        // 获取职员的角色列表
        result.RoleIds = await _repository.Queryable<EmployeeRoleModel>()
            .Where(wh => wh.EmployeeId == employeeId)
            .Select(sl => sl.RoleId)
            .ToListAsync();

        // 获取职员的职位
        var employeePosition = await _repository.Queryable<EmployeePositionModel>()
            .Where(wh => wh.EmployeeId == employeeId)
            .FirstAsync();
        
        if (employeePosition != null)
        {
            result.PositionId = employeePosition.PositionId;
            result.JobLevelId = employeePosition.JobLevelId;
        }

        return result;
    }

    /// <summary>
    /// 添加职员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加职员", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Employee.Add)]
    public async Task AddEmployee(AddEmployeeInput input)
    {
        if (await _repository.AnyAsync(a => a.EmployeeNo == input.EmployeeNo))
        {
            throw new UserFriendlyException("职员工号重复！");
        }

        if (await _repository.AnyAsync(a => a.Mobile == input.Mobile))
        {
            throw new UserFriendlyException("手机号已存在！");
        }

        var employeeModel = new EmployeeModel
        {
            EmployeeName = input.EmployeeName,
            EmployeeNo = input.EmployeeNo,
            Mobile = input.Mobile,
            Email = input.Email,
            Sex = input.Sex,
            Avatar = input.Avatar,
            Birthday = input.Birthday,
            Address = input.Address,
            Status = CommonStatusEnum.Enable,
            Remark = input.Remark
        };

        await _repository.InsertAsync(employeeModel);
    }

    /// <summary>
    /// 编辑职员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑职员", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.Edit)]
    public async Task EditEmployee(EditEmployeeInput input)
    {
        if (await _repository.AnyAsync(a => a.EmployeeNo == input.EmployeeNo && a.EmployeeId != input.EmployeeId))
        {
            throw new UserFriendlyException("职员工号重复！");
        }

        if (await _repository.AnyAsync(a => a.Mobile == input.Mobile && a.EmployeeId != input.EmployeeId))
        {
            throw new UserFriendlyException("手机号已存在！");
        }

        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        employeeModel.EmployeeName = input.EmployeeName;
        employeeModel.EmployeeNo = input.EmployeeNo;
        employeeModel.Mobile = input.Mobile;
        employeeModel.Email = input.Email;
        employeeModel.Sex = input.Sex;
        employeeModel.Avatar = input.Avatar;
        employeeModel.Birthday = input.Birthday;
        employeeModel.Address = input.Address;
        employeeModel.Status = input.Status;
        employeeModel.Remark = input.Remark;
        employeeModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(employeeModel);
    }

    /// <summary>
    /// 删除职员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除职员", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Employee.Delete)]
    public async Task DeleteEmployee(EmployeeIdInput input)
    {
        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.Ado.UseTranAsync(async () =>
        {
            // 删除职员部门关联
            await _repository.Deleteable<EmployeeDepartmentModel>()
                .Where(wh => wh.EmployeeId == input.EmployeeId)
                .ExecuteCommandAsync();

            // 删除职员角色关联
            await _repository.Deleteable<EmployeeRoleModel>()
                .Where(wh => wh.EmployeeId == input.EmployeeId)
                .ExecuteCommandAsync();

            // 删除职员职位关联
            await _repository.Deleteable<EmployeePositionModel>()
                .Where(wh => wh.EmployeeId == input.EmployeeId)
                .ExecuteCommandAsync();

            // 删除职员
            await _repository.DeleteAsync(employeeModel);
        }, ex => throw ex);
    }

    /// <summary>
    /// 设置职员组织
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("设置职员组织", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.SetOrg)]
    public async Task SetEmployeeOrg(SetEmployeeOrgInput input)
    {
        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("职员不存在！");
        }

        // 验证部门是否都存在
        var departmentIds = input.DepartmentIds ?? new List<long>();
        if (departmentIds.Any())
        {
            var existDepartmentIds = await _repository.Queryable<DepartmentModel>()
                .Where(wh => departmentIds.Contains(wh.DepartmentId))
                .Select(sl => sl.DepartmentId)
                .ToListAsync();

            if (existDepartmentIds.Count != departmentIds.Count)
            {
                throw new UserFriendlyException("部分部门不存在！");
            }
        }

        // 验证职位是否存在
        if (input.PositionId != null)
        {
            if (!await _repository.Queryable<PositionModel>()
                .AnyAsync(a => a.PositionId == input.PositionId))
            {
                throw new UserFriendlyException("职位不存在！");
            }
        }

        // 验证职级是否存在
        if (input.JobLevelId != null)
        {
            if (!await _repository.Queryable<JobLevelModel>()
                .AnyAsync(a => a.JobLevelId == input.JobLevelId))
            {
                throw new UserFriendlyException("职级不存在！");
            }
        }

        await _repository.Ado.UseTranAsync(async () =>
        {
            // 删除旧的部门关联
            await _repository.Deleteable<EmployeeDepartmentModel>()
                .Where(wh => wh.EmployeeId == input.EmployeeId)
                .ExecuteCommandAsync();

            // 添加新的部门关联
            if (departmentIds.Any())
            {
                var employeeDepartmentList = departmentIds.Select(departmentId => new EmployeeDepartmentModel
                {
                    EmployeeId = input.EmployeeId,
                    DepartmentId = departmentId
                }).ToList();

                await _repository.Insertable(employeeDepartmentList).ExecuteCommandAsync();
            }

            // 处理职位和职级
            var employeePosition = await _repository.Queryable<EmployeePositionModel>()
                .Where(wh => wh.EmployeeId == input.EmployeeId)
                .FirstAsync();

            if (input.PositionId != null || input.JobLevelId != null)
            {
                if (employeePosition == null)
                {
                    // 新增
                    employeePosition = new EmployeePositionModel
                    {
                        EmployeeId = input.EmployeeId,
                        PositionId = input.PositionId ?? 0,
                        JobLevelId = input.JobLevelId ?? 0
                    };
                    await _repository.Insertable(employeePosition).ExecuteCommandAsync();
                }
                else
                {
                    // 更新
                    employeePosition.PositionId = input.PositionId ?? 0;
                    employeePosition.JobLevelId = input.JobLevelId ?? 0;
                    await _repository.Updateable(employeePosition).ExecuteCommandAsync();
                }
            }
            else if (employeePosition != null)
            {
                // 删除
                await _repository.Deleteable(employeePosition).ExecuteCommandAsync();
            }
        }, ex => throw ex);
    }

    /// <summary>
    /// 设置职员角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("设置职员角色", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.SetRole)]
    public async Task SetEmployeeRole(SetEmployeeRoleInput input)
    {
        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("职员不存在！");
        }

        // 验证角色是否都存在
        var roleIds = input.RoleIds ?? new List<long>();
        if (roleIds.Any())
        {
            var existRoleIds = await _repository.Queryable<RoleModel>()
                .Where(wh => roleIds.Contains(wh.RoleId))
                .Select(sl => sl.RoleId)
                .ToListAsync();

            if (existRoleIds.Count != roleIds.Count)
            {
                throw new UserFriendlyException("部分角色不存在！");
            }
        }

        await _repository.Ado.UseTranAsync(async () =>
        {
            // 删除旧的角色关联
            await _repository.Deleteable<EmployeeRoleModel>()
                .Where(wh => wh.EmployeeId == input.EmployeeId)
                .ExecuteCommandAsync();

            // 添加新的角色关联
            if (roleIds.Any())
            {
                var employeeRoleList = roleIds.Select(roleId => new EmployeeRoleModel
                {
                    EmployeeId = input.EmployeeId,
                    RoleId = roleId
                }).ToList();

                await _repository.Insertable(employeeRoleList).ExecuteCommandAsync();
            }
        }, ex => throw ex);
    }
}
