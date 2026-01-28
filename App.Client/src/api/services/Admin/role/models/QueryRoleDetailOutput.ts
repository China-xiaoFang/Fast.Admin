import { RoleTypeEnum } from "@/api/enums/RoleTypeEnum";
import { DataScopeTypeEnum } from "@/api/enums/DataScopeTypeEnum";

/**
 * Fast.Admin.Service.Role.Dto.QueryRoleDetailOutput 获取角色详情输出
 */
export interface QueryRoleDetailOutput {
  /**
   * 角色Id
   */
  roleId?: number;
  /**
   * 
   */
  roleType?: RoleTypeEnum;
  /**
   * 角色名称
   */
  roleName?: string;
  /**
   * 角色编码
   */
  roleCode?: string;
  /**
   * 排序
   */
  sort?: number;
  /**
   * 
   */
  dataScopeType?: DataScopeTypeEnum;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 
   */
  departmentName?: string;
  /**
   * 
   */
  createdUserName?: string;
  /**
   * 
   */
  createdTime?: Date;
  /**
   * 
   */
  updatedUserName?: string;
  /**
   * 
   */
  updatedTime?: Date;
  /**
   * 
   */
  rowVersion?: number;
}

