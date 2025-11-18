namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// <see cref="MenuIdInput"/> 菜单Id输入
/// </summary>
public class MenuIdInput : UpdateVersionInput
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    [LongRequired(ErrorMessage = "菜单Id不能为空")]
    public long MenuId { get; set; }
}