/**
 * Fast.Admin.Service.Employee.Dto.BindLoginAccountInput 绑定登录账号输入
 */
export interface BindLoginAccountInput {
  /**
   * 职员Id
   */
  employeeId?: number;
  /**
   * 账号
   */
  account?: string;
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
  rowVersion?: number;
}

