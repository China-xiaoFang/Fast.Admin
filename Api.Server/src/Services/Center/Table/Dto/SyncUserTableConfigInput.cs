namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="SyncUserTableConfigInput"/> 同步用户表格配置输入
/// </summary>
public class SyncUserTableConfigInput
{
    /// <summary>
    /// 表格Key
    /// </summary>
    [StringRequired(ErrorMessage = "表格Key不能为空")]
    public string TableKey { get; set; }
}