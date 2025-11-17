namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="AddApplicationInput"/> 添加应用输入
/// </summary>
public class AddApplicationInput
{
    /// <summary>
    /// 版本
    /// </summary>
    [EnumRequired(ErrorMessage = "版本不能为空")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [StringRequired(ErrorMessage = "应用名称不能为空")]
    public string AppName { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    [StringRequired(ErrorMessage = "LogoUrl不能为空")]
    public string LogoUrl { get; set; }

    /// <summary>
    /// 主题色
    /// </summary>
    [StringRequired(ErrorMessage = "主题色不能为空")]
    public string ThemeColor { get; set; }

    /// <summary>
    /// ICP备案号
    /// </summary>
    public string ICPSecurityCode { get; set; }

    /// <summary>
    /// 公安备案号
    /// </summary>
    public string PublicSecurityCode { get; set; }

    /// <summary>
    /// 用户协议
    /// </summary>
    public string UserAgreement { get; set; }

    /// <summary>
    /// 隐私协议
    /// </summary>
    public string PrivacyAgreement { get; set; }

    /// <summary>
    /// 服务协议
    /// </summary>
    public string ServiceAgreement { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}