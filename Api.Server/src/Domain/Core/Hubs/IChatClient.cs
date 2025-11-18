using Fast.Center.Entity;


namespace Fast.Core;

/// <summary>
/// <see cref="IChatClient"/> 集线器客户端接口
/// </summary>
public interface IChatClient
{
    /// <summary>
    /// 连接成功
    /// </summary>
    /// <returns></returns>
    Task ConnectSuccess();

    /// <summary>
    /// 登录失败
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task LoginFail(string message);

    /// <summary>
    /// 其他地方登录
    /// </summary>
    /// <param name="onlineUser"></param>
    /// <returns></returns>
    Task ElsewhereLogin(TenantOnlineUserModel onlineUser);

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task ForceOffline(ForceOfflineOutput input);
}