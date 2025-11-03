// ------------------------------------------------------------------------
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

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="TenantModel"/> 租户信息表Model类
/// </summary>
[SugarTable("Tenant", "租户信息表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(TenantNo)}", nameof(TenantNo), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(TenantName)}", nameof(TenantName), OrderByType.Asc, true)]
public class TenantModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 租户编号
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "租户编号", ColumnDataType = "varchar(11)")]
    public string TenantNo { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    /// <remarks>单号生成前缀</remarks>
    [Required]
    [SugarColumn(ColumnDescription = "租户编码", ColumnDataType = "varchar(5)")]
    public string TenantCode { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "租户名称", Length = 30)]
    public string TenantName { get; set; }

    /// <summary>
    /// 租户简称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "租户简称", Length = 20)]
    public string ShortName { get; set; }

    /// <summary>
    /// 租户英文名称
    /// </summary>
    /// <remarks>根据 <see cref="TenantName"/> 生成的拼音</remarks>
    [Required]
    [SugarColumn(ColumnDescription = "租户英文名称", Length = 100)]
    public string SpellName { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [SugarColumn(ColumnDescription = "版本")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 租户管理员账号Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户管理员账号Id")]
    public long AdminAccountId { get; set; }

    /// <summary>
    /// 租户管理员名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "租户管理员名称", Length = 20)]
    public string AdminName { get; set; }

    /// <summary>
    /// 租户管理员手机
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "租户管理员手机", ColumnDataType = "varchar(11)")]
    public string AdminMobile { get; set; }

    /// <summary>
    /// 租户管理员邮箱
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "租户管理员邮箱", Length = 50)]
    public string AdminEmail { get; set; }

    /// <summary>
    /// 租户管理员电话
    /// </summary>
    [SugarColumn(ColumnDescription = "租户管理员电话", Length = 20)]
    public string AdminPhone { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    [SugarColumn(ColumnDescription = "租户类型")]
    public TenantTypeEnum TenantType { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "LogoUrl", Length = 200)]
    public string LogoUrl { get; set; }

    /// <summary>
    /// 数据库已初始化
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库已初始化")]
    public bool DatabaseInitialized { get; set; }

    /// <summary>
    /// 允许删除数据
    /// </summary>
    [SugarColumn(ColumnDescription = "允许删除数据")]
    public bool AllowDeleteData { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }

    /// <summary>
    /// 数据库信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(MainDatabaseModel.TenantId), nameof(Id))]
    public List<MainDatabaseModel> DatabaseList { get; set; }

    /// <summary>
    /// 租户管理员账号
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(AdminAccountId))]
    public AccountModel TenantAdminAccount { get; set; }
}