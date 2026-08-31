import type { EditApplicationTemplateIdInput } from "./EditApplicationTemplateIdInput";
import type { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import type { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * 编辑应用标识输入
 */
export interface EditApplicationOpenIdInput {
	/**
	 * 记录Id
	 */
	recordId?: number;
	/**
	 * 应用Id
	 */
	appId?: number;
	/**
	 * 应用标识
	 */
	openId?: string;
	/**
	 * 
	 */
	appType?: AppEnvironmentEnum;
	/**
	 * 开放平台密钥；留空表示保留现有密钥
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
	/**
	 * 模板Id信息
	 */
	templateIdList?: EditApplicationTemplateIdInput[];
	/**
	 * 
	 */
	rowVersion?: number;
}

