// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 地区表Model类
/// </summary>
[SugarTable("Region", "地区表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"UX_{{table}}_{nameof(RegionCode)}", nameof(RegionCode), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(RegionName)}", nameof(RegionName), OrderByType.Asc)]
public class RegionModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 区域Id
    /// </summary>
    [SugarColumn(ColumnDescription = "区域Id", IsPrimaryKey = true)]
    public long RegionId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id")]
    public long ParentId { get; set; }

    /// <summary>
    /// 行政编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "行政编码", Length = 12)]
    public string RegionCode { get; set; }

    /// <summary>
    /// 区域名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "区域名称", Length = 50)]
    public string RegionName { get; set; }

    /// <summary>
    /// 区号
    /// </summary>
    /// <remarks>地级市</remarks>
    [SugarColumn(ColumnDescription = "区号", Length = 4)]
    public string AreaCode { get; set; }

    /// <summary>
    /// 邮政编码
    /// </summary>
    /// <remarks>区/县/自治县</remarks>
    [SugarColumn(ColumnDescription = "邮政编码", Length = 6)]
    public string PostalCode { get; set; }

    /// <summary>
    /// 区域层级
    /// </summary>
    [SugarColumn(ColumnDescription = "区域层级")]
    public RegionLevelEnum RegionLevel { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
    [SugarColumn(ColumnDescription = "纬度", Length = 20, DecimalDigits = 7)]
    public decimal? Latitude { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    [SugarColumn(ColumnDescription = "经度", Length = 20, DecimalDigits = 7)]
    public decimal? Longitude { get; set; }

    /// <summary>
    /// 全称
    /// </summary>
    [SugarColumn(ColumnDescription = "全称", Length = 200)]
    public string FullRegionName { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    /// <remarks>大写</remarks>
    [SugarColumn(ColumnDescription = "拼音首字母", Length = 50)]
    public string PinYin { get; set; }

    /// <summary>
    /// 拼音全拼
    /// </summary>
    [SugarColumn(ColumnDescription = "拼音全拼", Length = 200)]
    public string PinYinFull { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}
