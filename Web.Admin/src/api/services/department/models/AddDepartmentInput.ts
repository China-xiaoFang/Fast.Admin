/**
 * Fast.Admin.Service.Department.Dto.AddDepartmentInput 添加部门输入
 */
export interface AddDepartmentInput {
  /**
   * 机构Id
   */
  orgId?: number;
  /**
   * 父级Id
   */
  parentId?: number;
  /**
   * 部门名称
   */
  departmentName?: string;
  /**
   * 部门编码
   */
  departmentCode?: string;
  /**
   * 联系人
   */
  contacts?: string;
  /**
   * 电话
   */
  phone?: string;
  /**
   * 邮箱
   */
  email?: string;
  /**
   * 排序
   */
  sort?: number;
  /**
   * 备注
   */
  remark?: string;
}

