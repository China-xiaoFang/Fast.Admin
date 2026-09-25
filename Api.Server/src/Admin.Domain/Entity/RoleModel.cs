// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 角色表Model类
/// </summary>
[SugarTable("Role", "角色表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(RoleName)}", nameof(RoleName), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(RoleCode)}", nameof(RoleCode), OrderByType.Asc, true)]
public class RoleModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [SugarColumn(ColumnDescription = "角色Id", IsPrimaryKey = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    /// <remarks>仅用于初始化默认菜单和按钮，运行时权限以角色菜单、角色按钮关联为准</remarks>
    [SugarColumn(ColumnDescription = "角色类型")]
    public RoleTypeEnum RoleType { get; set; }

    /// <summary>
    /// 是否使用系统菜单
    /// </summary>
    /// <remarks>仅在新增角色时按模板初始化当前应用的菜单和按钮</remarks>
    [SugarColumn(ColumnDescription = "是否使用系统菜单")]
    public bool IsSystemMenu { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "角色名称", Length = 20)]
    public string RoleName { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "角色编码", Length = 30)]
    public string RoleCode { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 数据范围类型
    /// </summary>
    [SugarColumn(ColumnDescription = "数据范围类型")]
    public DataScopeTypeEnum DataScopeType { get; set; }

    /// <summary>
    /// 自定义数据范围部门Id集合
    /// </summary>
    /// <remarks>仅在 <see cref="DataScopeTypeEnum.CustomDept"/> 时生效</remarks>
    [SugarColumn(ColumnDescription = "自定义数据范围部门Id集合", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<long> DataScopeDepartmentIds { get; set; } = [];

    /// <summary>
    /// 可分配的角色Id集合
    /// </summary>
    /// <remarks>普通用户为空时不能分配任何角色；有值时只能分配集合内的角色，且集合不得包含角色自身</remarks>
    [SugarColumn(ColumnDescription = "可分配的角色Id集合", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<long> AssignableRoleIds { get; set; } = [];

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
