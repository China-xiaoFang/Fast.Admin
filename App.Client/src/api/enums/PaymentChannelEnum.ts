/**
 * Fast.Shared.PaymentChannelEnum 支付渠道枚举
 */
export enum PaymentChannelEnum {
  /**
   * 未知
   */
  None = 0,
  /**
   * 微信
   */
  WeChat = 1,
  /**
   * 支付宝
   */
  Alipay = 2,
  /**
   * 银行卡
   */
  BankCard = 4,
  /**
   * Apple
   */
  ApplePay = 8,
  /**
   * 云闪付
   */
  UnionPay = 16,
}
