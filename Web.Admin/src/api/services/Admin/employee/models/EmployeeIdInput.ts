import type { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * 职员Id输入
 */
export interface EmployeeIdInput {
	/**
	 * 职员Id
	 */
	employeeId?: number;
	/**
	 * 
	 */
	accountStatus?: CommonStatusEnum;
	/**
	 * 
	 */
	rowVersion?: number;
}

