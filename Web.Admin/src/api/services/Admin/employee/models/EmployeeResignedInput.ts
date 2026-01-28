/**
 * Fast.Admin.Service.Employee.Dto.EmployeeResignedInput 职员离职输入
 */
export interface EmployeeResignedInput {
  /**
   * 职员Id
   */
  employeeId?: number;
  /**
   * 离职日期
   */
  resignDate?: Date;
  /**
   * 离职原因
   */
  resignReason?: string;
  /**
   * 
   */
  rowVersion?: number;
}

