import type { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import type { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * 获取应用标识分页列表输出
 */
export interface QueryApplicationOpenIdPagedOutput {
	/**
	 * 记录Id
	 */
	recordId?: string;
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
	 * 请求超时时间（毫秒）
	 */
	requestTimeout?: number;
	/**
	 * 请求加密
	 */
	requestEncipher?: boolean;
	/**
	 * 微信商户号
	 */
	weChatMerchantNo?: string;
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

