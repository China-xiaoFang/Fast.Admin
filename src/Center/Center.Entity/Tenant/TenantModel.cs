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
[SugarIndex($"IX_{{table}}_{nameof(TenantNo)}", nameof(TenantNo), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(ChName)}", nameof(ChName), OrderByType.Asc, true)]
[SugarIndex($"IX_{{table}}_{nameof(Secret)}", nameof(Secret), OrderByType.Asc, true)]
public class TenantModel : SnowflakeKeyEntity
{
    /// <summary>
    /// 租户编号
    /// </summary>
    [SugarColumn(ColumnDescription = "租户编号", ColumnDataType = "Nvarchar(32)", IsNullable = false)]
    public string TenantNo { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态", ColumnDataType = "tinyint", IsNullable = false)]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 租户公司中文名称
    /// </summary>
    [SugarColumn(ColumnDescription = "租户公司中文名称", ColumnDataType = "Nvarchar(50)", IsNullable = false)]
    public string ChName { get; set; }

    /// <summary>
    /// 租户公司英文名称（拼音）
    /// </summary>
    [SugarColumn(ColumnDescription = "租户公司英文名称", ColumnDataType = "Nvarchar(50)", IsNullable = false)]
    public string EnName { get; set; }

    /// <summary>
    /// 租户公司中文简称
    /// </summary>
    [SugarColumn(ColumnDescription = "租户公司中文简称", ColumnDataType = "Nvarchar(30)", IsNullable = false)]
    public string ChShortName { get; set; }

    /// <summary>
    /// 租户公司英文简称（拼音）
    /// </summary>
    [SugarColumn(ColumnDescription = "租户公司英文简称", ColumnDataType = "Nvarchar(30)", IsNullable = false)]
    public string EnShortName { get; set; }

    /// <summary>
    /// 租户密钥
    /// </summary>
    /// <remarks>32位长度</remarks>
    [SugarColumn(ColumnDescription = "租户密钥", ColumnDataType = "Nvarchar(32)", IsNullable = false)]
    public string Secret { get; set; }

    /// <summary>
    /// 租户公钥
    /// </summary>
    [SugarColumn(ColumnDescription = "租户公钥", ColumnDataType = "Nvarchar(MAX)", IsNullable = false)]
    public string PublicKey { get; set; }

    /// <summary>
    /// 租户私钥
    /// </summary>
    [SugarColumn(ColumnDescription = "租户私钥", ColumnDataType = "Nvarchar(MAX)", IsNullable = false)]
    public string PrivateKey { get; set; }

    /// <summary>
    /// 租户管理员名称
    /// </summary>
    [SugarColumn(ColumnDescription = "租户管理员名称", ColumnDataType = "Nvarchar(20)", IsNullable = false)]
    public string AdminName { get; set; }

    /// <summary>
    /// 租户管理员手机
    /// </summary>
    [SugarColumn(ColumnDescription = "租户管理员手机", ColumnDataType = "varchar(11)", IsNullable = false)]
    public string AdminMobile { get; set; }

    /// <summary>
    /// 租户管理员邮箱
    /// </summary>
    [SugarColumn(ColumnDescription = "租户管理员邮箱", ColumnDataType = "Nvarchar(50)", IsNullable = false)]
    public string AdminEmail { get; set; }

    /// <summary>
    /// 租户电话
    /// </summary>
    [SugarColumn(ColumnDescription = "租户电话", ColumnDataType = "Nvarchar(20)", IsNullable = false)]
    public string Mobile { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    [SugarColumn(ColumnDescription = "租户类型", ColumnDataType = "tinyint", IsNullable = false)]
    public TenantTypeEnum TenantType { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    [SugarColumn(ColumnDescription = "LogoUrl", ColumnDataType = "Nvarchar(max)", IsNullable = false)]
    public string LogoUrl { get; set; }

    /// <summary>
    /// 创建者用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户Id", IsNullable = true, CreateTableFieldSort = 991)]
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户名称", ColumnDataType = "Nvarchar(20)", IsNullable = true, CreateTableFieldSort = 992)]
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarSearchTime,
     SugarColumn(ColumnDescription = "创建时间", ColumnDataType = "datetimeoffset", IsNullable = true, CreateTableFieldSort = 993)]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "更新者用户Id", IsNullable = true, CreateTableFieldSort = 994)]
    public long? UpdatedUserId { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "更新者用户名称", ColumnDataType = "Nvarchar(20)", IsNullable = true, CreateTableFieldSort = 995)]
    public string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", ColumnDataType = "datetimeoffset", IsNullable = true, CreateTableFieldSort = 996)]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, IsNullable = false,
        CreateTableFieldSort = 998)]
    public long UpdatedVersion { get; set; }

    /// <summary>
    /// App授权信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(SysTenantAppInfoModel.TenantId), nameof(Id))]
    public List<SysTenantAppInfoModel> AppList { get; set; }

    /// <summary>
    /// 数据库信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(SysTenantMainDatabaseModel.TenantId), nameof(Id))]
    public List<SysTenantMainDatabaseModel> DatabaseList { get; set; }

    /// <summary>
    /// 系统管理员账号
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public SysTenantAccountModel SystemAdminAccount { get; set; }

    /// <summary>
    /// 租户管理员账号
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public SysTenantAccountModel TenantAdminAccount { get; set; }
}