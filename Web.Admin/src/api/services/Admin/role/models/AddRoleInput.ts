import { RoleTypeEnum } from "@/api/enums/RoleTypeEnum";
import { DataScopeTypeEnum } from "@/api/enums/DataScopeTypeEnum";

/**
 * Fast.Admin.Service.Role.Dto.AddRoleInput 添加角色输入
 */
export interface AddRoleInput {
  /**
   * 
   */
  roleType?: RoleTypeEnum;
  /**
   * 是否使用系统菜单
   */
  isSystemMenu?: boolean;
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
   * 数据范围自定义部门Id集合
   */
  dataScopeDepartmentIds?: Array<number>;
  /**
   * 可分配的角色Id集合
   */
  assignableRoleIds?: Array<number>;
  /**
   * 备注
   */
  remark?: string;
}

