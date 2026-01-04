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
using Fast.Center.Service.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.Application;

/// <summary>
/// <see cref="ApplicationService"/> 应用服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "application")]
public class ApplicationService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<ApplicationModel> _repository;

    public ApplicationService(IUser user, ISqlSugarRepository<ApplicationModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 应用选择器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("应用选择器", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.App.Paged)]
    public async Task<List<ElSelectorOutput<long>>> ApplicationSelector()
    {
        var queryable = _repository.Entities;
        if (!_user.IsSuperAdmin)
        {
            queryable = queryable.Where(wh => wh.TenantId == _user.TenantId);
        }

        var data = await queryable.OrderBy(ob => ob.AppName)
            .Select(sl => new
            {
                sl.AppId,
                sl.Edition,
                sl.AppName,
                sl.AppNo,
                sl.LogoUrl
            })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
            {
                Value = sl.AppId, Label = sl.AppName, Data = new {sl.AppNo, sl.Edition, sl.LogoUrl}
            })
            .ToList();
    }

    /// <summary>
    /// 获取应用分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取应用分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.App.Paged)]
    public async Task<PagedResult<QueryApplicationPagedOutput>> QueryApplicationPaged(QueryApplicationPagedInput input)
    {
        var queryable = _repository.Entities;
        if (!_user.IsSuperAdmin)
        {
            queryable = queryable.Where(wh => wh.TenantId == _user.TenantId);
        }

        return await queryable.WhereIF(input.Edition != null, wh => wh.Edition == input.Edition)
            .OrderByDescending(ob => ob.CreatedTime)
            .ToPagedListAsync(input,
                sl => new QueryApplicationPagedOutput
                {
                    AppId = sl.AppId,
                    Edition = sl.Edition,
                    AppNo = sl.AppNo,
                    AppName = sl.AppName,
                    LogoUrl = sl.LogoUrl,
                    ThemeColor = sl.ThemeColor,
                    ICPSecurityCode = sl.ICPSecurityCode,
                    PublicSecurityCode = sl.PublicSecurityCode,
                    Remark = sl.Remark,
                    TenantName = sl.TenantName,
                    DepartmentName = sl.DepartmentName,
                    CreatedUserName = sl.CreatedUserName,
                    CreatedTime = sl.CreatedTime,
                    UpdatedUserName = sl.UpdatedUserName,
                    UpdatedTime = sl.UpdatedTime,
                    RowVersion = sl.RowVersion
                });
    }

    /// <summary>
    /// 获取应用详情
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取应用详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.App.Detail)]
    public async Task<QueryApplicationDetailOutput> QueryApplicationDetail([Required(ErrorMessage = "应用Id不能为空")] long? appId)
    {
        var result = await _repository.Entities.Where(wh => wh.AppId == appId)
            .Select(sl => new QueryApplicationDetailOutput
            {
                AppId = sl.AppId,
                Edition = sl.Edition,
                AppNo = sl.AppNo,
                AppName = sl.AppName,
                LogoUrl = sl.LogoUrl,
                ThemeColor = sl.ThemeColor,
                ICPSecurityCode = sl.ICPSecurityCode,
                PublicSecurityCode = sl.PublicSecurityCode,
                UserAgreement = sl.UserAgreement,
                PrivacyAgreement = sl.PrivacyAgreement,
                ServiceAgreement = sl.ServiceAgreement,
                Remark = sl.Remark,
                TenantId = sl.TenantId,
                TenantName = sl.TenantName,
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
    /// 添加应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加应用", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.App.Add)]
    public async Task AddApplication(AddApplicationInput input)
    {
        if (await _repository.AnyAsync(a => a.AppName == input.AppName))
        {
            throw new UserFriendlyException("应用名称重复！");
        }

        var applicationModel = new ApplicationModel
        {
            Edition = input.Edition,
            AppName = input.AppName,
            LogoUrl = input.LogoUrl,
            ThemeColor = input.ThemeColor?.ToUpper(),
            ICPSecurityCode = input.ICPSecurityCode,
            PublicSecurityCode = input.PublicSecurityCode,
            UserAgreement = input.UserAgreement,
            PrivacyAgreement = input.PrivacyAgreement,
            ServiceAgreement = input.ServiceAgreement,
            Remark = input.Remark,
            TenantId = input.TenantId,
            TenantName = input.TenantName
        };

        await _repository.Ado.UseTranAsync(async () =>
        {
            applicationModel.AppNo = SysSerialContext.GenAppNo(_repository);

            await _repository.InsertAsync(applicationModel);
        }, ex => throw ex);
    }

    /// <summary>
    /// 编辑应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑应用", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.App.Edit)]
    public async Task EditApplication(EditApplicationInput input)
    {
        if (await _repository.AnyAsync(a => a.AppName == input.AppName && a.AppId != input.AppId))
        {
            throw new UserFriendlyException("应用名称重复！");
        }

        var applicationModel = await _repository.SingleOrDefaultAsync(input.AppId);
        if (applicationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        applicationModel.Edition = input.Edition;
        applicationModel.AppName = input.AppName;
        applicationModel.LogoUrl = input.LogoUrl;
        applicationModel.ThemeColor = input.ThemeColor?.ToUpper();
        applicationModel.ICPSecurityCode = input.ICPSecurityCode;
        applicationModel.PublicSecurityCode = input.PublicSecurityCode;
        applicationModel.UserAgreement = input.UserAgreement;
        applicationModel.PrivacyAgreement = input.PrivacyAgreement;
        applicationModel.ServiceAgreement = input.ServiceAgreement;
        applicationModel.Remark = input.Remark;
        applicationModel.TenantId = input.TenantId;
        applicationModel.TenantName = input.TenantName;
        applicationModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(applicationModel);
    }

    /// <summary>
    /// 删除应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除应用", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.App.Delete)]
    public async Task DeleteApplication(AppIdInput input)
    {
        var applicationModel = await _repository.SingleOrDefaultAsync(input.AppId);
        if (applicationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.Queryable<ApplicationOpenIdModel>()
                .AnyAsync(a => a.AppId == input.AppId))
        {
            throw new UserFriendlyException("应用存在OpenId信息，无法删除！");
        }

        await _repository.DeleteAsync(applicationModel);
    }
}