using Fast.Center.Entity;
using Fast.Center.Service.App.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.App;

/// <summary>
/// <see cref="AppService"/> App
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "app", Order = 996)]
public class AppService : IDynamicApplication
{
    private readonly ISqlSugarRepository<ApplicationModel> _repository;

    public AppService(ISqlSugarRepository<ApplicationModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Launch
    /// </summary>
    /// <returns></returns>
    [HttpPost("/launch")]
    [ApiInfo("Launch", HttpRequestActionEnum.Auth)]
    [AllowAnonymous]
    [ResponseEncipher]
    public async Task<LaunchOutput> Launch()
    {
        var applicationOpenIdModel = await ApplicationContext.GetApplication(GlobalContext.Origin);
        if (applicationOpenIdModel == null || applicationOpenIdModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("非法访问！");
        }

        return new LaunchOutput
        {
            Edition = applicationOpenIdModel.Application.Edition,
            AppNo = applicationOpenIdModel.Application.AppNo,
            AppName = applicationOpenIdModel.Application.AppName,
            LogoUrl = applicationOpenIdModel.Application.LogoUrl,
            ThemeColor = applicationOpenIdModel.Application.ThemeColor,
            ICPSecurityCode = applicationOpenIdModel.Application.ICPSecurityCode,
            PublicSecurityCode = applicationOpenIdModel.Application.PublicSecurityCode,
            UserAgreement = applicationOpenIdModel.Application.UserAgreement,
            PrivacyAgreement = applicationOpenIdModel.Application.PrivacyAgreement,
            ServiceAgreement = applicationOpenIdModel.Application.ServiceAgreement,
            AppType = applicationOpenIdModel.AppType,
            EnvironmentType = applicationOpenIdModel.EnvironmentType,
            LoginComponent = applicationOpenIdModel.LoginComponent,
            WebSocketUrl = applicationOpenIdModel.WebSocketUrl,
            RequestTimeout = applicationOpenIdModel.RequestTimeout,
            RequestEncipher = applicationOpenIdModel.RequestEncipher
        };
    }
}