/**
 * 职员离职输入
 */
export interface EmployeeResignedInput {
	/**
	 * 职员Id
	 */
	employeeId?: number;
	/**
	 * 离职日期
	 */
	resignDate?: string;
	/**
	 * 离职原因
	 */
	resignReason?: string;
	/**
	 * 
	 */
	rowVersion?: number;
}

