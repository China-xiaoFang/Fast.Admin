namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="MenuIdInput"/> 菜单Id输入
/// </summary>
public class MenuIdInput
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    [LongRequired(ErrorMessage = "菜单Id不能为空")]
    public long MenuId { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }
}