namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// <see cref="QueryMerchantPagedInput"/> 获取商户号分页列表输入
/// </summary>
public class QueryMerchantPagedInput : PagedInput
{
    /// <summary>
    /// 商户号类型
    /// </summary>
    public PaymentChannelEnum? MerchantType { get; set; }
}