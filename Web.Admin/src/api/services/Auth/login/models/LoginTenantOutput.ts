import type { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import type { EditionEnum } from "@/api/enums/EditionEnum";
import type { UserTypeEnum } from "@/api/enums/UserTypeEnum";

/**
 * 登录租户输出
 */
export interface LoginTenantOutput {
	/**
	 * 用户Key
	 */
	userKey?: string;
	/**
	 * 租户名称
	 */
	tenantName?: string;
	/**
	 * 租户简称
	 */
	shortName?: string;
	/**
	 * 租户英文名称
	 */
	spellName?: string;
	/**
	 * 
	 */
	edition?: EditionEnum;
	/**
	 * Logo URL
	 */
	logoUrl?: string;
	/**
	 * 工号
	 */
	employeeNo?: string;
	/**
	 * 姓名
	 */
	employeeName?: string;
	/**
	 * 证件照
	 */
	idPhoto?: string;
	/**
	 * 部门Id
	 */
	departmentId?: number;
	/**
	 * 部门名称
	 */
	departmentName?: string;
	/**
	 * 
	 */
	userType?: UserTypeEnum;
	/**
	 * 
	 */
	status?: CommonStatusEnum;
}

