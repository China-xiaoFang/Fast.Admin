import type { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * 编辑应用输入
 */
export interface EditApplicationInput {
	/**
	 * 应用Id
	 */
	appId?: string;
	/**
	 * 
	 */
	edition?: EditionEnum;
	/**
	 * 应用名称
	 */
	appName?: string;
	/**
	 * Logo URL
	 */
	logoUrl?: string;
	/**
	 * 主题色
	 */
	themeColor?: string;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 租户Id
	 */
	tenantId?: string;
	/**
	 * 租户名称
	 */
	tenantName?: string;
	/**
	 * 
	 */
	rowVersion?: string;
}

