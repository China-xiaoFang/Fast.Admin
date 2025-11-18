namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="AppIdInput"/> 应用Id输入
/// </summary>
public class AppIdInput : UpdateVersionInput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [LongRequired(ErrorMessage = "应用Id不能为空")]
    public long AppId { get; set; }
}