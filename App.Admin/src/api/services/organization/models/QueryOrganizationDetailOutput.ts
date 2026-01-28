/**
 * Fast.Admin.Service.Organization.Dto.QueryOrganizationDetailOutput 获取机构详情输出
 */
export interface QueryOrganizationDetailOutput {
  /**
   * 机构Id
   */
  orgId?: number;
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
   * 机构名称
   */
  orgName?: string;
  /**
   * 机构编码
   */
  orgCode?: string;
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

