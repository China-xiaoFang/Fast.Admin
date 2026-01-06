import { RoleTypeEnum } from "@/api/enums/RoleTypeEnum";
import { DataScopeTypeEnum } from "@/api/enums/DataScopeTypeEnum";

/**
 * Fast.Admin.Service.Role.Dto.QueryRolePagedInput 获取角色分页列表输入
 */
export interface QueryRolePagedInput extends PagedInput  {
  /**
   * 
   */
  roleType?: RoleTypeEnum;
  /**
   * 
   */
  dataScopeType?: DataScopeTypeEnum;
}

