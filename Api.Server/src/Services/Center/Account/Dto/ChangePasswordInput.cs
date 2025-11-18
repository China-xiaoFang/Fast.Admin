namespace Fast.Center.Service.Account.Dto;

/// <summary>
/// <see cref="ChangePasswordInput"/> 账号修改密码输入
/// </summary>
public class ChangePasswordInput : UpdateVersionInput
{
    /// <summary>
    /// 旧密码
    /// </summary>
    [StringRequired(ErrorMessage = "旧密码不能为空")]
    public string OldPassword { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [StringRequired(ErrorMessage = "新密码不能为空"), MinLength(6, ErrorMessage = "新密码不能少于6位字符")]
    public string NewPassword { get; set; }

    /// <summary>
    /// 确认密码
    /// </summary>
    [StringRequired(ErrorMessage = "确认密码不能为空"), MinLength(6, ErrorMessage = "确认密码不能少于6位字符")]
    public string ConfirmPassword { get; set; }
}