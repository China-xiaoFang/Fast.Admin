

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="MerchantModel"/> 商户号表Model类
/// </summary>
[SugarTable("Merchant", "商户号表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(MerchantNo)}", nameof(MerchantNo), OrderByType.Asc, true)]
public class MerchantModel : BaseEntity, IUpdateVersion
{
    /// <summary>
    /// 商户号Id
    /// </summary>
    [SugarColumn(ColumnDescription = "商户号Id", IsPrimaryKey = true)]
    public long MerchantId { get; set; }

    /// <summary>
    /// 商户号类型
    /// </summary>
    [SugarColumn(ColumnDescription = "商户号类型")]
    public PaymentChannelEnum MerchantType { get; set; }

    /// <summary>
    /// 商户号
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "商户号", Length = 32)]
    public string MerchantNo { get; set; }

    /// <summary>
    /// 商户密钥
    /// </summary>
    [SugarColumn(ColumnDescription = "商户密钥", Length = 200)]
    public string MerchantSecret { get; set; }

    /// <summary>
    /// 公钥序号
    /// </summary>
    [SugarColumn(ColumnDescription = "平台公钥序号", Length = 200)]
    public string PublicSerialNum { get; set; }

    /// <summary>
    /// 公钥
    /// </summary>
    [SugarColumn(ColumnDescription = "平台公钥", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string PublicKey { get; set; }

    /// <summary>
    /// 证书序号
    /// </summary>
    [SugarColumn(ColumnDescription = "商户证书序号", Length = 200)]
    public string CertSerialNum { get; set; }

    /// <summary>
    /// 证书
    /// </summary>
    [SugarColumn(ColumnDescription = "证书", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string Cert { get; set; }

    /// <summary>
    /// 证书私钥
    /// </summary>
    [SugarColumn(ColumnDescription = "证书私钥", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string CertPrivateKey { get; set; }

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