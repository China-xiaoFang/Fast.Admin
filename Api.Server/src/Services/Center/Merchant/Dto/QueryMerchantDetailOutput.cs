namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// <see cref="QueryMerchantDetailOutput"/> 获取商户号详情输出
/// </summary>
public class QueryMerchantDetailOutput
{
    /// <summary>
    /// 商户号Id
    /// </summary>
    public long MerchantId { get; set; }

    /// <summary>
    /// 商户号类型
    /// </summary>
    public PaymentChannelEnum MerchantType { get; set; }

    /// <summary>
    /// 商户号
    /// </summary>
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

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    public string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    public long RowVersion { get; set; }
}