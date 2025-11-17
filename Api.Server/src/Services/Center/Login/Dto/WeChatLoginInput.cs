namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="WeChatLoginInput"/> 微信登录输入
/// </summary>
public class WeChatLoginInput
{
    /// <summary>
    /// wx.login 获取到的Code
    /// </summary>
    [StringRequired(ErrorMessage = "微信Code不能为空")]
    public string WeChatCode { get; set; }

    /// <summary>
    /// 加密算法的初始向量
    /// </summary>
    public string IV { get; set; }

    /// <summary>
    /// 包括敏感数据在内的完整用户信息的加密数据
    /// </summary>
    public string EncryptedData { get; set; }
}