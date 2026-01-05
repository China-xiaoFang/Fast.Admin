/**
 * Fast.Admin.Service.Employee.Dto.EmployeeAuthInput 职员授权输入
 */
export interface EmployeeAuthInput {
  /**
   * 职员名称
   */
  employeeName?: string;
  /**
   * 菜单Id集合
   */
  menuIds?: Array<number>;
  /**
   * 角色菜单Id集合
   */
  roleMenuIds?: Array<number>;
  /**
   * 按钮Id集合
   */
  buttonIds?: Array<number>;
  /**
   * 角色按钮Id集合
   */
  roleButtonIds?: Array<number>;
  /**
   * 职员Id
   */
  employeeId?: number;
  /**
   * 
   */
  rowVersion?: number;
}

