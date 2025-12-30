import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * Fast.Center.Service.ApplicationOpenId.Dto.QueryApplicationOpenIdDetailOutput 获取应用标识详情输出
 */
export interface QueryApplicationOpenIdDetailOutput {
  /**
   * 记录Id
   */
  recordId?: number;
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 应用名称
   */
  appName?: string;
  /**
   * 应用标识
   */
  openId?: string;
  /**
   * 
   */
  appType?: AppEnvironmentEnum;
  /**
   * 开放平台密钥
   */
  openSecret?: string;
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
   * 状态栏图片地址
   */
  statusBarImageUrl?: string;
  /**
   * 联系电话
   */
  contactPhone?: string;
  /**
   * 纬度
   */
  latitude?: number;
  /**
   * 经度
   */
  longitude?: number;
  /**
   * 地址
   */
  address?: string;
  /**
   * Banner图
   */
  bannerImages?: Array<string>;
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

