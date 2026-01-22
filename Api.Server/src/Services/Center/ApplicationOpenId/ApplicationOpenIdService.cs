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
using Fast.Center.Service.ApplicationOpenId.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;

namespace Fast.Center.Service.ApplicationOpenId;

/// <summary>
/// <see cref="ApplicationOpenIdService"/> 应用标识服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "applicationOpenId")]
public class ApplicationOpenIdService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<ApplicationOpenIdModel> _repository;

    public ApplicationOpenIdService(IUser user, ISqlSugarRepository<ApplicationOpenIdModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 获取应用标识分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取应用标识分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.AppOpenId.Paged)]
    public async Task<PagedResult<QueryApplicationOpenIdPagedOutput>> QueryApplicationOpenIdPaged(
        QueryApplicationOpenIdPagedInput input)
    {
        var queryable = _repository.Entities.Includes(e => e.Application);
        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);
        if (!_user.IsSuperAdmin && tenantModel.TenantType != TenantTypeEnum.System)
        {
            queryable = queryable.Where(wh => wh.Application.TenantId == _user.TenantId);
        }

        return await queryable.WhereIF(input.AppId != null, wh => wh.AppId == input.AppId)
            .WhereIF(input.AppType != null, wh => wh.AppType == input.AppType)
            .WhereIF(input.EnvironmentType != null, wh => wh.EnvironmentType == input.EnvironmentType)
            .OrderByDescending(ob => ob.AppId)
            .OrderByDescending(ob => ob.CreatedTime)
            .ToPagedListAsync(input,
                sl => new QueryApplicationOpenIdPagedOutput
                {
                    RecordId = sl.RecordId,
                    OpenId = sl.OpenId,
                    AppType = sl.AppType,
                    EnvironmentType = sl.EnvironmentType,
                    RequestTimeout = sl.RequestTimeout,
                    RequestEncipher = sl.RequestEncipher,
                    WeChatMerchantNo = sl.WeChatMerchantNo,
                    AlipayMerchantNo = sl.AlipayMerchantNo,
                    WeChatAccessTokenRefreshTime = sl.WeChatAccessTokenRefreshTime,
                    WeChatJsApiTicketRefreshTime = sl.WeChatJsApiTicketRefreshTime,
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
    /// 获取应用标识详情
    /// </summary>
    /// <param name="recordId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取应用标识详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.AppOpenId.Detail)]
    public async Task<QueryApplicationOpenIdDetailOutput> QueryApplicationOpenIdDetail(
        [Required(ErrorMessage = "记录Id不能为空")] long? recordId)
    {
        var result = await _repository.Entities.Includes(e => e.Application)
            .Where(wh => wh.RecordId == recordId)
            .Select(sl => new QueryApplicationOpenIdDetailOutput
            {
                RecordId = sl.RecordId,
                AppId = sl.AppId,
                AppName = sl.Application.AppName,
                OpenId = sl.OpenId,
                AppType = sl.AppType,
                OpenSecret = sl.OpenSecret,
                EnvironmentType = sl.EnvironmentType,
                LoginComponent = sl.LoginComponent,
                WebSocketUrl = sl.WebSocketUrl,
                RequestTimeout = sl.RequestTimeout,
                RequestEncipher = sl.RequestEncipher,
                StatusBarImageUrl = sl.StatusBarImageUrl,
                ContactPhone = sl.ContactPhone,
                Latitude = sl.Latitude,
                Longitude = sl.Longitude,
                Address = sl.Address,
                BannerImages = sl.BannerImages,
                WeChatMerchantId = sl.WeChatMerchantId,
                WeChatMerchantNo = sl.WeChatMerchantNo,
                AlipayMerchantId = sl.AlipayMerchantId,
                AlipayMerchantNo = sl.AlipayMerchantNo,
                WeChatAccessTokenRefreshTime = sl.WeChatAccessTokenRefreshTime,
                WeChatJsApiTicketRefreshTime = sl.WeChatJsApiTicketRefreshTime,
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

        result.TemplateIdList = await _repository.Queryable<ApplicationTemplateIdModel>()
            .Where(wh => wh.OpenId == result.OpenId)
            .Select(sl => new EditApplicationOpenIdInput.EditApplicationTemplateIdInput
            {
                RecordId = sl.RecordId, TemplateType = sl.TemplateType, TemplateId = sl.TemplateId
            })
            .ToListAsync();

        return result;
    }

    /// <summary>
    /// 添加应用标识
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加应用标识", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.AppOpenId.Add)]
    public async Task AddApplicationOpenId(AddApplicationOpenIdInput input)
    {
        if (await _repository.AnyAsync(a => a.OpenId == input.OpenId))
        {
            throw new UserFriendlyException("应用标识重复！");
        }

        var applicationModel = await _repository.Queryable<ApplicationModel>()
            .InSingleAsync(input.AppId);
        if (applicationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var applicationOpenIdModel = new ApplicationOpenIdModel
        {
            AppId = applicationModel.AppId,
            OpenId = input.OpenId,
            AppType = input.AppType,
            OpenSecret = input.OpenSecret,
            EnvironmentType = input.EnvironmentType,
            LoginComponent = input.LoginComponent,
            WebSocketUrl = input.WebSocketUrl,
            RequestTimeout = input.RequestTimeout,
            RequestEncipher = input.RequestEncipher,
            StatusBarImageUrl = input.StatusBarImageUrl,
            ContactPhone = input.ContactPhone,
            Latitude = input.Latitude,
            Longitude = input.Longitude,
            Address = input.Address,
            BannerImages = input.BannerImages,
            WeChatMerchantId = input.WeChatMerchantId,
            WeChatMerchantNo = input.WeChatMerchantNo,
            AlipayMerchantId = input.AlipayMerchantId,
            AlipayMerchantNo = input.AlipayMerchantNo,
            Remark = input.Remark
        };

        if (!string.IsNullOrWhiteSpace(input.OpenSecret))
        {
            var apiClient = WechatApiClientBuilder
                .Create(new WechatApiClientOptions {AppId = input.OpenId, AppSecret = input.OpenSecret})
                .Build();
            var response = await apiClient.ExecuteCgibinStableTokenAsync(new CgibinStableTokenRequest());
            if (!response.IsSuccessful())
            {
                throw new UserFriendlyException(
                    $"调用刷新AccessToken接口失败。ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
            }

            applicationOpenIdModel.WeChatAccessToken = response.AccessToken;
            applicationOpenIdModel.WeChatAccessTokenExpiresIn = response.ExpiresIn;
            applicationOpenIdModel.WeChatAccessTokenRefreshTime = DateTime.Now;

            if (input.AppType == AppEnvironmentEnum.WeChatServiceAccount)
            {
                var ticketResponse = await apiClient.ExecuteCgibinTicketGetTicketAsync(new CgibinTicketGetTicketRequest
                {
                    AccessToken = response.AccessToken
                });
                if (!ticketResponse.IsSuccessful())
                {
                    throw new UserFriendlyException(
                        $"调用获取Ticket接口失败。ErrorCode：{ticketResponse.ErrorCode}。ErrorMessage：{ticketResponse.ErrorMessage}");
                }

                applicationOpenIdModel.WeChatJsApiTicket = ticketResponse.Ticket;
                applicationOpenIdModel.WeChatJsApiTicketExpiresIn = ticketResponse.ExpiresIn;
                applicationOpenIdModel.WeChatJsApiTicketRefreshTime = DateTime.Now;
            }
        }

        await _repository.InsertAsync(applicationOpenIdModel);
        // 删除缓存
        await ApplicationContext.DeleteApplication(applicationOpenIdModel.OpenId);
    }

    /// <summary>
    /// 编辑应用标识
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑应用标识", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.AppOpenId.Edit)]
    public async Task EditApplicationOpenId(EditApplicationOpenIdInput input)
    {
        if (await _repository.AnyAsync(a => a.OpenId == input.OpenId && a.RecordId != input.RecordId))
        {
            throw new UserFriendlyException("应用标识重复！");
        }

        var templateIds = input.TemplateIdList.Select(sl => sl.TemplateId)
            .Distinct()
            .ToList();
        if (templateIds.Count != input.TemplateIdList.Count)
        {
            throw new UserFriendlyException("模板Id重复！");
        }

        var templateTypes = input.TemplateIdList.Select(sl => sl.TemplateType)
            .Distinct()
            .ToList();
        if (templateTypes.Count != input.TemplateIdList.Count)
        {
            throw new UserFriendlyException("模板类型重复！");
        }

        if (await _repository.Queryable<ApplicationTemplateIdModel>()
                .AnyAsync(a => templateIds.Contains(a.TemplateId) && a.OpenId != input.OpenId))
        {
            throw new UserFriendlyException("模板Id重复！");
        }

        var applicationModel = await _repository.Queryable<ApplicationModel>()
            .InSingleAsync(input.AppId);
        if (applicationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var applicationOpenIdModel = await _repository.SingleOrDefaultAsync(input.RecordId);
        if (applicationOpenIdModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var templateIdList = await _repository.Queryable<ApplicationTemplateIdModel>()
            .Where(wh => wh.OpenId == input.OpenId)
            .ToListAsync();

        applicationOpenIdModel.OpenId = input.OpenId;
        applicationOpenIdModel.AppId = applicationModel.AppId;
        applicationOpenIdModel.AppType = input.AppType;
        applicationOpenIdModel.OpenSecret = input.OpenSecret;
        applicationOpenIdModel.EnvironmentType = input.EnvironmentType;
        applicationOpenIdModel.LoginComponent = input.LoginComponent;
        applicationOpenIdModel.WebSocketUrl = input.WebSocketUrl;
        applicationOpenIdModel.RequestTimeout = input.RequestTimeout;
        applicationOpenIdModel.RequestEncipher = input.RequestEncipher;
        applicationOpenIdModel.StatusBarImageUrl = input.StatusBarImageUrl;
        applicationOpenIdModel.ContactPhone = input.ContactPhone;
        applicationOpenIdModel.Latitude = input.Latitude;
        applicationOpenIdModel.Longitude = input.Longitude;
        applicationOpenIdModel.Address = input.Address;
        applicationOpenIdModel.BannerImages = input.BannerImages;
        applicationOpenIdModel.WeChatMerchantId = input.WeChatMerchantId;
        applicationOpenIdModel.WeChatMerchantNo = input.WeChatMerchantNo;
        applicationOpenIdModel.AlipayMerchantId = input.AlipayMerchantId;
        applicationOpenIdModel.AlipayMerchantNo = input.AlipayMerchantNo;
        applicationOpenIdModel.Remark = input.Remark;
        applicationOpenIdModel.RowVersion = input.RowVersion;

        if (!string.IsNullOrWhiteSpace(input.OpenSecret))
        {
            var apiClient = WechatApiClientBuilder
                .Create(new WechatApiClientOptions {AppId = input.OpenId, AppSecret = input.OpenSecret})
                .Build();
            var response = await apiClient.ExecuteCgibinStableTokenAsync(new CgibinStableTokenRequest());
            if (!response.IsSuccessful())
            {
                throw new UserFriendlyException(
                    $"调用刷新AccessToken接口失败。ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
            }

            applicationOpenIdModel.WeChatAccessToken = response.AccessToken;
            applicationOpenIdModel.WeChatAccessTokenExpiresIn = response.ExpiresIn;
            applicationOpenIdModel.WeChatAccessTokenRefreshTime = DateTime.Now;

            if (input.AppType == AppEnvironmentEnum.WeChatServiceAccount)
            {
                var ticketResponse = await apiClient.ExecuteCgibinTicketGetTicketAsync(new CgibinTicketGetTicketRequest
                {
                    AccessToken = response.AccessToken
                });
                if (!ticketResponse.IsSuccessful())
                {
                    throw new UserFriendlyException(
                        $"调用获取Ticket接口失败。ErrorCode：{ticketResponse.ErrorCode}。ErrorMessage：{ticketResponse.ErrorMessage}");
                }

                applicationOpenIdModel.WeChatJsApiTicket = ticketResponse.Ticket;
                applicationOpenIdModel.WeChatJsApiTicketExpiresIn = ticketResponse.ExpiresIn;
                applicationOpenIdModel.WeChatJsApiTicketRefreshTime = DateTime.Now;
            }
        }

        var addApplicationTemplateIdList = new List<ApplicationTemplateIdModel>();
        var updateApplicationTemplateIdList = new List<ApplicationTemplateIdModel>();
        foreach (var item in input.TemplateIdList)
        {
            ApplicationTemplateIdModel applicationTemplateIdModel;
            if (item.RecordId == null)
            {
                // 新增的
                applicationTemplateIdModel = new ApplicationTemplateIdModel
                {
                    AppId = applicationOpenIdModel.AppId,
                    OpenId = applicationOpenIdModel.OpenId,
                    TemplateType = item.TemplateType,
                    TemplateId = item.TemplateId
                };
                addApplicationTemplateIdList.Add(applicationTemplateIdModel);
            }
            else
            {
                // 更新的
                applicationTemplateIdModel = templateIdList.SingleOrDefault(s => s.RecordId == item.RecordId);
                if (applicationTemplateIdModel == null)
                {
                    throw new UserFriendlyException("数据不存在！");
                }

                applicationTemplateIdModel.TemplateType = item.TemplateType;
                applicationTemplateIdModel.TemplateId = item.TemplateId;
                updateApplicationTemplateIdList.Add(applicationTemplateIdModel);
            }
        }

        // 删除的
        var deleteApplicationTemplateIdList = templateIdList.Where(wh => input.TemplateIdList.All(a => a.RecordId != wh.RecordId))
            .ToList();

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.UpdateAsync(applicationOpenIdModel);
            await _repository.Deleteable(deleteApplicationTemplateIdList)
                .ExecuteCommandAsync();
            await _repository.Updateable(updateApplicationTemplateIdList)
                .ExecuteCommandAsync();
            await _repository.Insertable(addApplicationTemplateIdList)
                .ExecuteCommandAsync();
        }, ex => throw ex);

        // 删除缓存
        await ApplicationContext.DeleteApplication(applicationOpenIdModel.OpenId);
    }

    /// <summary>
    /// 删除应用标识
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除应用标识", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.AppOpenId.Delete)]
    public async Task DeleteApplicationOpenId(RecordIdInput input)
    {
        var applicationOpenIdModel = await _repository.SingleOrDefaultAsync(input.RecordId);
        if (applicationOpenIdModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(applicationOpenIdModel);
        // 删除缓存
        await ApplicationContext.DeleteApplication(applicationOpenIdModel.OpenId);
    }
}