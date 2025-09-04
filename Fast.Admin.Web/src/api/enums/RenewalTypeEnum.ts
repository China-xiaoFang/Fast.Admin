/**
 * Fast.Common.RenewalTypeEnum 续费类型枚举
 */
export enum RenewalTypeEnum {
  /**
   * 初次开通
   */
  Activation = 1,
  /**
   * 手动续费
   */
  ManualRenewal = 2,
  /**
   * 自动续费
   */
  AutoRenewal = 4,
  /**
   * 试用
   */
  Trial = 8,
  /**
   * 试用
   */
  Gifted = 16,
  /**
   * 补偿赠送
   */
  Compensation = 32,
  /**
   * 内部开通
   */
  Internal = 64,
}
