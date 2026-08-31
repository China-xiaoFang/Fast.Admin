import type { EmployeeRoleModel } from "./EmployeeRoleModel";
import type { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * 添加职员输入
 */
export interface AddEmployeeInput {
	/**
	 * 姓名
	 */
	employeeName?: string;
	/**
	 * 手机
	 */
	mobile?: string;
	/**
	 * 邮箱
	 */
	email?: string;
	/**
	 * 
	 */
	sex?: GenderEnum;
	/**
	 * 证件照
	 */
	idPhoto?: string;
	/**
	 * 入职日期
	 */
	entryDate?: string;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 机构Id
	 */
	orgId?: number;
	/**
	 * 机构名称
	 */
	orgName?: string;
	/**
	 * 部门Id
	 */
	departmentId?: number;
	/**
	 * 部门名称
	 */
	departmentName?: string;
	/**
	 * 职位Id
	 */
	positionId?: number;
	/**
	 * 职位名称
	 */
	positionName?: string;
	/**
	 * 职级Id
	 */
	jobLevelId?: number;
	/**
	 * 职级名称
	 */
	jobLevelName?: string;
	/**
	 * 是否为负责人
	 */
	isPrincipal?: boolean;
	/**
	 * 角色信息
	 */
	roleList?: EmployeeRoleModel[];
}

