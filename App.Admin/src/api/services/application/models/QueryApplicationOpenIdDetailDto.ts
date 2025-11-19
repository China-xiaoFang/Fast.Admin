import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * Fast.Center.Service.Application.Dto.QueryApplicationDetailOutput.QueryApplicationOpenIdDetailDto 获取应用开放平台详情Dto
 */
export interface QueryApplicationOpenIdDetailDto {
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
   * 微信 AccessToken 刷新时间
   */
  weChatAccessTokenRefreshTime?: Date;
  /**
   * 微信 JsApi Ticket 刷新时间
   */
  weChatJsApiTicketRefreshTime?: Date;
  /**
   * 备注
   */
  remark?: string;
}

