

namespace Fast.Core;

/// <summary>
/// <see cref="MailSettingsOptions"/> 邮件配置选项
/// </summary>
public class MailSettingsOptions
{
    /// <summary>
    /// 发件服务器地址
    /// </summary>
    public string Smtp { get; set; }

    /// <summary>
    /// 发件服务器端口
    /// </summary>
    public int? Port { get; set; }

    /// <summary>
    /// 发件邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 授权码
    /// </summary>
    public string AuthCode { get; set; }

    /// <summary>
    /// 发件人名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 默认收件邮箱
    /// </summary>
    public List<string> ReceiveEmails { get; set; }

    /// <summary>后期配置</summary>
    public void PostConfigure()
    {
        Smtp ??= "smtp.qq.com";
        Port ??= 465;
        DisplayName ??= "FastDotNet";
    }
}