// ReSharper disable once CheckNamespace

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="RoleModel"/> 组织架构表Model类
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
    [SugarColumn(ColumnDescription = "角色类型")]
    public RoleTypeEnum RoleType { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "角色名称", Length = 20)]
    public string RoleName { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "角色编码", Length = 50)]
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