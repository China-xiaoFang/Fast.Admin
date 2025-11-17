namespace Fast.Center.Service.Account.Dto;

/// <summary>
/// <see cref="EditAccountInput"/> 编辑账号输入
/// </summary>
public class EditAccountInput
{
    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [StringRequired(ErrorMessage = "昵称不能为空")]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [EnumRequired(ErrorMessage = "性别不能为空")]
    public GenderEnum Sex { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }
}