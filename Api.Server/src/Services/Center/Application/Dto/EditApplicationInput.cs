namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="EditApplicationInput"/> 编辑应用输入
/// </summary>
public class EditApplicationInput : UpdateVersionInput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [LongRequired(ErrorMessage = "应用Id不能为空")]
    public long AppId { get; set; }
    /// <summary>
    /// 版本
    /// </summary>
    [EnumRequired(ErrorMessage = "版本不能为空")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [StringRequired(ErrorMessage = "应用名称不能为空")]
    public string AppName { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    [StringRequired(ErrorMessage = "LogoUrl不能为空")]
    public string LogoUrl { get; set; }

    /// <summary>
    /// 主题色
    /// </summary>
    [StringRequired(ErrorMessage = "主题色不能为空")]
    public string ThemeColor { get; set; }

    /// <summary>
    /// ICP备案号
    /// </summary>
    public string ICPSecurityCode { get; set; }

    /// <summary>
    /// 公安备案号
    /// </summary>
    public string PublicSecurityCode { get; set; }

    /// <summary>
    /// 用户协议
    /// </summary>
    public string UserAgreement { get; set; }

    /// <summary>
    /// 隐私协议
    /// </summary>
    public string PrivacyAgreement { get; set; }

    /// <summary>
    /// 服务协议
    /// </summary>
    public string ServiceAgreement { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

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