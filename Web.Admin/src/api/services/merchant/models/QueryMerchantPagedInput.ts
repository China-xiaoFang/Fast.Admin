import { PagedInput } from "fast-element-plus";
import { PaymentChannelEnum } from "@/api/enums/PaymentChannelEnum";

/**
 * Fast.Center.Service.Merchant.Dto.QueryMerchantPagedInput 获取商户号分页列表输入
 */
export interface QueryMerchantPagedInput extends PagedInput  {
  /**
   * 
   */
  merchantType?: PaymentChannelEnum;
}

