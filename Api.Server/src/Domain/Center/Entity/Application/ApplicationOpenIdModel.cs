

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="ApplicationOpenIdModel"/> 应用标识表Model类
/// </summary>
[SugarTable("ApplicationOpenId", "应用标识表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(OpenId)}", nameof(OpenId), OrderByType.Asc, true)]
public class ApplicationOpenIdModel : BaseEntity
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [SugarColumn(ColumnDescription = "记录Id", IsPrimaryKey = true)]
    public long RecordId { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id")]
    public long AppId { get; set; }

    /// <summary>
    /// 应用标识
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "应用标识", Length = 50)]
    public string OpenId { get; set; }

    /// <summary>
    /// 应用类型
    /// </summary>
    [SugarColumn(ColumnDescription = "应用类型")]
    public AppEnvironmentEnum AppType { get; set; }

    /// <summary>
    /// 环境类型
    /// </summary>
    [SugarColumn(ColumnDescription = "环境类型")]
    public EnvironmentTypeEnum EnvironmentType { get; set; }

    /// <summary>
    /// 登录组件
    /// </summary>
    [SugarColumn(ColumnDescription = "WebSocket地址", Length = 50)]
    public string LoginComponent { get; set; }

    /// <summary>
    /// WebSocket地址
    /// </summary>
    [SugarColumn(ColumnDescription = "WebSocket地址", Length = 50)]
    public string WebSocketUrl { get; set; }

    /// <summary>
    /// 请求超时时间（毫秒）
    /// </summary>
    [SugarColumn(ColumnDescription = "请求超时时间（毫秒）")]
    public int RequestTimeout { get; set; }

    /// <summary>
    /// 请求加密
    /// </summary>
    [SugarColumn(ColumnDescription = "请求加密")]
    public bool RequestEncipher { get; set; }

    /// <summary>
    /// 开放平台密钥
    /// </summary>
    [SugarColumn(ColumnDescription = "开放平台密钥", Length = 32)]
    public string OpenSecret { get; set; }

    /// <summary>
    /// 微信商户号Id
    /// </summary>
    [SugarColumn(ColumnDescription = "微信商户号Id")]
    public long? WeChatMerchantId { get; set; }

    /// <summary>
    /// 微信商户号
    /// </summary>
    [SugarColumn(ColumnDescription = "微信商户号", Length = 32)]
    public string WeChatMerchantNo { get; set; }

    /// <summary>
    /// 支付宝商户号Id
    /// </summary>
    [SugarColumn(ColumnDescription = "支付宝商户号Id")]
    public long? AlipayMerchantId { get; set; }

    /// <summary>
    /// 支付宝商户号
    /// </summary>
    [SugarColumn(ColumnDescription = "支付宝商户号", Length = 32)]
    public string AlipayMerchantNo { get; set; }

    /// <summary>
    /// 微信 AccessToken
    /// </summary>
    [SugarColumn(ColumnDescription = "微信 AccessToken", Length = 512)]
    public string WeChatAccessToken { get; set; }

    /// <summary>
    /// 微信 AccessToken 过期时间，单位秒
    /// </summary>
    [SugarColumn(ColumnDescription = "微信 AccessToken 过期时间，单位秒")]
    public int? WeChatAccessTokenExpiresIn { get; set; }

    /// <summary>
    /// 微信 AccessToken 刷新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "微信 AccessToken 刷新时间")]
    public DateTime? WeChatAccessTokenRefreshTime { get; set; }

    /// <summary>
    /// 微信 JsApi Ticket
    /// </summary>
    [SugarColumn(ColumnDescription = "微信 JsApi Ticket", Length = 200)]
    public string WeChatJsApiTicket { get; set; }

    /// <summary>
    /// 微信 JsApi Ticket 过期时间，单位秒
    /// </summary>
    [SugarColumn(ColumnDescription = "微信 JsApi Ticket 过期时间，单位秒")]
    public int? WeChatJsApiTicketExpiresIn { get; set; }

    /// <summary>
    /// 微信 JsApi Ticket 刷新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "微信 JsApi Ticket 刷新时间")]
    public DateTime? WeChatJsApiTicketRefreshTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 200)]
    public string Remark { get; set; }

    /// <summary>
    /// 应用
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(AppId), nameof(ApplicationModel.AppId))]
    public ApplicationModel Application { get; set; }
}