// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// <see cref="QueryApplicationDetailOutput"/> 获取应用详情输出
/// </summary>
public class QueryApplicationDetailOutput : PagedOutput
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
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

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
        /// 开放平台密钥
        /// </summary>
        public string OpenSecret { get; set; }

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
        /// 状态栏图片地址
        /// </summary>
        public string StatusBarImageUrl { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Banner图
        /// </summary>
        public List<string> BannerImages { get; set; }

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