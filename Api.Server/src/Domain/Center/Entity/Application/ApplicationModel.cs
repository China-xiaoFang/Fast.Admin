// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="ApplicationModel"/> 应用表Model类
/// </summary>
[SugarTable("Application", "应用表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(AppNo)}", nameof(AppNo), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(AppName)}", nameof(AppName), OrderByType.Asc, true)]
public class ApplicationModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id", IsPrimaryKey = true)]
    public long AppId { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [SugarColumn(ColumnDescription = "版本")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "应用编号", Length = 11)]
    public string AppNo { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "应用名称", Length = 20)]
    public string AppName { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "LogoUrl", Length = 200)]
    public string LogoUrl { get; set; }

    /// <summary>
    /// 主题色
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "主题色", Length = 7)]
    public string ThemeColor { get; set; }

    /// <summary>
    /// ICP备案号
    /// </summary>
    [SugarColumn(ColumnDescription = "ICP备案号", Length = 20)]
    public string ICPSecurityCode { get; set; }

    /// <summary>
    /// 公安备案号
    /// </summary>
    [SugarColumn(ColumnDescription = "公安备案号", Length = 30)]
    public string PublicSecurityCode { get; set; }

    /// <summary>
    /// 用户协议
    /// </summary>
    [SugarColumn(ColumnDescription = "用户协议", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string UserAgreement { get; set; }

    /// <summary>
    /// 隐私协议
    /// </summary>
    [SugarColumn(ColumnDescription = "隐私协议", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string PrivacyAgreement { get; set; }

    /// <summary>
    /// 服务协议
    /// </summary>
    [SugarColumn(ColumnDescription = "服务协议", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string ServiceAgreement { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string Remark { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}