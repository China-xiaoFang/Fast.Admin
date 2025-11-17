// ReSharper disable once CheckNamespace

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="DepartmentModel"/> 部门表Model类
/// </summary>
[SugarTable("Department", "部门表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(DepartmentName)}", nameof(DepartmentName), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(DepartmentCode)}", nameof(DepartmentCode), OrderByType.Asc, true)]
public class DepartmentModel : IUpdateVersion
{
    /// <summary>
    /// 部门Id
    /// </summary>
    [SugarColumn(ColumnDescription = "部门Id", IsPrimaryKey = true)]
    public long DepartmentId { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id")]
    public long OrgId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id")]
    public long ParentId { get; set; }

    /// <summary>
    /// 父级Id集合
    /// </summary>
    [SugarColumn(ColumnDescription = "父级Id集合", IsJson = true)]
    public List<long> ParentIds { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "部门名称", Length = 20)]
    public string DepartmentName { get; set; }

    /// <summary>
    /// 部门编码
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "部门编码", Length = 50)]
    public string DepartmentCode { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnDescription = "联系人", Length = 20)]
    public string Contacts { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [SugarColumn(ColumnDescription = "电话", Length = 20)]
    public string Phone { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string Remark { get; set; }

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
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "创建时间", CreateTableFieldSort = 993)]
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