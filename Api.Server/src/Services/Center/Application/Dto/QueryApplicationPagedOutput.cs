namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="QueryApplicationPagedOutput"/> 获取应用分页列表输出
/// </summary>
public class QueryApplicationPagedOutput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    public long AppId { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    public string AppNo { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    public string LogoUrl { get; set; }

    /// <summary>
    /// 主题色
    /// </summary>
    public string ThemeColor { get; set; }

    /// <summary>
    /// ICP备案号
    /// </summary>
    public string ICPSecurityCode { get; set; }

    /// <summary>
    /// 公安备案号
    /// </summary>
    public string PublicSecurityCode { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    public string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }
}