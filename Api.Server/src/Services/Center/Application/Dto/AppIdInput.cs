namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="AppIdInput"/> 应用Id输入
/// </summary>
public class AppIdInput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [LongRequired(ErrorMessage = "应用Id不能为空")]
    public long AppId { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }
}