/**
 * Fast.Admin.Service.Organization.Dto.AddOrganizationInput 添加机构输入
 */
export interface AddOrganizationInput {
  /**
   * 父级Id
   */
  parentId?: number;
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
}

