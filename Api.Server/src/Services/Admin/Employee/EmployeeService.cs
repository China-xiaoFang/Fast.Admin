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

using System.Text.RegularExpressions;
using Fast.Admin.Entity;
using Fast.Admin.Enum;
using Fast.Admin.Service.Employee.Dto;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace Fast.Admin.Service.Employee;

/// <summary>
/// <see cref="EmployeeService"/> 职员服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "employee")]
public class EmployeeService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<EmployeeModel> _repository;
    private readonly ISqlSugarClient _centerRepository;

    public EmployeeService(IUser user, ISqlSugarRepository<EmployeeModel> repository, ISqlSugarClient centerRepository)
    {
        _user = user;
        _repository = repository;
        _centerRepository = centerRepository;
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
        var result = await _repository.Entities
            .LeftJoin<EmployeeOrgModel>((t1, t2) => t1.EmployeeId == t2.EmployeeId && t2.IsPrimary == YesOrNotEnum.Y)
            .WhereIF(input.Status != null, t1 => t1.Status == input.Status)
            .WhereIF(input.Sex != null, t1 => t1.Sex == input.Sex)
            .WhereIF(input.Nation != null, t1 => t1.Nation == input.Nation)
            .WhereIF(!string.IsNullOrEmpty(input.NativePlace), t1 => t1.NativePlace.Contains(input.NativePlace))
            .WhereIF(input.EducationLevel != null, t1 => t1.EducationLevel == input.EducationLevel)
            .WhereIF(input.PoliticalStatus != null, t1 => t1.PoliticalStatus == input.PoliticalStatus)
            .WhereIF(!string.IsNullOrEmpty(input.GraduationCollege), t1 => t1.GraduationCollege.Contains(input.GraduationCollege))
            .WhereIF(input.AcademicQualifications != null, t1 => t1.AcademicQualifications == input.AcademicQualifications)
            .WhereIF(input.AcademicSystem != null, t1 => t1.AcademicSystem == input.AcademicSystem)
            .WhereIF(input.Degree != null, t1 => t1.Degree == input.Degree)
            .OrderByDescending(t1 => t1.CreatedTime)
            .Select((t1, t2) => new QueryEmployeePagedOutput
            {
                EmployeeId = t1.EmployeeId,
                UserId = t1.UserId,
                EmployeeNo = t1.EmployeeNo,
                EmployeeName = t1.EmployeeName,
                Mobile = t1.Mobile,
                Status = t1.Status,
                Email = t1.Email,
                Sex = t1.Sex,
                IdPhoto = t1.IdPhoto,
                EntryDate = t1.EntryDate,
                ResignDate = t1.ResignDate,
                Nation = t1.Nation,
                NativePlace = t1.NativePlace,
                Birthday = t1.Birthday,
                EducationLevel = t1.EducationLevel,
                PoliticalStatus = t1.PoliticalStatus,
                GraduationCollege = t1.GraduationCollege,
                AcademicQualifications = t1.AcademicQualifications,
                AcademicSystem = t1.AcademicSystem,
                Degree = t1.Degree,
                Remark = t1.Remark,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion,
                OrgId = t2.OrgId,
                OrgName = t2.OrgName,
                OrgNames = t2.OrgNames,
                DepartmentId = t2.DepartmentId,
                DepartmentName = t2.DepartmentName,
                DepartmentNames = t2.DepartmentNames,
                PositionId = t2.PositionId,
                PositionName = t2.PositionName,
                JobLevelId = t2.JobLevelId,
                JobLevelName = t2.JobLevelName,
                IsPrincipal = t2.IsPrincipal
            })
            .ToPagedListAsync(input);

        var userIds = result.Rows.Where(wh => wh.UserId != null)
            .Select(sl => sl.UserId)
            .ToList();

        var userList = await _centerRepository.Queryable<TenantUserModel>()
            .LeftJoin<AccountModel>((t1, t2) => t1.AccountId == t2.AccountId)
            .Where(t1 => userIds.Contains(t1.UserId))
            .Select((t1, t2) => new
            {
                t1.UserId,
                t1.Account,
                t1.Status,
                t2.Mobile,
                t2.Email,
                t2.NickName,
                t2.LastLoginTime
            })
            .ToListAsync();

        foreach (var item in result.Rows)
        {
            if (item.UserId == null)
                continue;
            var userInfo = userList.Single(s => s.UserId == item.UserId);
            item.Account = userInfo.Account;
            item.AccountStatus = userInfo.Status;
            item.AccountMobile = userInfo.Mobile;
            item.AccountEmail = userInfo.Email;
            item.AccountNickName = userInfo.NickName;
            item.LastLoginTime = userInfo.LastLoginTime;
        }

        return result;
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
        var result = await _repository.Entities.Where(wh => wh.EmployeeId == employeeId)
            .Select(sl => new QueryEmployeeDetailOutput
            {
                EmployeeId = sl.EmployeeId,
                UserId = sl.UserId,
                EmployeeNo = sl.EmployeeNo,
                EmployeeName = sl.EmployeeName,
                Mobile = sl.Mobile,
                Status = sl.Status,
                Email = sl.Email,
                Sex = sl.Sex,
                IdPhoto = sl.IdPhoto,
                FirstWorkDate = sl.FirstWorkDate,
                EntryDate = sl.EntryDate,
                ResignDate = sl.ResignDate,
                ResignReason = sl.ResignReason,
                Nation = sl.Nation,
                NativePlace = sl.NativePlace,
                FamilyAddress = sl.FamilyAddress,
                MailingAddress = sl.MailingAddress,
                Birthday = sl.Birthday,
                IdType = sl.IdType,
                IdNumber = sl.IdNumber,
                EducationLevel = sl.EducationLevel,
                PoliticalStatus = sl.PoliticalStatus,
                GraduationCollege = sl.GraduationCollege,
                AcademicQualifications = sl.AcademicQualifications,
                AcademicSystem = sl.AcademicSystem,
                Degree = sl.Degree,
                FamilyPhone = sl.FamilyPhone,
                OfficePhone = sl.OfficePhone,
                EmergencyContact = sl.EmergencyContact,
                EmergencyPhone = sl.EmergencyPhone,
                EmergencyAddress = sl.EmergencyAddress,
                Remark = sl.Remark,
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

        result.OrgList = await _repository.Queryable<EmployeeOrgModel>()
            .Where(wh => wh.EmployeeId == employeeId)
            .ToListAsync();

        result.RoleList = await _repository.Queryable<EmployeeRoleModel>()
            .Where(wh => wh.EmployeeId == employeeId)
            .ToListAsync();

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

        JobLevelModel jobLevelModel = null;
        if (input.JobLevelId != null)
        {
            jobLevelModel = await _repository.Queryable<JobLevelModel>()
                .SingleAsync(s => s.JobLevelId == input.JobLevelId);
        }

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
            FirstWorkDate = input.FirstWorkDate,
            EntryDate = input.EntryDate,
            ResignDate = null,
            ResignReason = null,
            Nation = input.Nation,
            NativePlace = input.NativePlace,
            FamilyAddress = input.FamilyAddress,
            MailingAddress = input.MailingAddress,
            Birthday = input.Birthday,
            IdType = input.IdType,
            IdNumber = input.IdNumber,
            EducationLevel = input.EducationLevel,
            PoliticalStatus = input.PoliticalStatus,
            GraduationCollege = input.GraduationCollege,
            AcademicQualifications = input.AcademicQualifications,
            AcademicSystem = input.AcademicSystem,
            Degree = input.Degree,
            FamilyPhone = input.FamilyPhone,
            OfficePhone = input.OfficePhone,
            EmergencyContact = input.EmergencyContact,
            EmergencyPhone = input.EmergencyPhone,
            EmergencyAddress = input.EmergencyAddress,
            Remark = input.Remark
        };

        var employeeOrgModel = new EmployeeOrgModel
        {
            EmployeeId = employeeModel.EmployeeId,
            OrgId = organizationModel.OrgId,
            OrgName = organizationModel.OrgName,
            OrgNames = [..organizationModel.ParentNames, organizationModel.OrgName],
            DepartmentId = departmentModel.DepartmentId,
            DepartmentName = departmentModel.DepartmentName,
            DepartmentNames = [..departmentModel.ParentNames, departmentModel.DepartmentName],
            IsPrimary = YesOrNotEnum.Y,
            PositionId = positionModel.PositionId,
            PositionName = positionModel.PositionName,
            JobLevelId = jobLevelModel?.JobLevelId,
            JobLevelName = jobLevelModel?.JobLevelName,
            IsPrincipal = input.IsPrincipal
        };

        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);

        await _repository.Ado.UseTranAsync(async () =>
        {
            var employeeNo = SerialContext.GenEmployeeNo(_repository, tenantModel.TenantCode);
            employeeModel.EmployeeNo = employeeNo;
            await _repository.InsertAsync(employeeModel);
            await _repository.Insertable(employeeOrgModel)
                .ExecuteCommandAsync();
        }, ex => throw ex);
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
        if (await _repository.AnyAsync(a => a.Mobile == input.Mobile && a.EmployeeId != input.EmployeeId))
        {
            throw new UserFriendlyException("手机号重复！");
        }

        if (input.OrgList?.Count < 1)
        {
            throw new UserFriendlyException("请至少填写一个部门！");
        }

        if (input.OrgList.Count(c => c.IsPrimary == YesOrNotEnum.Y) > 1)
        {
            throw new UserFriendlyException("只能存在一个主部门！");
        }

        if (input.RoleList.Select(sl => sl.RoleId)
                .Distinct()
                .Count()
            != input.RoleList.Count)
        {
            throw new UserFriendlyException("角色重复！");
        }

        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
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

        var jobLevelId = input.OrgList.Where(wh => wh.JobLevelId != null)
            .Select(sl => sl.JobLevelId)
            .Distinct()
            .ToList();
        var jobLevelList = await _repository.Queryable<JobLevelModel>()
            .Where(wh => jobLevelId.Contains(wh.JobLevelId))
            .ToListAsync();
        if (jobLevelList.Count != jobLevelId.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var roleIds = input.RoleList.Select(sl => sl.RoleId)
            .ToList();
        var roleList = await _repository.Queryable<RoleModel>()
            .Where(wh => roleIds.Contains(wh.RoleId))
            .ToListAsync();
        if (roleList.Count != roleIds.Count)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        employeeModel.EmployeeName = input.EmployeeName;
        employeeModel.Mobile = input.Mobile;
        employeeModel.Email = input.Email;
        employeeModel.Sex = input.Sex;
        employeeModel.IdPhoto = input.IdPhoto;
        employeeModel.FirstWorkDate = input.FirstWorkDate;
        employeeModel.EntryDate = input.EntryDate;
        employeeModel.Nation = input.Nation;
        employeeModel.NativePlace = input.NativePlace;
        employeeModel.FamilyAddress = input.FamilyAddress;
        employeeModel.MailingAddress = input.MailingAddress;
        employeeModel.Birthday = input.Birthday;
        employeeModel.IdType = input.IdType;
        employeeModel.IdNumber = input.IdNumber;
        employeeModel.EducationLevel = input.EducationLevel;
        employeeModel.PoliticalStatus = input.PoliticalStatus;
        employeeModel.GraduationCollege = input.GraduationCollege;
        employeeModel.AcademicQualifications = input.AcademicQualifications;
        employeeModel.AcademicSystem = input.AcademicSystem;
        employeeModel.Degree = input.Degree;
        employeeModel.FamilyPhone = input.FamilyPhone;
        employeeModel.OfficePhone = input.OfficePhone;
        employeeModel.EmergencyContact = input.EmergencyContact;
        employeeModel.EmergencyPhone = input.EmergencyPhone;
        employeeModel.EmergencyAddress = input.EmergencyAddress;
        employeeModel.Remark = input.Remark;

        var employeeOrgList = new List<EmployeeOrgModel>();
        foreach (var item in input.OrgList)
        {
            var organizationModel = organizationList.Single(s => s.OrgId == item.OrgId);
            var departmentModel = departmentList.Single(s => s.DepartmentId == item.DepartmentId);
            var positionModel = positionList.Single(s => s.PositionId == item.PositionId);
            JobLevelModel jobLevelModel = null;
            if (item.JobLevelId != null)
            {
                jobLevelModel = jobLevelList.Single(s => s.JobLevelId == item.JobLevelId);
            }

            employeeOrgList.Add(new EmployeeOrgModel
            {
                EmployeeId = employeeModel.EmployeeId,
                OrgId = organizationModel.OrgId,
                OrgName = organizationModel.OrgName,
                OrgNames = [.. organizationModel.ParentNames, organizationModel.OrgName],
                DepartmentId = departmentModel.DepartmentId,
                DepartmentName = departmentModel.DepartmentName,
                DepartmentNames = [.. departmentModel.ParentNames, departmentModel.DepartmentName],
                IsPrimary = YesOrNotEnum.Y,
                PositionId = positionModel.PositionId,
                PositionName = positionModel.PositionName,
                JobLevelId = jobLevelModel?.JobLevelId,
                JobLevelName = jobLevelModel?.JobLevelName,
                IsPrincipal = item.IsPrincipal
            });
        }

        var employeeRoleList = new List<EmployeeRoleModel>();
        foreach (var item in input.RoleList)
        {
            var roleModel = roleList.Single(s => s.RoleId == item.RoleId);
            employeeRoleList.Add(new EmployeeRoleModel
            {
                EmployeeId = employeeModel.EmployeeId, RoleId = roleModel.RoleId, RoleName = roleModel.RoleName
            });
        }

        await _repository.Ado.UseTranAsync(async () =>
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
            var principalDepartmentIds = employeeOrgList.Where(wh => wh.IsPrincipal == YesOrNotEnum.Y)
                .Select(sl => sl.DepartmentId)
                .ToList();
            if (principalDepartmentIds.Any())
            {
                await _repository.Updateable<EmployeeOrgModel>()
                    .SetColumns(_ => new EmployeeOrgModel {IsPrincipal = YesOrNotEnum.N})
                    .Where(wh => principalDepartmentIds.Contains(wh.DepartmentId))
                    .ExecuteCommandAsync();
            }

            await _repository.Insertable(employeeOrgList)
                .ExecuteCommandAsync();
            await _repository.Insertable(employeeRoleList)
                .ExecuteCommandAsync();
            await _repository.UpdateAsync(employeeModel);
        }, ex => throw ex);
    }

    /// <summary>
    /// 职员更改状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("职员更改状态", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.Status)]
    public async Task ChangeStatus(ChangeStatusInput input)
    {
        if (input.Status == EmployeeStatusEnum.Resigned)
        {
            throw new UserFriendlyException("禁止直接修改为离职状态！");
        }

        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        employeeModel.Status = input.Status;
        employeeModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(employeeModel);
    }

    /// <summary>
    /// 职员离职
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("职员离职", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.Edit)]
    public async Task EmployeeResigned(EmployeeResignedInput input)
    {
        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        // 开启事务
        await _repository.Ado.BeginTranAsync();
        await _centerRepository.Ado.BeginTranAsync();
        try
        {
            if (employeeModel.UserId != null)
            {
                var tenantUserModel = await _centerRepository.Queryable<TenantUserModel>()
                    .InSingleAsync(employeeModel.UserId);
                if (tenantUserModel == null)
                {
                    throw new UserFriendlyException("数据不存在！");
                }

                tenantUserModel.Status = CommonStatusEnum.Disable;
                await _centerRepository.Updateable(tenantUserModel)
                    .ExecuteCommandAsync();
            }

            employeeModel.Status = EmployeeStatusEnum.Resigned;
            employeeModel.ResignDate = input.ResignDate;
            employeeModel.ResignReason = input.ResignReason;
            employeeModel.RowVersion = input.RowVersion;

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
    }

    /// <summary>
    /// 绑定登录账号
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("绑定登录账号", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.Edit)]
    public async Task BindLoginAccount(BindLoginAccountInput input)
    {
        if (!new Regex(RegexConst.Mobile).IsMatch(input.Mobile))
        {
            throw new UserFriendlyException("手机号码不正确！");
        }

        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (employeeModel.UserId != null)
        {
            throw new UserFriendlyException("已存在登录账号！");
        }

        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);

        var account = $"{tenantModel.TenantCode}_{input.Account}";

        // 判断账号是否存在
        if (await _centerRepository.Queryable<TenantUserModel>()
                .AnyAsync(a => a.Account == account))
        {
            throw new UserFriendlyException("账号重复！");
        }

        var employeeOrgModel = await _repository.Queryable<EmployeeOrgModel>()
            .SingleAsync(s => s.EmployeeId == employeeModel.EmployeeId && s.IsPrimary == YesOrNotEnum.Y);

        employeeModel.UserId = YitIdHelper.NextId();
        if (string.IsNullOrEmpty(employeeModel.Email))
        {
            employeeModel.Email = input.Email;
        }

        employeeModel.RowVersion = input.RowVersion;

        // 开启事务
        await _repository.Ado.BeginTranAsync();
        await _centerRepository.Ado.BeginTranAsync();
        try
        {
            var accountModel = await _centerRepository.Queryable<AccountModel>()
                .Where(wh => wh.Mobile == input.Mobile)
                .SingleAsync();
            if (accountModel == null)
            {
                if (await _centerRepository.Queryable<AccountModel>()
                        .AnyAsync(a => a.Email == input.Email))
                {
                    throw new UserFriendlyException("邮箱已存在账号信息！");
                }

                var accountId = YitIdHelper.NextId();
                accountModel = new AccountModel
                {
                    AccountId = accountId,
                    AccountKey = NumberUtil.IdToCodeByLong(accountId),
                    Mobile = input.Mobile,
                    Email = input.Email,
                    Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.Password),
                    Status = CommonStatusEnum.Enable,
                    NickName = employeeModel.EmployeeName,
                    Avatar = employeeModel.IdPhoto,
                    Sex = GenderEnum.Unknown
                };
                await _centerRepository.Insertable(accountModel)
                    .ExecuteCommandAsync();

                #region PasswordRecordModel

                // 初始化密码记录表
                await _centerRepository.Insertable(new List<PasswordRecordModel>
                    {
                        new()
                        {
                            AccountId = accountModel.AccountId,
                            OperationType = PasswordOperationTypeEnum.Create,
                            Type = PasswordTypeEnum.SHA1,
                            Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.Password)
                                .ToUpper()
                        }
                    })
                    .ExecuteCommandAsync();

                #endregion
            }

            var tenantUserModel = new TenantUserModel
            {
                UserId = employeeModel.UserId.Value,
                UserKey = NumberUtil.IdToCodeByLong(employeeModel.UserId.Value),
                AccountId = accountModel.AccountId,
                Account = account,
                EmployeeNo = employeeModel.EmployeeNo,
                EmployeeName = employeeModel.EmployeeName,
                IdPhoto = employeeModel.IdPhoto,
                DepartmentId = employeeOrgModel?.DepartmentId,
                DepartmentName = employeeOrgModel?.DepartmentName,
                UserType = UserTypeEnum.None,
                Status = CommonStatusEnum.Enable
            };
            await _centerRepository.Updateable(tenantUserModel)
                .ExecuteCommandAsync();

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
    }

    /// <summary>
    /// 更改登录状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("更改登录状态", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.Status)]
    public async Task ChangeLoginStatus(EmployeeIdInput input)
    {
        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (employeeModel.UserId == null)
        {
            throw new UserFriendlyException("未绑定登录账号！");
        }

        if (employeeModel.Status == EmployeeStatusEnum.Resigned)
        {
            throw new UserFriendlyException("禁止操作已离职的职员！");
        }

        var tenantUserModel = await _centerRepository.Queryable<TenantUserModel>()
            .InSingleAsync(employeeModel.UserId);
        if (tenantUserModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        tenantUserModel.Status = tenantUserModel.Status switch
        {
            CommonStatusEnum.Enable => CommonStatusEnum.Disable,
            CommonStatusEnum.Disable => CommonStatusEnum.Enable,
            _ => tenantUserModel.Status
        };

        await _centerRepository.Updateable(tenantUserModel)
            .ExecuteCommandAsync();

        if (tenantUserModel.Status == CommonStatusEnum.Disable)
        {
            // 强制下线在线用户
            var _hubContext = FastContext.HttpContext.RequestServices.GetService<IHubContext<ChatHub, IChatClient>>();

            var connectionId = await _centerRepository.Queryable<TenantOnlineUserModel>()
                .Where(wh => wh.IsOnline)
                .Where(wh => wh.UserId == tenantUserModel.UserId)
                .Select(sl => sl.ConnectionId)
                .SingleAsync();

            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Clients(connectionId)
                    .ForceOffline(new ForceOfflineOutput
                    {
                        IsAdmin = _user.IsSuperAdmin || _user.IsAdmin,
                        NickName = _user.NickName,
                        EmployeeNo = _user.EmployeeNo,
                        OfflineTime = DateTime.Now,
                        Message = "账号已被禁用"
                    });
            }
        }
    }

    /// <summary>
    /// 职员授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("职员授权", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Employee.Edit)]
    public async Task EmployeeAuth(EmployeeAuthInput input)
    {
        var employeeModel = await _repository.SingleOrDefaultAsync(input.EmployeeId);
        if (employeeModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        // 验证菜单是否都存在
        var menuIds = input.MenuIds ?? [];
        if (menuIds.Any())
        {
            if (await _centerRepository.Queryable<MenuModel>()
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
            var existButtonIds = await _centerRepository.Queryable<ButtonModel>()
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
            await _repository.Deleteable<EmployeeMenuModel>()
                .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
                .ExecuteCommandAsync();

            // 添加新的菜单权限
            if (menuIds.Any())
            {
                await _repository.Insertable(menuIds
                        .Select(menuId => new EmployeeMenuModel {EmployeeId = employeeModel.EmployeeId, MenuId = menuId})
                        .ToList())
                    .ExecuteCommandAsync();
            }

            // 删除旧的按钮权限
            await _repository.Deleteable<EmployeeButtonModel>()
                .Where(wh => wh.EmployeeId == employeeModel.EmployeeId)
                .ExecuteCommandAsync();

            // 添加新的按钮权限
            if (buttonIds.Any())
            {
                await _repository.Insertable(buttonIds
                        .Select(buttonId => new EmployeeButtonModel {EmployeeId = employeeModel.EmployeeId, ButtonId = buttonId})
                        .ToList())
                    .ExecuteCommandAsync();
            }
        }, ex => throw ex);
    }
}