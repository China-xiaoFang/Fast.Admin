import { PaymentChannelEnum } from "@/api/enums/PaymentChannelEnum";

/**
 * Fast.Center.Service.Merchant.Dto.QueryMerchantDetailOutput 获取商户号详情输出
 */
export interface QueryMerchantDetailOutput {
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
   * 商户密钥
   */
  merchantSecret?: string;
  /**
   * 公钥序号
   */
  publicSerialNum?: string;
  /**
   * 公钥
   */
  publicKey?: string;
  /**
   * 证书序号
   */
  certSerialNum?: string;
  /**
   * 证书
   */
  cert?: string;
  /**
   * 证书私钥
   */
  certPrivateKey?: string;
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
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}

