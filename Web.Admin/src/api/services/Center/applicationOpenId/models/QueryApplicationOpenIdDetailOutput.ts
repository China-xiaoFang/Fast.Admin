import type { EditApplicationTemplateIdInput } from "./EditApplicationTemplateIdInput";
import type { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import type { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * 获取应用标识详情输出
 */
export interface QueryApplicationOpenIdDetailOutput {
	/**
	 * 记录Id
	 */
	recordId?: string;
	/**
	 * 应用Id
	 */
	appId?: string;
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
	 * 微信商户号Id
	 */
	weChatMerchantId?: string;
	/**
	 * 微信商户号
	 */
	weChatMerchantNo?: string;
	/**
	 * 支付宝商户号Id
	 */
	alipayMerchantId?: string;
	/**
	 * 支付宝商户号
	 */
	alipayMerchantNo?: string;
	/**
	 * 微信 AccessToken 刷新时间
	 */
	weChatAccessTokenRefreshTime?: string;
	/**
	 * 微信 JSAPI Ticket 刷新时间
	 */
	weChatJsApiTicketRefreshTime?: string;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 模板Id信息
	 */
	templateIdList?: EditApplicationTemplateIdInput[];
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
	createdTime?: string;
	/**
	 * 
	 */
	updatedUserName?: string;
	/**
	 * 
	 */
	updatedTime?: string;
	/**
	 * 
	 */
	rowVersion?: string;
}

