// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 菜单表Model类
/// </summary>
[SugarTable("Menu", "菜单表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(MenuCode)}", nameof(MenuCode), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(MenuName)}", nameof(AppId), OrderByType.Asc, nameof(MenuName), OrderByType.Asc, true)]
public class MenuModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单Id", IsPrimaryKey = true)]
    public long MenuId { get; set; }

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
    /// 菜单编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "菜单编码", Length = 50)]
    public string MenuCode { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "菜单名称", Length = 20)]
    public string MenuName { get; set; }

    /// <summary>
    /// 菜单标题
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "菜单标题", Length = 20)]
    public string MenuTitle { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id")]
    public long ParentId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<long> ParentIds { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单类型")]
    public MenuTypeEnum MenuType { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    /// <remarks>仅用于初始化角色默认菜单</remarks>
    [SugarColumn(ColumnDescription = "角色类型")]
    public RoleTypeEnum RoleType { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否桌面端")]
    public bool HasDesktop { get; set; }

    /// <summary>
    /// 桌面端图标
    /// </summary>
    [SugarColumn(ColumnDescription = "桌面端图标", Length = 200)]
    public string DesktopIcon { get; set; }

    /// <summary>
    /// 桌面端路由地址
    /// </summary>
    [SugarColumn(ColumnDescription = "桌面端路由地址", Length = 200)]
    public string DesktopRouter { get; set; }

    /// <summary>
    /// 是否Web端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否Web端")]
    public bool HasWeb { get; set; }

    /// <summary>
    /// Web端图标
    /// </summary>
    [SugarColumn(ColumnDescription = "Web端图标", Length = 200)]
    public string WebIcon { get; set; }

    /// <summary>
    /// Web端路由地址
    /// </summary>
    [SugarColumn(ColumnDescription = "Web端路由地址", Length = 200)]
    public string WebRouter { get; set; }

    /// <summary>
    /// Web端组件地址
    /// </summary>
    [SugarColumn(ColumnDescription = "Web端组件地址", Length = 200)]
    public string WebComponent { get; set; }

    /// <summary>
    /// Web端页面是否在导航栏显示
    /// </summary>
    [SugarColumn(ColumnDescription = "Web端页面是否在导航栏显示")]
    public bool WebTab { get; set; }

    /// <summary>
    /// Web端页面是否缓存
    /// </summary>
    [SugarColumn(ColumnDescription = "Web端页面是否缓存")]
    public bool WebKeepAlive { get; set; }

    /// <summary>
    /// 是否移动端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否移动端")]
    public bool HasMobile { get; set; }

    /// <summary>
    /// 移动端图标
    /// </summary>
    [SugarColumn(ColumnDescription = "移动端图标", Length = 200)]
    public string MobileIcon { get; set; }

    /// <summary>
    /// 移动端路由地址
    /// </summary>
    [SugarColumn(ColumnDescription = "移动端路由地址", Length = 200)]
    public string MobileRouter { get; set; }

    /// <summary>
    /// 内链/外链地址
    /// </summary>
    [SugarColumn(ColumnDescription = "内链/外链地址", Length = 200)]
    public string Link { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    [SugarColumn(ColumnDescription = "是否显示")]
    public bool Visible { get; set; }

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

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}
