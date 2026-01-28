import { PagedInput } from "fast-element-plus";

/**
 * Fast.Admin.Service.Department.Dto.QueryDepartmentPagedInput 获取部门分页列表输入
 */
export interface QueryDepartmentPagedInput extends PagedInput  {
  /**
   * 机构Id
   */
  orgId?: number;
  /**
   * 
   */
  readonly isOrderBy?: boolean;
}

