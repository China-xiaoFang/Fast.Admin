// ReSharper disable once CheckNamespace

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="EmployeeModel"/> 职员表Model类
/// </summary>
/// <remarks>这里的主键Id 和 Center 库 TenantUser 表主键Id 一致</remarks>
[SugarTable("Employee", "职员表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
[SugarIndex($"IX_{{table}}_{nameof(EmployeeNo)}", nameof(EmployeeNo), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(Mobile)}", nameof(Mobile), OrderByType.Asc, true)]
public class EmployeeModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 职员Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职员Id", IsPrimaryKey = true)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 登录用户Id
    /// </summary>
    /// <remarks>绑定 Center 库 TenantUser 表主键Id</remarks>
    [SugarColumn(ColumnDescription = "登录用户Id")]
    public long? UserId { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    /// <remarks>2024010101 ~ 20240101999</remarks>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "工号", ColumnDataType = "varchar(11)")]
    public string EmployeeNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "姓名", Length = 20)]
    public string EmployeeName { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)")]
    public string Mobile { get; set; }

    /// <summary>
    /// 证件照
    /// </summary>
    [SugarColumn(ColumnDescription = "证件照", Length = 200)]
    public string IdPhoto { get; set; }

    /// <summary>
    /// 入职日期
    /// </summary>
    [SugarColumn(ColumnDescription = "入职日期")]
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// 离职日期
    /// </summary>
    [SugarColumn(ColumnDescription = "离职日期")]
    public DateTime? ResignDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    [SugarColumn(ColumnDescription = "离职原因", Length = 200)]
    public string ResignReason { get; set; }

    /// <summary>
    /// 民族
    /// </summary>
    [SugarColumn(ColumnDescription = "民族")]
    public NationEnum? Nation { get; set; }

    /// <summary>
    /// 籍贯
    /// </summary>
    [SugarColumn(ColumnDescription = "籍贯", Length = 50)]
    public string NativePlace { get; set; }

    /// <summary>
    /// 家庭地址
    /// </summary>
    [SugarColumn(ColumnDescription = "家庭地址", Length = 200)]
    public string FamilyAddress { get; set; }

    /// <summary>
    /// 通信地址
    /// </summary>
    [SugarColumn(ColumnDescription = "通信地址", Length = 200)]
    public string MailingAddress { get; set; }

    /// <summary>
    /// 证件类型
    /// </summary>
    [SugarColumn(ColumnDescription = "证件类型")]
    public IdTypeEnum? IdType { get; set; }

    /// <summary>
    /// 证件号码
    /// </summary>
    [SugarColumn(ColumnDescription = "证件号码", Length = 50)]
    public string IdNumber { get; set; }

    /// <summary>
    /// 文件程度
    /// </summary>
    [SugarColumn(ColumnDescription = "文件程度")]
    public EducationLevelEnum? EducationLevel { get; set; }

    /// <summary>
    /// 政治面貌
    /// </summary>
    [SugarColumn(ColumnDescription = "政治面貌")]
    public PoliticalStatusEnum? PoliticalStatus { get; set; }

    /// <summary>
    /// 毕业学院
    /// </summary>
    [SugarColumn(ColumnDescription = "毕业学院", Length = 50)]
    public string GraduationCollege { get; set; }

    /// <summary>
    /// 学历
    /// </summary>
    [SugarColumn(ColumnDescription = "学历")]
    public AcademicQualificationsEnum? AcademicQualifications { get; set; }

    /// <summary>
    /// 学制
    /// </summary>
    [SugarColumn(ColumnDescription = "学制")]
    public AcademicSystemEnum? AcademicSystem { get; set; }

    /// <summary>
    /// 学位
    /// </summary>
    [SugarColumn(ColumnDescription = "学位")]
    public DegreeEnum? Degree { get; set; }

    /// <summary>
    /// 家庭电话
    /// </summary>
    [SugarColumn(ColumnDescription = "家庭电话", Length = 20)]
    public string FamilyPhone { get; set; }

    /// <summary>
    /// 办公电话
    /// </summary>
    [SugarColumn(ColumnDescription = "办公电话", Length = 20)]
    public string OfficePhone { get; set; }

    /// <summary>
    /// 紧急联系人
    /// </summary>
    [SugarColumn(ColumnDescription = "紧急联系人", Length = 20)]
    public string EmergencyContact { get; set; }

    /// <summary>
    /// 紧急联系电话
    /// </summary>
    [SugarColumn(ColumnDescription = "紧急联系电话", Length = 20)]
    public string EmergencyPhone { get; set; }

    /// <summary>
    /// 紧急联系地址
    /// </summary>
    [SugarColumn(ColumnDescription = "紧急联系地址", Length = 200)]
    public string EmergencyAddress { get; set; }

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