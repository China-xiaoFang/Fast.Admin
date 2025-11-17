namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="QueryApplicationDetailOutput"/> 获取应用详情输出
/// </summary>
public class QueryApplicationDetailOutput
{
    /// <summary>
    /// 应用Id
    /// </summary>
    public long AppId { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    public string AppNo { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    public string LogoUrl { get; set; }

    /// <summary>
    /// 主题色
    /// </summary>
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

    /// <summary>
    /// 开放平台信息
    /// </summary>
    public List<QueryApplicationOpenIdDetailDto> OpenIdList { get; set; }

    /// <summary>
    /// <see cref="QueryApplicationOpenIdDetailDto"/> 获取应用开放平台详情Dto
    /// </summary>
    public class QueryApplicationOpenIdDetailDto
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public long RecordId { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 应用类型
        /// </summary>
        public AppEnvironmentEnum AppType { get; set; }

        /// <summary>
        /// 环境类型
        /// </summary>
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
        public int RequestTimeout { get; set; }

        /// <summary>
        /// 请求加密
        /// </summary>
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
        /// 微信 AccessToken 刷新时间
        /// </summary>
        public DateTime? WeChatAccessTokenRefreshTime { get; set; }

        /// <summary>
        /// 微信 JsApi Ticket 刷新时间
        /// </summary>
        public DateTime? WeChatJsApiTicketRefreshTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}