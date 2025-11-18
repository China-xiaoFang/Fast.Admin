

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="OrganizationModel"/> 组织架构表Model类
/// </summary>
[SugarTable("Organization", "组织架构表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(OrgName)}", nameof(OrgName), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(OrgCode)}", nameof(OrgCode), OrderByType.Asc, true)]
public class OrganizationModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 组织Id
    /// </summary>
    [SugarColumn(ColumnDescription = "组织Id", IsPrimaryKey = true)]
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
    /// 组织名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "组织名称", Length = 30)]
    public string OrgName { get; set; }

    /// <summary>
    /// 组织编码
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "组织编码", Length = 50)]
    public string OrgCode { get; set; }

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
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }
}