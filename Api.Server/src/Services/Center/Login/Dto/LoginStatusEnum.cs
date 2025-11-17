using System.ComponentModel;

namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="LoginStatusEnum"/> 登录状态枚举
/// </summary>
[FastEnum("登录状态枚举")]
public enum LoginStatusEnum
{
    /// <summary>
    /// 登录成功
    /// </summary>
    [Description("登录成功")] Success = 1,

    /// <summary>
    /// 选择租户
    /// </summary>
    [Description("选择租户")] SelectTenant = 2,

    /// <summary>
    /// 授权过期
    /// </summary>
    [Description("授权过期")] AuthExpired = 4,

    /// <summary>
    /// 无账号
    /// </summary>
    [Description("无账号")] NotAccount = 8
}