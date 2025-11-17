namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="LoginInput"/> 登录输入
/// </summary>
public class LoginInput
{
    /// <summary>
    /// 账号
    /// </summary>
    /// <remarks>手机号/账号/工号</remarks>
    [StringRequired(ErrorMessage = "账号不能为空")]
    public string Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [StringRequired(ErrorMessage = "密码不能为空"), MinLength(6, ErrorMessage = "密码不能少于6位字符")]
    public string Password { get; set; }
}