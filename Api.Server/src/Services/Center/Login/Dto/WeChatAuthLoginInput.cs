namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="WeChatAuthLoginInput"/> 微信授权登录输入
/// </summary>
public class WeChatAuthLoginInput
{
    /// <summary>
    /// wx.login 获取到的Code
    /// </summary>
    [StringRequired(ErrorMessage = "微信Code不能为空")]
    public string WeChatCode { get; set; }

    /// <summary>
    /// 动态令牌
    /// </summary>
    [StringRequired(ErrorMessage = "动态令牌不能为空")]
    public string Code { get; set; }
}