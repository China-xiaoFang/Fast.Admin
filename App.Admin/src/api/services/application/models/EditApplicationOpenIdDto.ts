import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * Fast.Center.Service.Application.Dto.EditApplicationInput.EditApplicationOpenIdDto 编辑应用开放平台Dto
 */
export interface EditApplicationOpenIdDto {
  /**
   * 记录Id
   */
  recordId?: number;
  /**
   * 应用标识
   */
  openId?: string;
  /**
   * 
   */
  appType?: AppEnvironmentEnum;
  /**
   * 
   */
  environmentType?: EnvironmentTypeEnum;
  /**
   * 登录组件
   */
  loginComponent?: string;
  /**
   * WebSocket地址
   */
  webSocketUrl?: string;
  /**
   * 请求超时时间（毫秒）
   */
  requestTimeout?: number;
  /**
   * 请求加密
   */
  requestEncipher?: boolean;
  /**
   * 开放平台密钥
   */
  openSecret?: string;
  /**
   * 微信商户号Id
   */
  weChatMerchantId?: number;
  /**
   * 微信商户号
   */
  weChatMerchantNo?: string;
  /**
   * 支付宝商户号Id
   */
  alipayMerchantId?: number;
  /**
   * 支付宝商户号
   */
  alipayMerchantNo?: string;
  /**
   * 备注
   */
  remark?: string;
}

