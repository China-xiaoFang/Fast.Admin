namespace Fast.Center.Service.File.Dto;

/// <summary>
/// <see cref="DownloadFileInput"/> 下载文件输入
/// </summary>
public class DownloadFileInput
{
    /// <summary>
    /// 文件Id
    /// </summary>
    [LongRequired(ErrorMessage = "文件Id不能为空")]
    public long FileId { get; set; }
}