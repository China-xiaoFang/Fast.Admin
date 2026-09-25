// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 职级表Model类
/// </summary>
[SugarTable("JobLevel", "职级表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(JobLevelName)}", nameof(JobLevelName), OrderByType.Asc, true)]
public class JobLevelModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 职级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职级Id", IsPrimaryKey = true)]
    public long JobLevelId { get; set; }

    /// <summary>
    /// 职级名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "职级名称", Length = 20)]
    public string JobLevelName { get; set; }

    /// <summary>
    /// 职级等级
    /// </summary>
    [SugarColumn(ColumnDescription = "职级等级", Length = 5)]
    public string Level { get; set; }

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
