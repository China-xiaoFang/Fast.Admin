// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

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