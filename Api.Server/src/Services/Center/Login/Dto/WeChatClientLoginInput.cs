namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="WeChatClientLoginInput"/> 微信客户端登录输入
/// </summary>
public class WeChatClientLoginInput
{
    /// <summary>
    /// 微信Code
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

    /// <summary>
    /// 动态令牌
    /// </summary>
    public string Code { get; set; }
}