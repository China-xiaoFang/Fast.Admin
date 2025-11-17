namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="TenantLoginInput"/> 租户登录输入
/// </summary>
public class TenantLoginInput
{
    /// <summary>
    /// 账号Key
    /// </summary>
    public string AccountKey { get; set; }

    /// <summary>
    /// 用户Key
    /// </summary>
    [StringRequired(ErrorMessage = "用户Key不能为空")]
    public string UserKey { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}