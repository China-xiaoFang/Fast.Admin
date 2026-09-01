import type { EmployeeStatusEnum } from "@/api/enums/EmployeeStatusEnum";

/**
 * 职员更改状态输入
 */
export interface ChangeStatusInput {
	/**
	 * 职员Id
	 */
	employeeId?: string;
	/**
	 * 
	 */
	status?: EmployeeStatusEnum;
	/**
	 * 
	 */
	rowVersion?: string;
}

