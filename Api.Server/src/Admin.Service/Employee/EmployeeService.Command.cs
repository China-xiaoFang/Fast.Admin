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
using Fast.AdminLog.Domain.Enum;
using Fast.Center.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yitter.IdGenerator;

namespace Fast.Admin.Service.Employee;

public partial class EmployeeService
{
    /// <summary>
    /// 添加职员
    /// </summary>
    [HttpPost]
    [ApiInfo("添加职员", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Employee.Add)]
    public async Task AddEmployee(AddEmployeeInput input)
    {
        if (await _repository.AnyAsync(a => a.Mobile == input.Mobile))
        {
            throw new UserFriendlyException("手机号重复！");
        }

        var organizationModel = await _repository.Queryable<OrganizationModel>()
            .SingleAsync(s => s.OrgId == input.OrgId);
        if (organizationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var departmentModel = await _repository.Queryable<DepartmentModel>()
            .SingleAsync(s => s.DepartmentId == input.DepartmentId);
        if (departmentModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var positionModel = await _repository.Queryable<PositionModel>()
            .SingleAsync(s => s.PositionId == input.PositionId);
        if (positionModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var jobLevelModel = await _repository.Queryable<JobLevelModel>()
            .SingleAsync(s => s.JobLevelId == input.JobLevelId);
        if (jobLevelModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var roleIds = (input.RoleList ?? []).Select(sl => sl.RoleId)
            .Distinct()
            .ToList();
        var roleList = await _repository.Queryable<RoleModel>()
            .Where(wh => roleIds.Contains(wh.RoleId))
            .ToListAsync();
        if (roleList.Count != roleIds.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await ValidateRoleAssignment(roleIds);

        var employeeModel = new EmployeeModel
        {
            EmployeeId = YitIdHelper.NextId(),
            EmployeeName = input.EmployeeName,
            Mobile = input.Mobile,
            // 新增默认正式员工
            Status = EmployeeStatusEnum.Formal,
            Email = input.Email,
            Sex = input.Sex,
            IdPhoto = input.IdPhoto,
            EntryDate = input.EntryDate,
            ResignDate = null,
            ResignReason = null,
            Remark = input.Remark
        };

        var employeeOrgModel = new EmployeeOrgModel
        {
            EmployeeId = employeeModel.EmployeeId,
            OrgId = organizationModel.OrgId,
            OrgName = organizationModel.OrgName,
            OrgNames = [.. organizationModel.ParentNames, organizationModel.OrgName],
            DepartmentId = departmentModel.DepartmentId,
            DepartmentName = departmentModel.DepartmentName,
            DepartmentNames = [.. departmentModel.ParentNames, departmentModel.DepartmentName],
            IsPrimary = true,
            PositionId = positionModel.PositionId,
            PositionName = positionModel.PositionName,
            JobLevelId = jobLevelModel.JobLevelId,
            JobLevelName = jobLevelModel.JobLevelName,
            IsPrincipal = input.IsPrincipal
        };

        var employeeRoleList = new List<EmployeeRoleModel>();
        foreach (var item in input.RoleList)
        {
            var roleModel = roleList.Single(s => s.RoleId == item.RoleId);
            employeeRoleList.Add(new EmployeeRoleModel
            {
                EmployeeId = employeeModel.EmployeeId, RoleId = roleModel.RoleId, RoleName = roleModel.RoleName
            });
        }

        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);

        await _repository.Ado.UseTranAsync(async () =>
        {
            var employeeNo = SerialContext.GenEmployeeNo(_repository, tenantModel.TenantCode);
            employeeModel.EmployeeNo = employeeNo;
            await _repository.InsertAsync(employeeModel);

            // 如果当前职员是负责人，则清除该部门原有负责人
            if (employeeOrgModel.IsPrincipal)
            {
                await _repository.Updateable<EmployeeOrgModel>()
                    .SetColumns(_ => new EmployeeOrgModel {IsPrincipal = false})
                    .Where(wh => wh.DepartmentId == employeeOrgModel.DepartmentId)
                    .ExecuteCommandAsync();
            }

            await _repository.Insertable(employeeOrgModel)
                .ExecuteCommandAsync();

            // 删除旧的角色数据
            await _repository.Deleteable<EmployeeRoleModel>()
                .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
                .ExecuteCommandAsync();
            await _repository.Insertable(employeeRoleList)
                .ExecuteCommandAsync();
        }, ex => throw ex);

        // 操作日志
        await LogContext.OperateLog(new OperateLogDto
        {
            Title = "添加职员",
            OperateType = OperateLogTypeEnum.Organization,
            BizId = employeeModel.EmployeeId,
            BizNo = employeeModel.EmployeeNo,
            Description =
                $"职员名称：{employeeModel.EmployeeName}，职员手机：{employeeModel.Mobile}，职员邮箱：{employeeModel.Email}，职员部门：{employeeOrgModel.DepartmentName}"
        });
    }

    /// <summary>
    /// 编辑本职员
    /// </summary>
    [HttpPost]
    [ApiInfo("编辑本职员", HttpRequestActionEnum.Edit)]
    public async Task EditSelfEmployee(EditEmployeeInput input)
    {
        if (await _repository.AnyAsync(a => a.Mobile == input.Mobile && a.EmployeeId != input.EmployeeId))
        {
            throw new UserFriendlyException("手机号重复！");
        }

        var employeeModel = await _repository.SingleOrDefaultAsync(_user.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        employeeModel.EmployeeName = input.EmployeeName;
        employeeModel.Mobile = input.Mobile;
        employeeModel.Email = input.Email;
        employeeModel.Sex = input.Sex;
        employeeModel.IdPhoto = input.IdPhoto;

        // 开启事务
        await _repository.Ado.BeginTranAsync();
        await _centerRepository.Ado.BeginTranAsync();
        try
        {
            var tenantUserModel = await _centerRepository.Queryable<TenantUserModel>()
                .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
                .SingleAsync();
            if (tenantUserModel != null)
            {
                tenantUserModel.EmployeeName = employeeModel.EmployeeName;
                tenantUserModel.IdPhoto = employeeModel.IdPhoto;
                await _centerRepository.Updateable(tenantUserModel)
                    .ExecuteCommandAsync();
            }

            await _repository.UpdateAsync(employeeModel);

            // 提交事务
            await _repository.Ado.CommitTranAsync();
            await _centerRepository.Ado.CommitTranAsync();
        }
        catch
        {
            // 回滚事务
            await _repository.Ado.RollbackTranAsync();
            await _centerRepository.Ado.RollbackTranAsync();
            throw;
        }

        // 操作日志
        await LogContext.OperateLog(new OperateLogDto
        {
            Title = "编辑本职员",
            OperateType = OperateLogTypeEnum.Organization,
            BizId = employeeModel.EmployeeId,
            BizNo = employeeModel.EmployeeNo,
            Description = $"编辑本职员：{employeeModel.EmployeeName}"
        });
    }

    /// <summary>
    /// 编辑职员
    /// </summary>
    [HttpPost]
    [ApiInfo("编辑职员", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.Edit)]
    public async Task EditEmployee(EditEmployeeInput input)
    {
        var employeeModel = await GetEmployeeWithinDataScope(input.EmployeeId);

        if (await _repository.AnyAsync(a => a.Mobile == input.Mobile && a.EmployeeId != input.EmployeeId))
        {
            throw new UserFriendlyException("手机号重复！");
        }

        if (input.OrgList?.Count < 1)
        {
            throw new UserFriendlyException("请至少填写一个部门！");
        }

        if (input.OrgList.Count(c => c.IsPrimary) > 1)
        {
            throw new UserFriendlyException("只能存在一个主部门！");
        }

        if ((input.RoleList ?? []).Select(sl => sl.RoleId)
            .Distinct()
            .Count()
            != (input.RoleList?.Count ?? 0))
        {
            throw new UserFriendlyException("角色重复！");
        }

        if (employeeModel.Status == EmployeeStatusEnum.Resigned)
        {
            throw new UserFriendlyException("禁止修改已离职的职员资料！");
        }

        var orgIds = input.OrgList.Select(sl => sl.OrgId)
            .Distinct()
            .ToList();
        var organizationList = await _repository.Queryable<OrganizationModel>()
            .Where(wh => orgIds.Contains(wh.OrgId))
            .ToListAsync();
        if (organizationList.Count != orgIds.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var departmentIds = input.OrgList.Select(sl => sl.DepartmentId)
            .Distinct()
            .ToList();
        var departmentList = await _repository.Queryable<DepartmentModel>()
            .Where(wh => departmentIds.Contains(wh.DepartmentId))
            .ToListAsync();
        if (departmentList.Count != departmentIds.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var positionId = input.OrgList.Select(sl => sl.PositionId)
            .Distinct()
            .ToList();
        var positionList = await _repository.Queryable<PositionModel>()
            .Where(wh => positionId.Contains(wh.PositionId))
            .ToListAsync();
        if (positionList.Count != positionId.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var jobLevelId = input.OrgList.Select(sl => sl.JobLevelId)
            .Distinct()
            .ToList();
        var jobLevelList = await _repository.Queryable<JobLevelModel>()
            .Where(wh => jobLevelId.Contains(wh.JobLevelId))
            .ToListAsync();
        if (jobLevelList.Count != jobLevelId.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var roleIds = (input.RoleList ?? []).Select(sl => sl.RoleId)
            .ToList();
        var roleList = await _repository.Queryable<RoleModel>()
            .Where(wh => roleIds.Contains(wh.RoleId))
            .ToListAsync();
        if (roleList.Count != roleIds.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var existingRoleIds = await _repository.Queryable<EmployeeRoleModel>()
            .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
            .Select(sl => sl.RoleId)
            .ToListAsync();
        await ValidateRoleAssignment(roleIds, existingRoleIds);

        employeeModel.EmployeeName = input.EmployeeName;
        employeeModel.Mobile = input.Mobile;
        employeeModel.Email = input.Email;
        employeeModel.Sex = input.Sex;
        employeeModel.IdPhoto = input.IdPhoto;
        employeeModel.RowVersion = input.RowVersion;

        var employeeOrgList = new List<EmployeeOrgModel>();
        var employeeRoleList = new List<EmployeeRoleModel>();
        if (employeeModel.EmployeeId != _user.EmployeeId)
        {
            employeeModel.EntryDate = input.EntryDate;
            employeeModel.Remark = input.Remark;

            foreach (var item in input.OrgList)
            {
                var organizationModel = organizationList.Single(s => s.OrgId == item.OrgId);
                var departmentModel = departmentList.Single(s => s.DepartmentId == item.DepartmentId);
                var positionModel = positionList.Single(s => s.PositionId == item.PositionId);
                var jobLevelModel = jobLevelList.Single(s => s.JobLevelId == item.JobLevelId);

                employeeOrgList.Add(new EmployeeOrgModel
                {
                    EmployeeId = employeeModel.EmployeeId,
                    OrgId = organizationModel.OrgId,
                    OrgName = organizationModel.OrgName,
                    OrgNames = [.. organizationModel.ParentNames, organizationModel.OrgName],
                    DepartmentId = departmentModel.DepartmentId,
                    DepartmentName = departmentModel.DepartmentName,
                    DepartmentNames = [.. departmentModel.ParentNames, departmentModel.DepartmentName],
                    IsPrimary = item.IsPrimary,
                    PositionId = positionModel.PositionId,
                    PositionName = positionModel.PositionName,
                    JobLevelId = jobLevelModel.JobLevelId,
                    JobLevelName = jobLevelModel.JobLevelName,
                    IsPrincipal = item.IsPrincipal
                });
            }

            foreach (var item in input.RoleList ?? [])
            {
                var roleModel = roleList.Single(s => s.RoleId == item.RoleId);
                employeeRoleList.Add(new EmployeeRoleModel
                {
                    EmployeeId = employeeModel.EmployeeId, RoleId = roleModel.RoleId, RoleName = roleModel.RoleName
                });
            }
        }

        // 开启事务
        await _repository.Ado.BeginTranAsync();
        await _centerRepository.Ado.BeginTranAsync();
        try
        {
            var tenantUserModel = await _centerRepository.Queryable<TenantUserModel>()
                .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
                .SingleAsync();
            if (tenantUserModel != null)
            {
                tenantUserModel.EmployeeName = employeeModel.EmployeeName;
                tenantUserModel.IdPhoto = employeeModel.IdPhoto;
            }

            if (employeeModel.EmployeeId != _user.EmployeeId)
            {
                // 删除旧的部门数据
                await _repository.Deleteable<EmployeeOrgModel>()
                    .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
                    .ExecuteCommandAsync();
                // 删除旧的角色数据
                await _repository.Deleteable<EmployeeRoleModel>()
                    .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
                    .ExecuteCommandAsync();

                // 处理部门负责人
                var principalDepartmentIds = employeeOrgList.Where(wh => wh.IsPrincipal)
                    .Select(sl => sl.DepartmentId)
                    .ToList();
                if (principalDepartmentIds.Any())
                {
                    await _repository.Updateable<EmployeeOrgModel>()
                        .SetColumns(_ => new EmployeeOrgModel {IsPrincipal = false})
                        .Where(wh => principalDepartmentIds.Contains(wh.DepartmentId))
                        .ExecuteCommandAsync();
                }

                await _repository.Insertable(employeeOrgList)
                    .ExecuteCommandAsync();
                await _repository.Insertable(employeeRoleList)
                    .ExecuteCommandAsync();

                if (tenantUserModel != null)
                {
                    tenantUserModel.DepartmentId = employeeOrgList.Single(s => s.IsPrimary)
                        .DepartmentId;
                    tenantUserModel.DepartmentName = employeeOrgList.Single(s => s.IsPrimary)
                        .DepartmentName;
                }
            }

            if (tenantUserModel != null)
            {
                await _centerRepository.Updateable(tenantUserModel)
                    .ExecuteCommandAsync();
            }

            await _repository.UpdateAsync(employeeModel);

            // 提交事务
            await _repository.Ado.CommitTranAsync();
            await _centerRepository.Ado.CommitTranAsync();
        }
        catch
        {
            // 回滚事务
            await _repository.Ado.RollbackTranAsync();
            await _centerRepository.Ado.RollbackTranAsync();
            throw;
        }

        if (employeeModel.EmployeeId != _user.EmployeeId)
        {
            await _user.RevokeEmployee(_user.TenantNo, employeeModel.EmployeeNo);
        }

        // 操作日志
        await LogContext.OperateLog(new OperateLogDto
        {
            Title = "编辑职员",
            OperateType = OperateLogTypeEnum.Organization,
            BizId = employeeModel.EmployeeId,
            BizNo = employeeModel.EmployeeNo,
            Description = $"编辑职员：{employeeModel.EmployeeName}"
        });
    }
}