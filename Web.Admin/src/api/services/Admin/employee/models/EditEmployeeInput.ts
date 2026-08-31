import type { EmployeeOrgModel } from "./EmployeeOrgModel";
import type { EmployeeRoleModel } from "./EmployeeRoleModel";
import type { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * 编辑职员输入
 */
export interface EditEmployeeInput {
	/**
	 * 职员Id
	 */
	employeeId?: number;
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
	 * 机构信息
	 */
	orgList?: EmployeeOrgModel[];
	/**
	 * 角色信息
	 */
	roleList?: EmployeeRoleModel[];
	/**
	 * 
	 */
	rowVersion?: number;
}

