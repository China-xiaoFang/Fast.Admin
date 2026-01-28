import { EmployeeStatusEnum } from "@/api/enums/EmployeeStatusEnum";

/**
 * Fast.Admin.Service.Employee.Dto.ChangeStatusInput 职员更改状态输入
 */
export interface ChangeStatusInput {
  /**
   * 职员Id
   */
  employeeId?: number;
  /**
   * 
   */
  status?: EmployeeStatusEnum;
  /**
   * 
   */
  rowVersion?: number;
}

