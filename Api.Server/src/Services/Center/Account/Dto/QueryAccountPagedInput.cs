namespace Fast.Center.Service.Account.Dto;

/// <summary>
/// <see cref="QueryAccountPagedInput"/> 获取账号分页列表输入
/// </summary>
public class QueryAccountPagedInput : PagedInput
{
    /// <summary>
    /// 手机
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum? Status { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderEnum? Sex { get; set; }

    /// <summary>
    /// 初次登录城市
    /// </summary>
    public string FirstLoginCity { get; set; }

    /// <summary>
    /// 初次登录Ip
    /// </summary>
    public string FirstLoginIp { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    public string LastLoginCity { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    public string LastLoginIp { get; set; }

    /// <summary>
    /// 是否锁定
    /// </summary>
    public bool? IsLock { get; set; }
}