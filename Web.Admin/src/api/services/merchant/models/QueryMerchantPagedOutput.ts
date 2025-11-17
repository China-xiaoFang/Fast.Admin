import { PaymentChannelEnum } from "@/api/enums/PaymentChannelEnum";

/**
 * Fast.Center.Service.Merchant.Dto.QueryMerchantPagedOutput 获取商户号分页列表输出
 */
export interface QueryMerchantPagedOutput {
  /**
   * 商户号Id
   */
  merchantId?: number;
  /**
   * 
   */
  merchantType?: PaymentChannelEnum;
  /**
   * 商户号
   */
  merchantNo?: string;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 创建者用户名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新者用户名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
}

