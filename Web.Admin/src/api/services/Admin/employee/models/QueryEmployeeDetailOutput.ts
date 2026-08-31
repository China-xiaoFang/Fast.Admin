import type { EmployeeOrgModel } from "./EmployeeOrgModel";
import type { EmployeeRoleModel } from "./EmployeeRoleModel";
import type { EmployeeStatusEnum } from "@/api/enums/EmployeeStatusEnum";
import type { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * 获取职员详情输出
 */
export interface QueryEmployeeDetailOutput {
	/**
	 * 职员Id
	 */
	employeeId?: string;
	/**
	 * 工号
	 */
	employeeNo?: string;
	/**
	 * 姓名
	 */
	employeeName?: string;
	/**
	 * 手机
	 */
	mobile?: string;
	/**
	 * 
	 */
	status?: EmployeeStatusEnum;
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
	 * 离职日期
	 */
	resignDate?: string;
	/**
	 * 离职原因
	 */
	resignReason?: string;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 创建者用户名称
	 */
	createdUserName?: string;
	/**
	 * 创建时间
	 */
	createdTime?: string;
	/**
	 * 更新者用户名称
	 */
	updatedUserName?: string;
	/**
	 * 更新时间
	 */
	updatedTime?: string;
	/**
	 * 更新版本控制字段
	 */
	rowVersion?: string;
	/**
	 * 机构信息
	 */
	orgList?: EmployeeOrgModel[];
	/**
	 * 角色信息
	 */
	roleList?: EmployeeRoleModel[];
}

