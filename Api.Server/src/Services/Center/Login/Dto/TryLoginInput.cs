namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="TryLoginInput"/> 尝试登录输入
/// </summary>
public class TryLoginInput
{
    /// <summary>
    /// 用户Key
    /// </summary>
    [StringRequired(ErrorMessage = "用户Key不能为空")]
    public string UserKey { get; set; }
}