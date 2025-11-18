

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="EmployeeOrgModel"/> 职员机构表Model类
/// </summary>
[SugarTable("EmployeeOrg", "职员机构表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
public class EmployeeOrgModel : IDatabaseEntity
{
    /// <summary>
    /// 职员Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职员Id", IsPrimaryKey = true)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id")]
    public long OrganizationId { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [SugarColumn(ColumnDescription = "部门Id", IsPrimaryKey = true)]
    public long DepartmentId { get; set; }

    /// <summary>
    /// 是否为主部门
    /// </summary>
    [SugarColumn(ColumnDescription = "是否为主部门")]
    public bool IsPrimary { get; set; }

    /// <summary>
    /// 职位Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职位Id", IsPrimaryKey = true)]
    public long PositionId { get; set; }

    /// <summary>
    /// 职级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职级Id")]
    public long? JobLevelId { get; set; }

    /// <summary>
    /// 主管Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主管Id")]
    public long? LeaderEmployeeId { get; set; }
}