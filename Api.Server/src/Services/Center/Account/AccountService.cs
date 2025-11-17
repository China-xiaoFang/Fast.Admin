using Fast.Center.Entity;
using Fast.Center.Service.Account.Dto;
using Fast.CenterLog.Entity;
using Fast.CenterLog.Enum;
using Fast.Core.Hubs.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace Fast.Center.Service.Account;

/// <summary>
/// <see cref="AccountService"/> 账号服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "account")]
public class AccountService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<AccountModel> _repository;

    public AccountService(IUser user, ISqlSugarRepository<AccountModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 获取账号分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取账号分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Account.Paged)]
    public async Task<PagedResult<QueryAccountPagedOutput>> QueryAccountPaged(QueryAccountPagedInput input)
    {
        var dateTime = DateTime.Now;

        var queryable = _repository.Queryable<AccountModel>()
            .LeftJoin<TenantModel>((t1, t2) => t1.FirstLoginTenantId == t2.TenantId)
            .LeftJoin<TenantModel>((t1, t2, t3) => t1.LastLoginTenantId == t3.TenantId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Mobile), t1 => t1.Mobile.Contains(input.Mobile))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Email), t1 => t1.Email.Contains(input.Email))
            .WhereIF(input.Status != null, t1 => t1.Status == input.Status)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), t1 => t1.Phone.Contains(input.Phone))
            .WhereIF(input.Sex != null, t1 => t1.Sex == input.Sex)
            .WhereIF(!string.IsNullOrWhiteSpace(input.FirstLoginCity),
                t1 => t1.FirstLoginCity.Contains(input.FirstLoginCity))
            .WhereIF(!string.IsNullOrWhiteSpace(input.FirstLoginIp), t1 => t1.FirstLoginIp.Contains(input.FirstLoginIp))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LastLoginCity),
                t1 => t1.LastLoginCity.Contains(input.LastLoginCity))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LastLoginIp), t1 => t1.LastLoginIp.Contains(input.LastLoginIp))
            .WhereIF(input.IsLock == true, t1 => t1.LockEndTime != null && t1.LockEndTime >= dateTime)
            .WhereIF(input.IsLock == false, t1 => t1.LockEndTime == null || t1.LockEndTime < dateTime);

        return await queryable.OrderBy(t1 => t1.CreatedTime)
            .Select((t1, t2, t3) => new QueryAccountPagedOutput
            {
                AccountId = t1.AccountId,
                Mobile = t1.Mobile,
                Email = t1.Email,
                Status = t1.Status,
                NickName = t1.NickName,
                Avatar = t1.Avatar,
                Sex = t1.Sex,
                Birthday = t1.Birthday,
                FirstLoginTenantName = t2.TenantName,
                FirstLoginDevice = t1.FirstLoginDevice,
                FirstLoginOS = t1.FirstLoginOS,
                FirstLoginBrowser = t1.FirstLoginBrowser,
                FirstLoginProvince = t1.FirstLoginProvince,
                FirstLoginCity = t1.FirstLoginCity,
                FirstLoginIp = t1.FirstLoginIp,
                FirstLoginTime = t1.FirstLoginTime,
                LastLoginTenantName = t3.TenantName,
                LastLoginDevice = t1.LastLoginDevice,
                LastLoginOS = t1.LastLoginOS,
                LastLoginBrowser = t1.LastLoginBrowser,
                LastLoginProvince = t1.LastLoginProvince,
                LastLoginCity = t1.LastLoginCity,
                LastLoginIp = t1.LastLoginIp,
                LastLoginTime = t1.LastLoginTime,
                PasswordErrorTime = t1.PasswordErrorTime,
                LockStartTime = t1.LockStartTime,
                LockEndTime = t1.LockEndTime,
                IsLock = t1.LockEndTime != null && t1.LockEndTime >= dateTime,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取账号详情
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取账号详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Account.Detail)]
    public async Task<QueryAccountDetailOutput> QueryAccountDetail(
        [Required(ErrorMessage = "账号Id不能为空")] long? accountId)
    {
        var result = await _repository.Queryable<AccountModel>()
            .LeftJoin<TenantModel>((t1, t2) => t1.FirstLoginTenantId == t2.TenantId)
            .LeftJoin<TenantModel>((t1, t2, t3) => t1.LastLoginTenantId == t3.TenantId)
            .Where(t1 => t1.AccountId == accountId)
            .Select((t1, t2, t3) => new QueryAccountDetailOutput
            {
                AccountId = t1.AccountId,
                Mobile = t1.Mobile,
                Email = t1.Email,
                Status = t1.Status,
                NickName = t1.NickName,
                Avatar = t1.Avatar,
                Phone = t1.Phone,
                Sex = t1.Sex,
                Birthday = t1.Birthday,
                FirstLoginTenantName = t2.TenantName,
                FirstLoginDevice = t1.FirstLoginDevice,
                FirstLoginOS = t1.FirstLoginOS,
                FirstLoginBrowser = t1.FirstLoginBrowser,
                FirstLoginProvince = t1.FirstLoginProvince,
                FirstLoginCity = t1.FirstLoginCity,
                FirstLoginIp = t1.FirstLoginIp,
                FirstLoginTime = t1.FirstLoginTime,
                LastLoginTenantName = t3.TenantName,
                LastLoginDevice = t1.LastLoginDevice,
                LastLoginOS = t1.LastLoginOS,
                LastLoginBrowser = t1.LastLoginBrowser,
                LastLoginProvince = t1.LastLoginProvince,
                LastLoginCity = t1.LastLoginCity,
                LastLoginIp = t1.LastLoginIp,
                LastLoginTime = t1.LastLoginTime,
                PasswordErrorTime = t1.PasswordErrorTime,
                LockStartTime = t1.LockStartTime,
                LockEndTime = t1.LockEndTime,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }

    /// <summary>
    /// 编辑账号
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑账号", HttpRequestActionEnum.Edit)]
    public async Task EditAccount(EditAccountInput input)
    {
        var accountModel = await _repository.SingleOrDefaultAsync(_user.AccountId);
        if (accountModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        accountModel.Email = input.Email;
        accountModel.NickName = input.NickName;
        accountModel.Avatar = input.Avatar;
        accountModel.Phone = input.Phone;
        accountModel.Sex = input.Sex;
        accountModel.Birthday = input.Birthday;
        accountModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(accountModel);

        // 刷新缓存
        await _user.RefreshAuth(new AuthUserInfo
        {
            DeviceType = _user.DeviceType,
            DeviceId = _user.DeviceId,
            AppNo = _user.AppNo,
            Mobile = _user.Mobile,
            TenantNo = _user.TenantNo,
            EmployeeNo = _user.EmployeeNo,
            IsSuperAdmin = _user.IsSuperAdmin,
            IsAdmin = _user.IsAdmin,
            NickName = input.NickName,
            Avatar = input.Avatar
        });
    }

    /// <summary>
    /// 账号修改密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("账号修改密码", HttpRequestActionEnum.Edit)]
    public async Task ChangePassword(ChangePasswordInput input)
    {
        if (!string.Equals(input.NewPassword, input.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
        {
            throw new UserFriendlyException("新密码和确认密码不一致！");
        }

        var accountModel = await _repository.SingleOrDefaultAsync(_user.AccountId);
        if (accountModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (!string.Equals(accountModel.Password, input.OldPassword, StringComparison.OrdinalIgnoreCase))
        {
            throw new UserFriendlyException("旧密码不正确！");
        }

        // 更新密码
        accountModel.Password = input.NewPassword.ToUpper();
        accountModel.RowVersion = input.RowVersion;

        var _visitLogRepository =
            FastContext.HttpContext.RequestServices.GetService<ISqlSugarRepository<VisitLogModel>>();

        // 添加访问日志
        var visitLogModel = new VisitLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user.AccountId,
            Account = _user.Account,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            VisitType = VisitTypeEnum.ChangePassword,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            CreatedUserId = _user.UserId,
            CreatedUserName = _user.EmployeeName,
            CreatedTime = DateTime.Now,
            TenantId = _user.TenantId
        };
        visitLogModel.RecordCreate(FastContext.HttpContext);

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.UpdateAsync(accountModel);
            await _visitLogRepository.InsertAsync(visitLogModel);
        });

        // 退出登录
        await _user.Logout();
    }

    /// <summary>
    /// 账号解除锁定
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("账号解除锁定", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Account.Unlock)]
    public async Task Unlock(AccountIdInput input)
    {
        var accountModel = await _repository.SingleOrDefaultAsync(input.AccountId);
        if (accountModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var dateTime = DateTime.Now;

        // 判断是否存在锁定
        if (accountModel.LockEndTime == null || accountModel.LockEndTime < dateTime)
        {
            throw new UserFriendlyException("账号未锁定！");
        }

        accountModel.PasswordErrorTime = null;
        accountModel.LockStartTime = null;
        accountModel.LockEndTime = null;
        accountModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(accountModel);
    }

    /// <summary>
    /// 账号重置密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("账号解除锁定", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Account.ResetPassword)]
    public async Task ResetPassword(AccountIdInput input)
    {
        if (_user.AccountId == input.AccountId)
        {
            throw new UserFriendlyException("禁止重置当前登录账号密码！");
        }

        var accountModel = await _repository.SingleOrDefaultAsync(input.AccountId);
        if (accountModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        // 更新密码
        accountModel.Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.Password)
            .ToUpper();
        accountModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(accountModel);
    }

    /// <summary>
    /// 账号更改状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("账号更改状态", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Account.Status)]
    public async Task ChangeStatus(AccountIdInput input)
    {
        if (_user.AccountId == input.AccountId)
        {
            throw new UserFriendlyException("禁止更改当前登录账号状态！");
        }

        var accountModel = await _repository.SingleOrDefaultAsync(input.AccountId);
        if (accountModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        // 更改状态
        accountModel.Status = accountModel.Status switch
        {
            CommonStatusEnum.Enable => CommonStatusEnum.Disable,
            CommonStatusEnum.Disable => CommonStatusEnum.Enable,
            _ => accountModel.Status
        };
        accountModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(accountModel);

        if (accountModel.Status == CommonStatusEnum.Disable)
        {
            // 强制下线当前账号所有在线用户
            var _hubContext = FastContext.HttpContext.RequestServices.GetService<IHubContext<ChatHub, IChatClient>>();

            var connectionIdList = await _repository.Queryable<TenantOnlineUserModel>()
                .Where(wh => wh.IsOnline)
                .Where(wh => wh.AccountId == accountModel.AccountId)
                .Select(sl => sl.ConnectionId)
                .ToListAsync();

            await _hubContext.Clients.Clients(connectionIdList)
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