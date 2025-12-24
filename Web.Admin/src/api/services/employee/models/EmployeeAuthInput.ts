/**
 * Fast.Admin.Service.Employee.Dto.EmployeeAuthInput 职员授权输入
 */
export interface EmployeeAuthInput {
  /**
   * 菜单Id集合
   */
  menuIds?: Array<number>;
  /**
   * 按钮Id集合
   */
  buttonIds?: Array<number>;
  /**
   * 职员Id
   */
  employeeId?: number;
  /**
   * 
   */
  rowVersion?: number;
}

