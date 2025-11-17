namespace Fast.Center.Service.App.Dto;

/// <summary>
/// <see cref="LaunchOutput"/> Launch 输出
/// </summary>
public class LaunchOutput
{
    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    public string AppNo { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    public string LogoUrl { get; set; }

    /// <summary>
    /// 主题色
    /// </summary>
    [Required]
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
    /// 应用类型
    /// </summary>
    public AppEnvironmentEnum AppType { get; set; }

    /// <summary>
    /// 环境类型
    /// </summary>
    public EnvironmentTypeEnum EnvironmentType { get; set; }

    /// <summary>
    /// 登录组件
    /// </summary>
    public string LoginComponent { get; set; }

    /// <summary>
    /// WebSocket地址
    /// </summary>
    public string WebSocketUrl { get; set; }

    /// <summary>
    /// 请求超时时间（毫秒）
    /// </summary>
    public int RequestTimeout { get; set; }

    /// <summary>
    /// 请求加密
    /// </summary>
    public bool RequestEncipher { get; set; }
}