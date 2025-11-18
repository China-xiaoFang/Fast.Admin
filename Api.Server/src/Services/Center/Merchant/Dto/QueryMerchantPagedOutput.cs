namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// <see cref="QueryMerchantPagedOutput"/> 获取商户号分页列表输出
/// </summary>
public class QueryMerchantPagedOutput : PagedOutput
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
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}