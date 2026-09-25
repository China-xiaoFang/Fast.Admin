// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 按钮表Model类
/// </summary>
[SugarTable("Button", "按钮表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(ButtonCode)}", nameof(ButtonCode), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(ButtonName)}", nameof(MenuId), OrderByType.Asc, nameof(ButtonName), OrderByType.Asc, true)]
public class ButtonModel : BaseEntity
{
    /// <summary>
    /// 按钮Id
    /// </summary>
    [SugarColumn(ColumnDescription = "按钮Id", IsPrimaryKey = true)]
    public long ButtonId { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [SugarColumn(ColumnDescription = "版本")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id")]
    public long AppId { get; set; }

    /// <summary>
    /// 菜单Id
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单Id")]
    public long MenuId { get; set; }

    /// <summary>
    /// 按钮编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "按钮编码", Length = 50)]
    public string ButtonCode { get; set; }

    /// <summary>
    /// 按钮名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "按钮名称", Length = 20)]
    public string ButtonName { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    /// <remarks>仅用于初始化角色默认按钮</remarks>
    [SugarColumn(ColumnDescription = "角色类型")]
    public RoleTypeEnum RoleType { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否桌面端")]
    public bool HasDesktop { get; set; }

    /// <summary>
    /// 是否Web端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否Web端")]
    public bool HasWeb { get; set; }

    /// <summary>
    /// 是否移动端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否移动端")]
    public bool HasMobile { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public CommonStatusEnum Status { get; set; }
}
