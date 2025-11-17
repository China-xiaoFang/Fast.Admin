namespace Fast.Center.Service.Account.Dto;

/// <summary>
/// <see cref="AccountIdInput"/> 账号Id输入
/// </summary>
public class AccountIdInput
{
    /// <summary>
    /// 账号Id
    /// </summary>
    [LongRequired(ErrorMessage = "账号Id不能为空")]
    public long AccountId { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }
}