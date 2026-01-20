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

using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.Center.Service.Tenant.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace Fast.Center.Service.Tenant;

/// <summary>
/// <see cref="TenantService"/> 租户服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "tenant")]
public class TenantService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<TenantModel> _repository;

    public TenantService(IUser user, ISqlSugarRepository<TenantModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 租户选择器
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("租户选择器", HttpRequestActionEnum.Query)]
    public async Task<PagedResult<ElSelectorOutput<long>>> TenantSelector(PagedInput input)
    {
        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);

        var data = await _repository.Entities
            .WhereIF(tenantModel.TenantType == TenantTypeEnum.Common, wh => wh.TenantId == _user.TenantId)
            .OrderBy(ob => ob.TenantName)
            .Select(sl => new
            {
                sl.TenantId,
                sl.TenantName,
                sl.TenantNo,
                sl.TenantCode,
                sl.ShortName,
                sl.Edition,
                sl.LogoUrl
            })
            .ToPagedListAsync(input);

        return data.ToPagedData(sl => new ElSelectorOutput<long>
        {
            Value = sl.TenantId,
            Label = sl.TenantName,
            Data = new
            {
                sl.TenantNo,
                sl.TenantCode,
                sl.ShortName,
                sl.Edition,
                sl.LogoUrl
            }
        });
    }

    /// <summary>
    /// 获取租户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取租户分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Tenant.Paged)]
    public async Task<PagedResult<QueryTenantPagedOutput>> QueryTenantPaged(QueryTenantPagedInput input)
    {
        return await _repository.Entities.WhereIF(input.Status != null, wh => wh.Status == input.Status)
            .WhereIF(input.Edition != null, wh => (wh.Edition & input.Edition) != 0)
            .WhereIF(!string.IsNullOrWhiteSpace(input.AdminMobile), wh => wh.AdminMobile.Contains(input.AdminMobile))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AdminEmail), wh => wh.AdminEmail.Contains(input.AdminEmail))
            .WhereIF(input.TenantType != null, wh => wh.TenantType == input.TenantType)
            .OrderByDescending(ob => ob.CreatedTime)
            .ToPagedListAsync(input,
                sl => new QueryTenantPagedOutput
                {
                    TenantId = sl.TenantId,
                    TenantNo = sl.TenantNo,
                    TenantCode = sl.TenantCode,
                    Status = sl.Status,
                    TenantName = sl.TenantName,
                    ShortName = sl.ShortName,
                    SpellName = sl.SpellName,
                    Edition = sl.Edition,
                    AdminAccountId = sl.AdminAccountId,
                    AdminName = sl.AdminName,
                    AdminMobile = sl.AdminMobile,
                    AdminEmail = sl.AdminEmail,
                    AdminPhone = sl.AdminPhone,
                    RobotName = sl.RobotName,
                    TenantType = sl.TenantType,
                    LogoUrl = sl.LogoUrl,
                    AllowDeleteData = sl.AllowDeleteData,
                    DepartmentName = sl.DepartmentName,
                    CreatedUserName = sl.CreatedUserName,
                    CreatedTime = sl.CreatedTime,
                    UpdatedUserName = sl.UpdatedUserName,
                    UpdatedTime = sl.UpdatedTime,
                    RowVersion = sl.RowVersion
                });
    }

    /// <summary>
    /// 获取租户详情
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取租户详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Tenant.Detail)]
    public async Task<QueryTenantDetailOutput> QueryTenantDetail([Required(ErrorMessage = "租户Id不能为空")] long? tenantId)
    {
        var result = await _repository.Queryable<TenantModel>()
            .LeftJoin<AccountModel>((t1, t2) => t1.AdminAccountId == t2.AccountId)
            .Where(t1 => t1.TenantId == tenantId)
            .Select((t1, t2) => new QueryTenantDetailOutput
            {
                TenantId = t1.TenantId,
                TenantNo = t1.TenantNo,
                TenantCode = t1.TenantCode,
                Status = t1.Status,
                TenantName = t1.TenantName,
                ShortName = t1.ShortName,
                SpellName = t1.SpellName,
                Edition = t1.Edition,
                AdminAccountId = t1.AdminAccountId,
                AdminName = t1.AdminName,
                AdminMobile = t1.AdminMobile,
                AdminEmail = t1.AdminEmail,
                AdminPhone = t1.AdminPhone,
                RobotName = t1.RobotName,
                TenantType = t1.TenantType,
                LogoUrl = t1.LogoUrl,
                AllowDeleteData = t1.AllowDeleteData,
                DepartmentName = t1.DepartmentName,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion,
                FirstLoginDevice = t2.FirstLoginDevice,
                FirstLoginOS = t2.FirstLoginOS,
                FirstLoginBrowser = t2.FirstLoginBrowser,
                FirstLoginProvince = t2.FirstLoginProvince,
                FirstLoginCity = t2.FirstLoginCity,
                FirstLoginIp = t2.FirstLoginIp,
                FirstLoginTime = t2.FirstLoginTime,
                LastLoginDevice = t2.LastLoginDevice,
                LastLoginOS = t2.LastLoginOS,
                LastLoginBrowser = t2.LastLoginBrowser,
                LastLoginProvince = t2.LastLoginProvince,
                LastLoginCity = t2.LastLoginCity,
                LastLoginIp = t2.LastLoginIp,
                LastLoginTime = t2.LastLoginTime,
                PasswordErrorTime = t2.PasswordErrorTime,
                LockStartTime = t2.LockStartTime,
                LockEndTime = t2.LockEndTime
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }

    /// <summary>
    /// 添加租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加租户", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Tenant.Add)]
    public async Task AddTenant(AddTenantInput input)
    {
        if (await _repository.AnyAsync(a => a.TenantCode == input.TenantCode))
        {
            throw new UserFriendlyException("租户编码重复！");
        }

        if (await _repository.AnyAsync(a => a.TenantName == input.TenantName))
        {
            throw new UserFriendlyException("租户名称重复！");
        }

        var tenantModel = new TenantModel
        {
            TenantId = YitIdHelper.NextId(),
            TenantCode = input.TenantCode,
            TenantName = input.TenantName,
            Status = CommonStatusEnum.Enable,
            ShortName = input.ShortName,
            SpellName = input.SpellName,
            Edition = input.Edition,
            AdminName = input.AdminName,
            AdminMobile = input.AdminMobile,
            AdminEmail = input.AdminEmail,
            AdminPhone = input.AdminPhone,
            RobotName = input.RobotName,
            TenantType = TenantTypeEnum.Common,
            LogoUrl = input.LogoUrl,
            AllowDeleteData = true
        };

        await _repository.Ado.UseTranAsync(async () =>
        {
            tenantModel.TenantNo = SysSerialContext.GenTenantNo(_repository);
            await _repository.InsertAsync(tenantModel);
        }, ex => throw ex);

        // 删除缓存
        await TenantContext.DeleteTenant(tenantModel.TenantNo);
    }

    /// <summary>
    /// 编辑租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑租户", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Tenant.Edit)]
    public async Task EditTenant(EditTenantInput input)
    {
        if (await _repository.AnyAsync(a => a.TenantCode == input.TenantCode && a.TenantId != input.TenantId))
        {
            throw new UserFriendlyException("租户编码重复！");
        }

        if (await _repository.AnyAsync(a => a.TenantName == input.TenantName && a.TenantId != input.TenantId))
        {
            throw new UserFriendlyException("租户名称重复！");
        }

        var tenantModel = await _repository.SingleOrDefaultAsync(input.TenantId);
        if (tenantModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        tenantModel.TenantCode = input.TenantCode;
        tenantModel.Status = input.Status;
        tenantModel.TenantName = input.TenantName;
        tenantModel.ShortName = input.ShortName;
        tenantModel.SpellName = input.SpellName;
        tenantModel.Edition = input.Edition;
        tenantModel.AdminName = input.AdminName;
        tenantModel.AdminEmail = input.AdminEmail;
        tenantModel.AdminPhone = input.AdminPhone;
        tenantModel.LogoUrl = input.LogoUrl;
        tenantModel.RowVersion = input.RowVersion;

        await _repository.Ado.UseTranAsync(async () =>
        {
            // 只有手机号码不同才更换管理员
            if (tenantModel.AdminMobile != input.AdminMobile)
            {
                if (tenantModel.AdminAccountId != 0)
                {
                    var accountModel = await _repository.Queryable<AccountModel>()
                        .Where(wh => wh.Mobile == input.AdminMobile)
                        .SingleAsync();
                    if (accountModel == null)
                    {
                        var accountId = YitIdHelper.NextId();
                        accountModel = new AccountModel
                        {
                            AccountId = accountId,
                            AccountKey = NumberUtil.IdToCodeByLong(accountId),
                            Mobile = input.AdminMobile,
                            Email = input.AdminEmail,
                            Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.Password)
                                .ToUpper(),
                            NickName = input.AdminName,
                            Avatar = tenantModel.LogoUrl,
                            Status = CommonStatusEnum.Enable,
                            Sex = GenderEnum.Unknown
                        };
                        accountModel = await _repository.Insertable(accountModel)
                            .ExecuteReturnEntityAsync();

                        #region PasswordRecordModel

                        // 初始化密码记录表
                        await _repository.Insertable(new List<PasswordRecordModel>
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

                    var tenantUserModel = await _repository.Queryable<TenantUserModel>()
                        .Where(wh => wh.AccountId == tenantModel.AdminAccountId)
                        .SingleAsync();
                    tenantUserModel.AccountId = accountModel.AccountId;
                    await _repository.Updateable(tenantUserModel)
                        .ExecuteCommandAsync();

                    // 回填管理员账号Id
                    tenantModel.AdminAccountId = accountModel.AccountId;
                }

                tenantModel.AdminMobile = input.AdminMobile;
            }

            if (tenantModel.RobotName != input.RobotName)
            {
                var tenantUserModel = await _repository.Queryable<TenantUserModel>()
                    .Where(wh => wh.UserType == UserTypeEnum.Robot)
                    .SingleAsync();
                if (tenantUserModel != null)
                {
                    tenantUserModel.EmployeeName = input.RobotName;
                    await _repository.Updateable(tenantUserModel)
                        .ExecuteCommandAsync();
                }

                tenantModel.RobotName = input.RobotName;
            }

            await _repository.UpdateAsync(tenantModel);
        }, ex => throw ex);

        // 删除缓存
        await TenantContext.DeleteTenant(tenantModel.TenantNo);
    }

    /// <summary>
    /// 租户更改状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("租户更改状态", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Tenant.Status)]
    public async Task ChangeStatus(TenantIdInput input)
    {
        var tenantModel = await _repository.SingleOrDefaultAsync(input.TenantId);
        if (tenantModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (tenantModel.TenantType == TenantTypeEnum.System)
        {
            throw new UserFriendlyException("禁止修改系统租户状态！");
        }

        // 更改状态
        tenantModel.Status = tenantModel.Status switch
        {
            CommonStatusEnum.Enable => CommonStatusEnum.Disable,
            CommonStatusEnum.Disable => CommonStatusEnum.Enable,
            _ => tenantModel.Status
        };
        tenantModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(tenantModel);

        if (tenantModel.Status == CommonStatusEnum.Disable)
        {
            // 强制下线当前租户所有在线用户
            var _hubContext = FastContext.HttpContext.RequestServices.GetService<IHubContext<ChatHub, IChatClient>>();

            var connectionIdList = await _repository.Queryable<TenantOnlineUserModel>()
                .Where(wh => wh.IsOnline)
                .Where(wh => wh.TenantId == tenantModel.TenantId)
                .Select(sl => sl.ConnectionId)
                .ToListAsync();

            await _hubContext.Clients.Clients(connectionIdList)
                .ForceOffline(new ForceOfflineOutput
                {
                    IsAdmin = true,
                    NickName = "系统操作",
                    EmployeeNo = "无",
                    OfflineTime = DateTime.Now,
                    Message = "当前租户已被禁用"
                });
        }

        // 删除缓存
        await TenantContext.DeleteTenant(tenantModel.TenantNo);
    }
}