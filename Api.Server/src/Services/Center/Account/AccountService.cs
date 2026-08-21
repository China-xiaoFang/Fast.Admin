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
using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.Center.Service.Account.Dto;
using Fast.CenterLog.Entity;
using Fast.CenterLog.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace Fast.Center.Service.Account;

/// <summary>
/// 账号服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "account")]
public class AccountService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<AccountModel> _repository;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public AccountService(IUser user, ISqlSugarRepository<AccountModel> repository, IHubContext<ChatHub, IChatClient> hubContext)
    {
        _user = user;
        _repository = repository;
        _hubContext = hubContext;
    }

    /// <summary>
    /// 账号选择器
    /// </summary>
    [HttpPost]
    [ApiInfo("账号选择器", HttpRequestActionEnum.Query)]
    public async Task<PagedResult<ElSelectorOutput<long>>> AccountSelector(PagedInput input)
    {
        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);
        if (tenantModel.TenantType == TenantTypeEnum.System)
        {
            var data = await _repository.Entities.WhereIF(!string.IsNullOrWhiteSpace(input.SearchValue),
                    wh => wh.Mobile.Contains(input.SearchValue)
                          || wh.Email.Contains(input.SearchValue)
                          || wh.NickName.Contains(input.SearchValue))
                .OrderBy(ob => ob.Mobile)
                .Select(sl => new AccountModel
                {
                    AccountId = sl.AccountId,
                    Mobile = sl.Mobile,
                    Email = sl.Email,
                    AccountKey = sl.AccountKey,
                    NickName = sl.NickName,
                    Avatar = sl.Avatar
                })
                .OrderBy(ob => ob.NickName)
                .ToPagedListAsync(input);

            return data.ToPagedData(sl => new ElSelectorOutput<long>
            {
                Value = sl.AccountId, Label = sl.Mobile, Data = new {sl.Email, sl.AccountKey, sl.NickName, sl.Avatar}
            });
        }
        else
        {
            var data = await _repository.Queryable<TenantUserModel>()
                .InnerJoin<AccountModel>((t1, t2) => t1.AccountId == t2.AccountId)
                .WhereIF(!string.IsNullOrWhiteSpace(input.SearchValue),
                    (t1, t2) => t2.Mobile.Contains(input.SearchValue)
                                || t2.Email.Contains(input.SearchValue)
                                || t2.NickName.Contains(input.SearchValue))
                .OrderBy((t1, t2) => t2.NickName)
                .Select((t1, t2) => new AccountModel
                {
                    AccountId = t2.AccountId,
                    Mobile = t2.Mobile,
                    Email = t2.Email,
                    AccountKey = t2.AccountKey,
                    NickName = t2.NickName,
                    Avatar = t2.Avatar
                })
                .Distinct()
                .ToPagedListAsync(input);

            return data.ToPagedData(sl => new ElSelectorOutput<long>
            {
                Value = sl.AccountId, Label = sl.Mobile, Data = new {sl.Email, sl.AccountKey, sl.NickName, sl.Avatar}
            });
        }
    }

    /// <summary>
    /// 获取账号分页列表
    /// </summary>
    [HttpPost]
    [ApiInfo("获取账号分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Account.Paged)]
    [PlatformOnly]
    public async Task<PagedResult<QueryAccountPagedOutput>> QueryAccountPaged(QueryAccountPagedInput input)
    {
        var dateTime = DateTime.Now;

        var queryable = _repository.Queryable<AccountModel>()
            .LeftJoin<TenantModel>((t1, t2) => t1.FirstLoginTenantId == t2.TenantId)
            .LeftJoin<TenantModel>((t1, t2, t3) => t1.LastLoginTenantId == t3.TenantId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Mobile), t1 => t1.Mobile.Contains(input.Mobile))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Email), t1 => t1.Email.Contains(input.Email))
            .WhereIF(input.Status != null, t1 => t1.Status == input.Status)
            .WhereIF(!string.IsNullOrWhiteSpace(input.FirstLoginCity), t1 => t1.FirstLoginCity.Contains(input.FirstLoginCity))
            .WhereIF(!string.IsNullOrWhiteSpace(input.FirstLoginIp), t1 => t1.FirstLoginIp.Contains(input.FirstLoginIp))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LastLoginCity), t1 => t1.LastLoginCity.Contains(input.LastLoginCity))
            .WhereIF(!string.IsNullOrWhiteSpace(input.LastLoginIp), t1 => t1.LastLoginIp.Contains(input.LastLoginIp))
            .WhereIF(input.IsLock == true, t1 => t1.LockEndTime != null && t1.LockEndTime >= dateTime)
            .WhereIF(input.IsLock == false, t1 => t1.LockEndTime == null || t1.LockEndTime < dateTime);

        return await queryable.SelectMergeTable((t1, t2, t3) => new QueryAccountPagedOutput
            {
                AccountId = t1.AccountId,
                Mobile = t1.Mobile,
                Email = t1.Email,
                Status = t1.Status,
                NickName = t1.NickName,
                Avatar = t1.Avatar,
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
                IsLock = SqlFunc.IF(t1.LockEndTime != null && t1.LockEndTime >= dateTime)
                    .Return(true)
                    .End(false),
                CreatedTime = t1.CreatedTime,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion
            })
            .OrderByIF(input.IsOrderBy, ob => ob.CreatedTime, OrderByType.Desc)
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取账号详情
    /// </summary>
    [HttpGet]
    [ApiInfo("获取账号详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Account.Detail)]
    [PlatformOnly]
    public async Task<QueryAccountDetailOutput> QueryAccountDetail([Required(ErrorMessage = "账号Id不能为空")] long? accountId)
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
                WeChatId = t1.WeChatId,
                Status = t1.Status,
                NickName = t1.NickName,
                Avatar = t1.Avatar,
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
                CreatedTime = t1.CreatedTime,
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
    /// 获取编辑账号详情
    /// </summary>
    [HttpGet]
    [ApiInfo("获取编辑账号详情", HttpRequestActionEnum.Query)]
    public async Task<EditAccountInput> QueryEditAccountDetail()
    {
        var result = await _repository.Queryable<AccountModel>()
            .Where(wh => wh.AccountId == _user.AccountId)
            .Select(sl => new EditAccountInput
            {
                Mobile = sl.Mobile,
                Email = sl.Email,
                NickName = sl.NickName,
                Avatar = sl.Avatar,
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
    /// 编辑账号
    /// </summary>
    [HttpPost]
    [ApiInfo("编辑账号", HttpRequestActionEnum.Edit)]
    public async Task EditAccount(EditAccountInput input)
    {
        if (!new Regex(RegexConst.Mobile).IsMatch(input.Mobile))
        {
            throw new UserFriendlyException("手机号码不正确！");
        }

        var accountModel = await _repository.SingleOrDefaultAsync(_user.AccountId);
        if (accountModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.Mobile == input.Mobile && a.AccountId != _user.AccountId))
        {
            throw new UserFriendlyException("手机号已存在账号信息！");
        }

        if (await _repository.AnyAsync(a => a.Email == input.Email && a.AccountId != _user.AccountId))
        {
            throw new UserFriendlyException("邮箱已存在账号信息！");
        }

        accountModel.Mobile = input.Mobile;
        accountModel.Email = input.Email;
        accountModel.NickName = input.NickName;
        accountModel.Avatar = input.Avatar;
        accountModel.RowVersion = input.RowVersion;

        // 同步微信用户信息
        UserModel userModel = null;
        if (accountModel.WeChatId != null)
        {
            userModel = await _repository.Queryable<UserModel>()
                .InSingleAsync(accountModel.WeChatId);

            if (userModel == null)
            {
                // 自动解绑
                accountModel.WeChatId = null;
            }
            else
            {
                userModel.NickName = input.NickName;
                userModel.Avatar = input.Avatar;
            }
        }

        await _repository.Ado.UseTranAsync(async () =>
        {
            if (userModel != null)
            {
                await _repository.Updateable(userModel)
                    .ExecuteCommandAsync();
            }

            await _repository.UpdateAsync(accountModel);
        }, ex => throw ex);

        // 刷新缓存
        await _user.RefreshAccount(new RefreshAccountDto
        {
            DeviceType = _user.DeviceType,
            AppNo = _user.AppNo,
            Mobile = accountModel.Mobile,
            NickName = input.NickName,
            Avatar = input.Avatar,
            TenantNo = _user.TenantNo,
            EmployeeNo = _user.EmployeeNo
        });
    }

    /// <summary>
    /// 账号修改密码
    /// </summary>
    [HttpPost]
    [ApiInfo("账号修改密码", HttpRequestActionEnum.Edit)]
    public async Task ChangePassword(ChangePasswordInput input)
    {
        if (!string.Equals(input.NewPassword, input.ConfirmPassword, StringComparison.Ordinal))
        {
            throw new UserFriendlyException("新密码和确认密码不一致！");
        }

        // 客户端提交原始密码后，服务端可以直接校验真实复杂度
        if (input.NewPassword.Length < 8
            || !input.NewPassword.Any(char.IsUpper)
            || !input.NewPassword.Any(char.IsLower)
            || !input.NewPassword.Any(char.IsDigit))
        {
            throw new UserFriendlyException("新密码至少8位，且必须包含大小写字母、数字！");
        }

        var accountModel = await _repository.SingleOrDefaultAsync(_user.AccountId);
        if (accountModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }


        if (!CryptoUtil.VerifyPasswordPBKDF2SHA256(input.OldPassword, accountModel.Password))
        {
            throw new UserFriendlyException("旧密码不正确！");
        }

        // 查询最近5次密码修改记录
        var passwordRecordList = await _repository.Queryable<PasswordRecordModel>()
            .Where(wh => wh.AccountId == accountModel.AccountId)
            .OrderByDescending(ob => ob.CreatedTime)
            .Take(3)
            .Select(sl => sl.Password)
            .ToListAsync();
        if (passwordRecordList.Any(history => CryptoUtil.VerifyPasswordPBKDF2SHA256(input.NewPassword, history)))
            throw new UserFriendlyException("新密码不能与最近3次使用的密码相同！");

        // 更新密码
        accountModel.Password = CryptoUtil.HashPasswordPBKDF2SHA256(input.NewPassword);
        accountModel.RowVersion = input.RowVersion;

        var _visitLogRepository = FastContext.HttpContext.RequestServices.GetService<ISqlSugarRepository<VisitLogModel>>();

        // 添加访问日志
        var visitLogModel = new VisitLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user.AccountId,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            VisitType = VisitTypeEnum.ChangePassword,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            CreatedUserId = _user.EmployeeId,
            CreatedUserName = _user.EmployeeName,
            CreatedTime = DateTime.Now,
            TenantId = _user.TenantId,
            TenantName = _user.TenantName
        };
        visitLogModel.RecordCreate(FastContext.HttpContext);

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.UpdateAsync(accountModel);
            await _repository.Insertable(new PasswordRecordModel
                {
                    AccountId = accountModel.AccountId,
                    OperationType = PasswordOperationTypeEnum.Change,
                    Type = PasswordTypeEnum.PBKDF2_SHA256,
                    Password = accountModel.Password
                })
                .ExecuteCommandAsync();
            await _visitLogRepository.InsertAsync(visitLogModel);
        }, ex => throw ex);

        // 退出登录
        await _user.Logout();
        await _user.RevokeAccount(accountModel.AccountId);
        await AccountForceOffline(accountModel.AccountId, "密码已修改，请重新登录");
    }

    /// <summary>
    /// 账号解除锁定
    /// </summary>
    [HttpPost]
    [ApiInfo("账号解除锁定", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Account.Unlock)]
    [PlatformOnly]
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
    [HttpPost]
    [ApiInfo("账号重置密码", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Account.ResetPassword)]
    [PlatformOnly]
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
        accountModel.Password = CryptoUtil.HashPasswordPBKDF2SHA256(CommonConst.Default.Password);
        accountModel.RowVersion = input.RowVersion;

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.UpdateAsync(accountModel);
            await _repository.Insertable(new PasswordRecordModel
                {
                    AccountId = accountModel.AccountId,
                    OperationType = PasswordOperationTypeEnum.Reset,
                    Type = PasswordTypeEnum.PBKDF2_SHA256,
                    Password = accountModel.Password
                })
                .ExecuteCommandAsync();
        }, ex => throw ex);

        await _user.RevokeAccount(accountModel.AccountId);
        await AccountForceOffline(accountModel.AccountId, "密码已重置，请重新登录");
    }

    /// <summary>
    /// 账号更改状态
    /// </summary>
    [HttpPost]
    [ApiInfo("账号更改状态", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Account.Status)]
    [PlatformOnly]
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
            await _user.RevokeAccount(accountModel.AccountId);
            await AccountForceOffline(accountModel.AccountId, "账号已被禁用");
        }
    }

    /// <summary>
    /// 强制下线账号
    /// </summary>
    private async Task AccountForceOffline(long accountId, string message)
    {
        var connectionIds = await _repository.Queryable<TenantOnlineUserModel>()
            .ClearFilter<IBaseTEntity>()
            .Where(wh => wh.IsOnline)
            .Where(wh => wh.AccountId == accountId)
            .Select(sl => sl.ConnectionId)
            .ToListAsync();
        if (connectionIds.Count == 0)
            return;

        // 强制下线当前账号所有在线用户
        await _hubContext.Clients.Clients(connectionIds)
            .ForceOffline(new ForceOfflineOutput
            {
                IsAdmin = _user.IsSuperAdmin || _user.IsAdmin,
                NickName = _user.NickName,
                EmployeeNo = _user.EmployeeNo,
                OfflineTime = DateTime.Now,
                Message = message
            });
    }
}