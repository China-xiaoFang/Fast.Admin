using Fast.Runtime;
using Microsoft.AspNetCore.Http;



namespace Fast.Center.Entity;

/// <summary>
/// <see cref="PayRecordModel"/> 支付记录表Model类
/// </summary>
[SugarTable("PayRecord", "支付记录表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(OrderNo)}", nameof(OrderNo), OrderByType.Desc, true)]
public class PayRecordModel : IUpdateVersion
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
    /// 收款商户号
    /// </summary>
    [SugarColumn(ColumnDescription = "收款商户号", Length = 32)]
    public string MerchantNo { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "订单号", Length = 20)]
    public string OrderNo { get; set; }

    /// <summary>
    /// 业务订单Id
    /// </summary>
    [SugarColumn(ColumnDescription = "业务订单Id")]
    public long BizOrderId { get; set; }

    /// <summary>
    /// 支付渠道
    /// </summary>
    [SugarColumn(ColumnDescription = "支付渠道")]
    public PaymentChannelEnum PayChannel { get; set; }

    /// <summary>
    /// 唯一用户标识
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "唯一用户标识", Length = 28)]
    public string OpenId { get; set; }

    /// <summary>
    /// 统一用户标识
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "统一用户标识", Length = 28)]
    public string UnionId { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)")]
    public string Mobile { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    [SugarColumn(ColumnDescription = "订单金额", Length = 18, DecimalDigits = 2)]
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// 回调地址
    /// </summary>
    [SugarColumn(ColumnDescription = "回调地址", Length = 200)]
    public string NotifyUrl { get; set; }

    /// <summary>
    /// 是否支付
    /// </summary>
    [SugarColumn(ColumnDescription = "是否支付")]
    public YesOrNotEnum IsPay { get; set; }

    /// <summary>
    /// 支付金额
    /// </summary>
    [SugarColumn(ColumnDescription = "支付金额", Length = 18, DecimalDigits = 2)]
    public decimal? PaymentAmount { get; set; }

    /// <summary>
    /// 交易流水号
    /// </summary>
    [SugarColumn(ColumnDescription = "交易流水号", Length = 128)]
    public string TransactionId { get; set; }

    /// <summary>
    /// 支付时间
    /// </summary>
    [SugarColumn(ColumnDescription = "支付时间")]
    public DateTime? PaymentTime { get; set; }

    /// <summary>
    /// 退款金额
    /// </summary>
    [SugarColumn(ColumnDescription = "退款金额", Length = 18, DecimalDigits = 2)]
    public decimal? RefundAmount { get; set; }

    /// <summary>
    /// 退款时间
    /// </summary>
    [SugarColumn(ColumnDescription = "退款时间")]
    public DateTime? RefundTime { get; set; }

    /// <summary>
    /// 订单过期时间
    /// </summary>
    [SugarColumn(ColumnDescription = "订单过期时间")]
    public DateTime ExpireTime { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    [SugarColumn(ColumnDescription = "设备", Length = 50, CreateTableFieldSort = 983)]
    public string Device { get; set; }

    /// <summary>
    /// 操作系统（版本）
    /// </summary>
    [SugarColumn(ColumnDescription = "操作系统（版本）", Length = 50, CreateTableFieldSort = 984)]
    public string OS { get; set; }

    /// <summary>
    /// 浏览器（版本）
    /// </summary>
    [SugarColumn(ColumnDescription = "浏览器（版本）", Length = 50, CreateTableFieldSort = 985)]
    public string Browser { get; set; }

    /// <summary>
    /// 省份
    /// </summary>
    [SugarColumn(ColumnDescription = "省份", Length = 20, CreateTableFieldSort = 986)]
    public string Province { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    [SugarColumn(ColumnDescription = "城市", Length = 20, CreateTableFieldSort = 987)]
    public string City { get; set; }

    /// <summary>
    /// Ip
    /// </summary>
    [SugarColumn(ColumnDescription = "Ip", Length = 15, CreateTableFieldSort = 988)]
    public string Ip { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "创建时间", CreateTableFieldSort = 993)]
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", CreateTableFieldSort = 996)]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public long RowVersion { get; set; }

    /// <summary>
    /// 记录表创建
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/> 请求上下文</param>
    public void RecordCreate(HttpContext httpContext)
    {
        var userAgentInfo = httpContext.RequestUserAgentInfo();
        var wanInfo = httpContext.RemoteIpv4Info();

        Device = userAgentInfo.Device;
        OS = userAgentInfo.OS;
        Browser = userAgentInfo.Browser;
        Province = wanInfo.Province;
        City = wanInfo.City;
        Ip = wanInfo.Ip;
    }
}