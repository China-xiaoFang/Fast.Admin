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
   * 
   */
  departmentId?: number;
  /**
   * 
   */
  departmentName?: string;
  /**
   * 
   */
  createdUserName?: string;
  /**
   * 
   */
  createdTime?: Date;
  /**
   * 
   */
  updatedUserName?: string;
  /**
   * 
   */
  updatedTime?: Date;
  /**
   * 
   */
  rowVersion?: number;
}

