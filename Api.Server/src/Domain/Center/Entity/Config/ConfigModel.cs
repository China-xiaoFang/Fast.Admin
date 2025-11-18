

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
    /// 配置Id
    /// </summary>
    [SugarColumn(ColumnDescription = "配置Id", IsPrimaryKey = true)]
    public long ConfigId { get; set; }

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