/**
 * Fast.Admin.Entity.EmployeeOrgModel 职员机构表Model类
 */
export interface EmployeeOrgModel {
  /**
   * 职员Id
   */
  employeeId?: number;
  /**
   * 机构Id
   */
  orgId?: number;
  /**
   * 机构名称
   */
  orgName?: string;
  /**
   * 机构名称
   */
  orgNames?: Array<string>;
  /**
   * 部门Id
   */
  departmentId?: number;
  /**
   * 部门名称
   */
  departmentName?: string;
  /**
   * 部门名称
   */
  departmentNames?: Array<string>;
  /**
   * 是否为主部门
   */
  isPrimary?: boolean;
  /**
   * 职位Id
   */
  positionId?: number;
  /**
   * 职位名称
   */
  positionName?: string;
  /**
   * 职级Id
   */
  jobLevelId?: number;
  /**
   * 职级名称
   */
  jobLevelName?: string;
  /**
   * 是否为负责人
   */
  isPrincipal?: boolean;
}

