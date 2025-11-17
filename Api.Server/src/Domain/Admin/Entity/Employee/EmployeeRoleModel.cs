// ReSharper disable once CheckNamespace

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="EmployeeOrgModel"/> 职员角色表Model类
/// </summary>
[SugarTable("EmployeeRole", "职员角色表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
public class EmployeeRoleModel : IDatabaseEntity
{
    /// <summary>
    /// 职员Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职员Id", IsPrimaryKey = true)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    [SugarColumn(ColumnDescription = "角色Id", IsPrimaryKey = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(RoleId), nameof(RoleModel.RoleId))]
    public RoleModel Role { get; set; }
}