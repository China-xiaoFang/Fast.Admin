import type { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * 获取应用分页列表输出
 */
export interface QueryApplicationPagedOutput {
	/**
	 * 应用Id
	 */
	appId?: number;
	/**
	 * 
	 */
	edition?: EditionEnum;
	/**
	 * 应用编号
	 */
	appNo?: string;
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
	 * 租户名称
	 */
	tenantName?: string;
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
	rowVersion?: number;
}

