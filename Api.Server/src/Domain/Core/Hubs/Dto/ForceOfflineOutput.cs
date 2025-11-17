namespace Fast.Core.Hubs.Dto;

/// <summary>
/// <see cref="ForceOfflineOutput"/> 强制下线输出
/// </summary>
public class ForceOfflineOutput
{
    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public string EmployeeNo { get; set; }

    /// <summary>
    /// 下线时间
    /// </summary>
    public DateTime OfflineTime { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }
}