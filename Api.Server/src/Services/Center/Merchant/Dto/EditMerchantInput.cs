namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// <see cref="EditMerchantInput"/> 编辑商户号输入
/// </summary>
public class EditMerchantInput : AddMerchantInput
{
    /// <summary>
    /// 商户号Id
    /// </summary>
    [LongRequired(ErrorMessage = "商户号Id不能为空")]
    public long MerchantId { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }
}