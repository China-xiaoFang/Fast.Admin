// ------------------------------------------------------------------------
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本软件及其相关文档（以下简称“软件”）为项目定制，仅供获得授权的个人或组织使用。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发软件副本的权利。
// 但不得用于违反法律或侵犯他人合法权益的行为。
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

// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="ConfigModel"/> 配置表Model类
/// </summary>
[SugarTable("Config", "配置表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(ConfigCode)}", nameof(ConfigCode), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(ConfigName)}", nameof(ConfigName), OrderByType.Asc, true)]
public class ConfigModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 配置编码
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "配置编码", Length = 50)]
    public string ConfigCode { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "配置名称", Length = 50)]
    public string ConfigName { get; set; }

    /// <summary>
    /// 配置值
    /// </summary>
    /// <remarks>
    /// <para>Boolean：[True, False]</para>
    /// </remarks>
    [Required]
    [SugarColumn(ColumnDescription = "配置值", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string ConfigValue { get; set; }

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