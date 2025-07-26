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

using Fast.FastCloud.Enum;
using Fast.FastCloud.Service.Platform.Dto;

namespace Fast.FastCloud.Service.Platform;

/// <summary>
/// <see cref="PlatformService"/> 平台服务
/// </summary>
public class PlatformService : IPlatformService, ITransientDependency
{
    private readonly ISqlSugarRepository<PlatformModel> _repository;

    public PlatformService(ISqlSugarRepository<PlatformModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 平台选择器
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedResult<ElSelectorOutput<long>>> PlatformSelector(PagedInput input)
    {
        var pagedData = await _repository.Entities.PlatformScope(e => e.Id)
            .OrderBy(e => e.PlatformName)
            .Select(e => new
            {
                e.PlatformName,
                e.Id,
                e.AdminName,
                e.AdminMobile,
                e.LogoUrl,
                Disabled = e.Status == CommonStatusEnum.Disable
            })
            .ToPagedListAsync(input);

        return new PagedResult<ElSelectorOutput<long>>
        {
            PageIndex = pagedData.PageIndex,
            PageSize = pagedData.PageSize,
            TotalPage = pagedData.TotalPage,
            TotalRows = pagedData.TotalRows,
            HasPrevPages = pagedData.HasPrevPages,
            HasNextPages = pagedData.HasNextPages,
            Rows = pagedData.Rows.Select(sl => new ElSelectorOutput<long>
            {
                Label = sl.PlatformName,
                Value = sl.Id,
                Disabled = sl.Disabled,
                Data = new {sl.AdminName, sl.AdminMobile, sl.LogoUrl}
            })
        };
    }

    /// <summary>
    /// 获取平台分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedResult<QueryPlatformPagedOutput>> QueryPlatformPaged(PagedInput input)
    {
        return await _repository.Entities.PlatformScope(e => e.Id)
            .OrderBy(e => e.PlatformName)
            .Select(e => new QueryPlatformPagedOutput
            {
                Id = e.Id,
                PlatformNo = e.PlatformNo,
                PlatformName = e.PlatformName,
                ShortName = e.ShortName,
                Status = e.Status,
                AdminName = e.AdminName,
                AdminMobile = e.AdminMobile,
                LogoUrl = e.LogoUrl,
                ActivationTime = e.ActivationTime,
                Edition = e.Edition,
                AutoRenewal = e.AutoRenewal,
                RenewalExpiryTime = e.RenewalExpiryTime,
                IsTrial = e.IsTrial,
                IsInitialized = e.IsInitialized,
                Remark = e.Remark,
                CreatedUserName = e.CreatedUserName,
                CreatedTime = e.CreatedTime,
                UpdatedUserName = e.UpdatedUserName,
                UpdatedTime = e.UpdatedTime
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取平台详情
    /// </summary>
    /// <param name="platformId"></param>
    /// <returns></returns>
    public async Task<QueryPlatformDetailOutput> QueryPlatformDetail(long platformId)
    {
        var result = await _repository.Entities.Where(wh => wh.Id == platformId)
            .Select(sl => new QueryPlatformDetailOutput
            {
                Id = sl.Id,
                PlatformNo = sl.PlatformNo,
                PlatformName = sl.PlatformName,
                ShortName = sl.ShortName,
                Status = sl.Status,
                AdminName = sl.AdminName,
                AdminMobile = sl.AdminMobile,
                AdminEmail = sl.AdminEmail,
                AdminPhone = sl.AdminPhone,
                LogoUrl = sl.LogoUrl,
                ActivationTime = sl.ActivationTime,
                Edition = sl.Edition,
                AutoRenewal = sl.AutoRenewal,
                RenewalExpiryTime = sl.RenewalExpiryTime,
                IsTrial = sl.IsTrial,
                IsInitialized = sl.IsInitialized,
                Remark = sl.Remark,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }

    /// <summary>
    /// 获取平台续费记录分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedResult<QueryPlatformRenewalRecordPagedOutput>> QueryPlatformRenewalRecord(
        QueryPlatformRenewalRecordInput input)
    {
        return await _repository.Queryable<PlatformRenewalRecordModel>()
            .Where(wh => wh.PlatformId == input.PlatformId)
            .OrderBy(ob => ob.RenewalTime)
            .Select(sl => new QueryPlatformRenewalRecordPagedOutput
            {
                Id = sl.Id,
                FromEdition = sl.FromEdition,
                ToEdition = sl.ToEdition,
                RenewalType = sl.RenewalType,
                Duration = sl.Duration,
                RenewalTime = sl.RenewalTime,
                RenewalExpiryTime = sl.RenewalExpiryTime,
                Amount = sl.Amount,
                Remark = sl.Remark,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 初次开通平台
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task FirstActivationPlatform(FirstActivationPlatformInput input)
    {
        if (await _repository.Entities.AnyAsync(a => a.PlatformNo == input.PlatformNo))
        {
            throw new UserFriendlyException("平台编号重复！");
        }

        if (await _repository.Entities.AnyAsync(a => a.PlatformName == input.PlatformName))
        {
            throw new UserFriendlyException("平台名称重复！");
        }

        var dateTime = DateTime.Now;

        // 计算续费到期时间
        var renewalExpiryTime = dateTime.AddDays(input.Duration switch
        {
            RenewalDurationEnum.SevenDays => 7,
            RenewalDurationEnum.FifteenDays => 15,
            RenewalDurationEnum.OneMonth => 30,
            RenewalDurationEnum.FortyFiveDays => 45,
            RenewalDurationEnum.ThreeMonth => 90,
            RenewalDurationEnum.SixMonth => 180,
            RenewalDurationEnum.OneYear => 360,
            RenewalDurationEnum.TwoYear => 720,
            RenewalDurationEnum.ThreeYear => 1080,
            _ => 0
        });

        // 创建平台
        var platformModel = new PlatformModel
        {
            Id = YitIdHelper.NextId(),
            PlatformNo = input.PlatformNo,
            PlatformName = input.PlatformName,
            ShortName = input.ShortName,
            Status = CommonStatusEnum.Enable,
            AdminName = input.AdminName,
            AdminMobile = input.AdminMobile,
            AdminEmail = input.AdminEmail,
            AdminPhone = input.AdminPhone,
            LogoUrl = input.LogoUrl,
            ActivationTime = dateTime,
            Edition = input.Edition,
            AutoRenewal = true,
            RenewalExpiryTime = renewalExpiryTime,
            IsTrial = true,
            IsInitialized = false,
            Remark = input.Remark,
            CreatedTime = dateTime
        };
        // 创建平台续费记录
        var platformRenewalRecordModel = new PlatformRenewalRecordModel
        {
            PlatformId = platformModel.Id,
            FromEdition = EditionEnum.None,
            ToEdition = input.Edition,
            RenewalType = RenewalTypeEnum.Activation,
            Duration = input.Duration,
            RenewalTime = dateTime,
            RenewalExpiryTime = renewalExpiryTime,
            Amount = input.Amount,
            Remark = input.Remark,
            CreatedTime = dateTime
        };
        // 初始化平台核心库
        var databaseModel = new DatabaseModel
        {
            PlatformId = platformModel.Id,
            DatabaseType = DatabaseTypeEnum.Center,
            DbType = input.DbType,
            Status = CommonStatusEnum.Enable,
            PublicIp = input.PublicIp,
            IntranetIp = input.IntranetIp,
            Port = input.Port,
            DbName = input.DbName,
            DbUser = input.DbUser,
            DbPwd = input.DbPwd,
            CustomConnectionStr = input.CustomConnectionStr,
            CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
            SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
            DiffLog = true,
            DisableAop = false,
            CreatedTime = dateTime
        };

        try
        {
            // 开启事务
            await _repository.Ado.BeginTranAsync();

            await _repository.InsertAsync(platformModel);
            await _repository.Insertable(platformRenewalRecordModel)
                .ExecuteCommandAsync();
            await _repository.Insertable(databaseModel)
                .ExecuteCommandAsync();

            // 提交事务
            await _repository.Ado.CommitTranAsync();
        }
        catch
        {
            // 回滚事务
            await _repository.Ado.RollbackTranAsync();
            throw;
        }
    }

    /// <summary>
    /// 编辑平台
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task EditPlatform(EditPlatformInput input)
    {
        // 查询平台详情
        var platformModel = await _repository.SingleAsync(s => s.Id == input.PlatformId);

        if (platformModel == null)
        {
            throw new UserFriendlyException("平台信息不存在！");
        }

        if (platformModel.Status == CommonStatusEnum.Delete)
        {
            throw new UserFriendlyException("平台已删除，禁止操作！");
        }

        if (await _repository.Entities.AnyAsync(a => a.PlatformName == input.PlatformName && a.Id != input.PlatformId))
        {
            throw new UserFriendlyException("平台名称重复！");
        }

        platformModel.PlatformName = input.PlatformName;
        platformModel.ShortName = input.ShortName;
        platformModel.LogoUrl = input.LogoUrl;
        platformModel.Remark = input.Remark;

        // 更新数据
        await _repository.UpdateAsync(platformModel);
    }

    /// <summary>
    /// 启用/禁用平台
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task ChangePlatformStatus(ChangePlatformStatusInput input)
    {
        // 查询平台详情
        var platformModel = await _repository.SingleAsync(s => s.Id == input.PlatformId);

        if (platformModel == null)
        {
            throw new UserFriendlyException("平台信息不存在！");
        }

        platformModel.Status = platformModel.Status switch
        {
            CommonStatusEnum.Delete => throw new UserFriendlyException("平台已删除，禁止操作！"),
            CommonStatusEnum.Enable => CommonStatusEnum.Disable,
            _ => CommonStatusEnum.Enable
        };

        // 更新数据
        await _repository.UpdateAsync(platformModel);
    }
}