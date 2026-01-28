/**
 * Fast.Admin.Service.Department.Dto.QueryDepartmentDetailOutput 获取部门详情输出
 */
export interface QueryDepartmentDetailOutput {
  /**
   * 部门Id
   */
  departmentId?: number;
  /**
   * 机构Id
   */
  orgId?: number;
  /**
   * 机构名称
   */
  orgName?: string;
  /**
   * 父级Id
   */
  parentId?: number;
  /**
   * 父级名称
   */
  parentName?: string;
  /**
   * 父级Id集合
   */
  parentIds?: Array<number>;
  /**
   * 父级名称集合
   */
  parentNames?: Array<string>;
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
  /**
   * 创建者用户名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新者用户名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}

