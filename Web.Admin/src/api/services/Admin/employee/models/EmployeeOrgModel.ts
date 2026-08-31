/**
 * 职员机构表Model类
 */
export interface EmployeeOrgModel {
	/**
	 * 职员Id
	 */
	employeeId?: string;
	/**
	 * 机构Id
	 */
	orgId?: string;
	/**
	 * 机构名称
	 */
	orgName?: string;
	/**
	 * 机构名称
	 */
	orgNames?: string[];
	/**
	 * 部门Id
	 */
	departmentId?: string;
	/**
	 * 部门名称
	 */
	departmentName?: string;
	/**
	 * 部门名称
	 */
	departmentNames?: string[];
	/**
	 * 是否为主部门
	 */
	isPrimary?: boolean;
	/**
	 * 职位Id
	 */
	positionId?: string;
	/**
	 * 职位名称
	 */
	positionName?: string;
	/**
	 * 职级Id
	 */
	jobLevelId?: string;
	/**
	 * 职级名称
	 */
	jobLevelName?: string;
	/**
	 * 是否为负责人
	 */
	isPrincipal?: boolean;
}

