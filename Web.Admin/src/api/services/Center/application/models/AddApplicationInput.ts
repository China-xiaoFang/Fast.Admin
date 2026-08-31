import type { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * 添加应用输入
 */
export interface AddApplicationInput {
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
	tenantId?: number;
	/**
	 * 租户名称
	 */
	tenantName?: string;
}

