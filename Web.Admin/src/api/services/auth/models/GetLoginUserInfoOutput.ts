import { AuthModuleInfoDto } from "./AuthModuleInfoDto";

/**
 * Fast.Admin.Service.Auth.Dto.GetLoginUserInfoOutput 获取登录用户信息输出
 */
export interface GetLoginUserInfoOutput {
  /**
   * 账号Key
   */
  accountKey?: string;
  /**
   * 手机
   */
  mobile?: string;
  /**
   * 昵称
   */
  nickName?: string;
  /**
   * 头像
   */
  avatar?: string;
  /**
   * 租户编号
   */
  tenantNo?: string;
  /**
   * 租户名称
   */
  tenantName?: string;
  /**
   * 用户Key
   */
  userKey?: string;
  /**
   * 账户
   */
  account?: string;
  /**
   * 工号
   */
  employeeNo?: string;
  /**
   * 姓名
   */
  employeeName?: string;
  /**
   * 部门Id
   */
  departmentId?: number;
  /**
   * 部门名称
   */
  departmentName?: string;
  /**
   * 是否超级管理员
   */
  isSuperAdmin?: boolean;
  /**
   * 是否管理员
   */
  isAdmin?: boolean;
  /**
   * 角色名称集合
   */
  roleNameList?: Array<string>;
  /**
   * 按钮编码集合
   */
  buttonCodeList?: Array<string>;
  /**
   * 菜单集合
   */
  menuList?: Array<AuthModuleInfoDto>;
}

