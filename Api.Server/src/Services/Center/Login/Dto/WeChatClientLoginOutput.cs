namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="WeChatClientLoginOutput"/> 微信客户端登录输出
/// </summary>
public class WeChatClientLoginOutput
{
    /// <summary>
    /// 登录状态
    /// </summary>
    public LoginStatusEnum Status { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 唯一用户标识
    /// </summary>
    public string OpenId { get; set; }

    /// <summary>
    /// 统一用户标识
    /// </summary>
    public string UnionId { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }
}