// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 配置表Model类
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
    [SugarColumn(ColumnDescription = "配置编码", Length = 50)]
    public string ConfigCode { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    [Required]
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
