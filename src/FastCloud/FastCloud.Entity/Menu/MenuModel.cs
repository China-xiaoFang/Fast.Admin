﻿// ------------------------------------------------------------------------
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

// ReSharper disable once CheckNamespace

namespace Fast.FastCloud.Entity;

/// <summary>
/// <see cref="MenuModel"/> 菜单表Model类
/// </summary>
[SugarTable("Menu", "菜单表")]
[SugarDbType(DatabaseTypeEnum.FastCloud)]
[SugarIndex($"IX_{{table}}_{nameof(MenuCode)}", nameof(MenuCode), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(MenuName)}", nameof(ModuleId), OrderByType.Asc, nameof(MenuName), OrderByType.Asc, true)]
public class MenuModel : SnowflakeKeyEntity
{
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
    /// 模块Id
    /// </summary>
    [SugarColumn(ColumnDescription = "模块Id")]
    public long ModuleId { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单编码", Length = 50, IsNullable = false)]
    public string MenuCode { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单名称", Length = 20, IsNullable = false)]
    public string MenuName { get; set; }

    /// <summary>
    /// 菜单标题
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单标题", Length = 20, IsNullable = false)]
    public string MenuTitle { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id")]
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单类型")]
    public MenuTypeEnum MenuType { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否桌面端")]
    public bool HasDesktop { get; set; }

    /// <summary>
    /// 桌面端图标
    /// </summary>
    [SugarColumn(ColumnDescription = "桌面端图标", Length = 20)]
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
    [SugarColumn(ColumnDescription = "Web端图标", Length = 20)]
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
    /// 是否移动端
    /// </summary>
    [SugarColumn(ColumnDescription = "是否移动端")]
    public bool HasMobile { get; set; }

    /// <summary>
    /// 移动端图标
    /// </summary>
    [SugarColumn(ColumnDescription = "移动端图标", Length = 20)]
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
    public YesOrNotEnum Visible { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 创建者用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户Id", CreateTableFieldSort = 991)]
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户名称", Length = 20, CreateTableFieldSort = 992)]
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", CreateTableFieldSort = 993)]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "更新者用户Id", CreateTableFieldSort = 994)]
    public long? UpdatedUserId { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "更新者用户名称", Length = 20, CreateTableFieldSort = 995)]
    public string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", CreateTableFieldSort = 996)]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}