using Fast.Center.Entity;
using Fast.Center.Service.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;

namespace Fast.Center.Service.Application;

/// <summary>
/// <see cref="ApplicationService"/> 应用服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "application")]
public class ApplicationService : IDynamicApplication
{
    private readonly ISqlSugarRepository<ApplicationModel> _repository;

    public ApplicationService(ISqlSugarRepository<ApplicationModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 应用选择器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("应用选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> ApplicationSelector()
    {
        var data = await _repository.Entities.OrderBy(ob => ob.AppName)
            .Select(sl => new { sl.AppId, sl.Edition, sl.AppName, sl.LogoUrl })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
            {
                Value = sl.AppId, Label = sl.AppName, Data = new { sl.Edition, sl.LogoUrl }
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
        return await _repository.Entities.WhereIF(input.Edition != null, wh => wh.Edition == input.Edition)
            .OrderByDescending(ob => ob.CreatedTime)
            .Select(sl => new QueryApplicationPagedOutput
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
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取应用详情
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取应用详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.App.Detail)]
    public async Task<QueryApplicationDetailOutput> QueryApplicationDetail(
        [Required(ErrorMessage = "应用Id不能为空")] long? appId)
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

        result.OpenIdList = await _repository.Queryable<ApplicationOpenIdModel>()
            .Where(wh => wh.AppId == appId)
            .Select(sl => new QueryApplicationDetailOutput.QueryApplicationOpenIdDetailDto
            {
                RecordId = sl.RecordId,
                OpenId = sl.OpenId,
                AppType = sl.AppType,
                EnvironmentType = sl.EnvironmentType,
                LoginComponent = sl.LoginComponent,
                WebSocketUrl = sl.WebSocketUrl,
                RequestTimeout = sl.RequestTimeout,
                RequestEncipher = sl.RequestEncipher,
                OpenSecret = sl.OpenSecret,
                WeChatMerchantId = sl.WeChatMerchantId,
                WeChatMerchantNo = sl.WeChatMerchantNo,
                AlipayMerchantId = sl.AlipayMerchantId,
                AlipayMerchantNo = sl.AlipayMerchantNo,
                WeChatAccessTokenRefreshTime = sl.WeChatAccessTokenRefreshTime,
                WeChatJsApiTicketRefreshTime = sl.WeChatJsApiTicketRefreshTime,
                Remark = sl.Remark
            })
            .ToListAsync();

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
            ThemeColor = input.ThemeColor,
            ICPSecurityCode = input.ICPSecurityCode,
            PublicSecurityCode = input.PublicSecurityCode,
            UserAgreement = input.UserAgreement,
            PrivacyAgreement = input.PrivacyAgreement,
            ServiceAgreement = input.ServiceAgreement,
            Remark = input.Remark
        };

        await _repository.Ado.UseTranAsync(async () =>
        {
            applicationModel.AppNo = SysSerialContext.GenAppNo(_repository);

            await _repository.InsertAsync(applicationModel);
        });
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
        var openIds = input.OpenIdList.Select(sl => sl.OpenId)
            .Distinct()
            .ToList();
        if (openIds.Count != input.OpenIdList.Count)
        {
            throw new UserFriendlyException("应用标识重复！");
        }

        if (await _repository.AnyAsync(a => a.AppName == input.AppName && a.AppId != input.AppId))
        {
            throw new UserFriendlyException("应用名称重复！");
        }

        if (await _repository.Queryable<ApplicationOpenIdModel>()
                .AnyAsync(a => openIds.Contains(a.OpenId) && a.AppId != input.AppId))
        {
            throw new UserFriendlyException("应用标识重复！");
        }

        var applicationModel = await _repository.SingleOrDefaultAsync(input.AppId);
        if (applicationModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var applicationOpenIdList = await _repository.Queryable<ApplicationOpenIdModel>()
            .Where(wh => wh.AppId == input.AppId)
            .ToListAsync();

        applicationModel.Edition = input.Edition;
        applicationModel.AppName = input.AppName;
        applicationModel.LogoUrl = input.LogoUrl;
        applicationModel.ThemeColor = input.ThemeColor;
        applicationModel.ICPSecurityCode = input.ICPSecurityCode;
        applicationModel.PublicSecurityCode = input.PublicSecurityCode;
        applicationModel.UserAgreement = input.UserAgreement;
        applicationModel.PrivacyAgreement = input.PrivacyAgreement;
        applicationModel.ServiceAgreement = input.ServiceAgreement;
        applicationModel.Remark = input.Remark;
        applicationModel.RowVersion = input.RowVersion;

        var addApplicationOpenIdList = new List<ApplicationOpenIdModel>();
        var updateApplicationOpenIdList = new List<ApplicationOpenIdModel>();
        foreach (var item in input.OpenIdList)
        {
            ApplicationOpenIdModel applicationOpenIdModel;
            if (item.RecordId == null)
            {
                // 新增的
                applicationOpenIdModel = new ApplicationOpenIdModel
                {
                    AppId = applicationModel.AppId,
                    OpenId = item.OpenId,
                    AppType = item.AppType,
                    EnvironmentType = item.EnvironmentType,
                    LoginComponent = item.LoginComponent,
                    WebSocketUrl = item.WebSocketUrl,
                    RequestTimeout = item.RequestTimeout,
                    RequestEncipher = item.RequestEncipher,
                    OpenSecret = item.OpenSecret,
                    WeChatMerchantId = item.WeChatMerchantId,
                    WeChatMerchantNo = item.WeChatMerchantNo,
                    AlipayMerchantId = item.AlipayMerchantId,
                    AlipayMerchantNo = item.AlipayMerchantNo,
                    Remark = item.Remark
                };
                addApplicationOpenIdList.Add(applicationOpenIdModel);
            }
            else
            {
                // 更新的
                applicationOpenIdModel = applicationOpenIdList.SingleOrDefault(s => s.RecordId == item.RecordId);
                if (applicationOpenIdModel == null)
                {
                    throw new UserFriendlyException("数据不存在！");
                }

                applicationOpenIdModel.OpenId = item.OpenId;
                applicationOpenIdModel.AppType = item.AppType;
                applicationOpenIdModel.EnvironmentType = item.EnvironmentType;
                applicationOpenIdModel.LoginComponent = item.LoginComponent;
                applicationOpenIdModel.WebSocketUrl = item.WebSocketUrl;
                applicationOpenIdModel.RequestTimeout = item.RequestTimeout;
                applicationOpenIdModel.RequestEncipher = item.RequestEncipher;
                applicationOpenIdModel.OpenSecret = item.OpenSecret;
                applicationOpenIdModel.WeChatMerchantId = item.WeChatMerchantId;
                applicationOpenIdModel.WeChatMerchantNo = item.WeChatMerchantNo;
                applicationOpenIdModel.AlipayMerchantId = item.AlipayMerchantId;
                applicationOpenIdModel.AlipayMerchantNo = item.AlipayMerchantNo;
                applicationOpenIdModel.Remark = item.Remark;
                updateApplicationOpenIdList.Add(applicationOpenIdModel);
            }

            if (!string.IsNullOrWhiteSpace(item.OpenSecret))
            {
                var options = new WechatApiClientOptions { AppId = item.OpenId, AppSecret = item.OpenSecret };
                var client = WechatApiClientBuilder.Create(options)
                    .Build();
                var response = await client.ExecuteCgibinStableTokenAsync(new CgibinStableTokenRequest());
                if (!response.IsSuccessful())
                {
                    throw new UserFriendlyException(
                        $"调用刷新AccessToken接口失败。ErrorCode：{response.ErrorCode}。ErrorMessage：{response.ErrorMessage}");
                }

                applicationOpenIdModel.WeChatAccessToken = response.AccessToken;
                applicationOpenIdModel.WeChatAccessTokenExpiresIn = response.ExpiresIn;
                applicationOpenIdModel.WeChatAccessTokenRefreshTime = DateTime.Now;

                if (item.AppType == AppEnvironmentEnum.WeChatServiceAccount)
                {
                    var ticketResponse = await client.ExecuteCgibinTicketGetTicketAsync(new CgibinTicketGetTicketRequest
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
        }

        // 删除的
        var deleteApplicationOpenIdList = applicationOpenIdList
            .Where(wh => input.OpenIdList.All(a => a.RecordId != wh.RecordId))
            .ToList();

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.UpdateAsync(applicationModel);
            await _repository.Deleteable(deleteApplicationOpenIdList)
                .ExecuteCommandAsync();
            await _repository.Updateable(updateApplicationOpenIdList)
                .ExecuteCommandAsync();
            await _repository.Insertable(addApplicationOpenIdList)
                .ExecuteCommandAsync();
        });

        // 删除全部缓存
        await ApplicationContext.DeleteAllApplication();
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
        // 删除全部缓存
        await ApplicationContext.DeleteAllApplication();
    }

    /// <summary>
    /// 获取接口分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取接口分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.ApiPaged)]
    public async Task<PagedResult<ApiInfoModel>> QueryApiPaged(PagedInput input)
    {
        return await _repository.Queryable<ApiInfoModel>()
            .OrderByDescending(ob => ob.Sort)
            .OrderBy(ob => ob.ServiceName)
            .OrderBy(ob => ob.GroupName)
            .OrderBy(ob => ob.ModuleName)
            .ToPagedListAsync(input);
    }
}