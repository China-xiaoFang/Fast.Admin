namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// <see cref="MerchantIdInput"/> 商户号Id输入
/// </summary>
public class MerchantIdInput : UpdateVersionInput
{
    /// <summary>
    /// 商户号Id
    /// </summary>
    [LongRequired(ErrorMessage = "商户号Id不能为空")]
    public long MerchantId { get; set; }
}