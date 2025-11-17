namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="EditApplicationInput"/> 编辑应用输入
/// </summary>
public class EditApplicationInput : AddApplicationInput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [LongRequired(ErrorMessage = "应用Id不能为空")]
    public long AppId { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [LongRequired(ErrorMessage = "更新版本控制字段不能为空")]
    public long RowVersion { get; set; }

    /// <summary>
    /// 开放平台信息
    /// </summary>
    public List<EditApplicationOpenIdDto> OpenIdList { get; set; }

    /// <summary>
    /// <see cref="EditApplicationOpenIdDto"/> 编辑应用开放平台Dto
    /// </summary>
    public class EditApplicationOpenIdDto
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public long? RecordId { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        [StringRequired(ErrorMessage = "应用标识不能为空")]
        public string OpenId { get; set; }

        /// <summary>
        /// 应用类型
        /// </summary>
        [EnumRequired(ErrorMessage = "应用类型不能为空")]
        public AppEnvironmentEnum AppType { get; set; }

        /// <summary>
        /// 环境类型
        /// </summary>
        [EnumRequired(ErrorMessage = "环境类型不能为空")]
        public EnvironmentTypeEnum EnvironmentType { get; set; }

        /// <summary>
        /// 登录组件
        /// </summary>
        public string LoginComponent { get; set; }

        /// <summary>
        /// WebSocket地址
        /// </summary>
        public string WebSocketUrl { get; set; }

        /// <summary>
        /// 请求超时时间（毫秒）
        /// </summary>
        [IntRequired(ErrorMessage = "请求超时时间不能为空")]
        public int RequestTimeout { get; set; }

        /// <summary>
        /// 请求加密
        /// </summary>
        [Required(ErrorMessage = "请求加密不能为空")]
        public bool RequestEncipher { get; set; }

        /// <summary>
        /// 开放平台密钥
        /// </summary>
        public string OpenSecret { get; set; }

        /// <summary>
        /// 微信商户号Id
        /// </summary>
        public long? WeChatMerchantId { get; set; }

        /// <summary>
        /// 微信商户号
        /// </summary>
        public string WeChatMerchantNo { get; set; }

        /// <summary>
        /// 支付宝商户号Id
        /// </summary>
        public long? AlipayMerchantId { get; set; }

        /// <summary>
        /// 支付宝商户号
        /// </summary>
        public string AlipayMerchantNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}