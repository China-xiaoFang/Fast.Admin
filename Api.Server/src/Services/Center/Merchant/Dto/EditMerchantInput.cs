namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// <see cref="EditMerchantInput"/> 编辑商户号输入
/// </summary>
public class EditMerchantInput : UpdateVersionInput
{
    /// <summary>
    /// 商户号Id
    /// </summary>
    [LongRequired(ErrorMessage = "商户号Id不能为空")]
    public long MerchantId { get; set; }

    /// <summary>
    /// 商户号类型
    /// </summary>
    [EnumRequired(ErrorMessage = "商户号类型不能为空")]
    public PaymentChannelEnum MerchantType { get; set; }

    /// <summary>
    /// 商户号
    /// </summary>
    [StringRequired(ErrorMessage = "商户号不能为空")]
    public string MerchantNo { get; set; }

    /// <summary>
    /// 商户密钥
    /// </summary>
    public string MerchantSecret { get; set; }

    /// <summary>
    /// 公钥序号
    /// </summary>
    public string PublicSerialNum { get; set; }

    /// <summary>
    /// 公钥
    /// </summary>
    public string PublicKey { get; set; }

    /// <summary>
    /// 证书序号
    /// </summary>
    public string CertSerialNum { get; set; }

    /// <summary>
    /// 证书
    /// </summary>
    public string Cert { get; set; }

    /// <summary>
    /// 证书私钥
    /// </summary>
    public string CertPrivateKey { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}