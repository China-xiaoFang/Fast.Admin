/**
 * Fast.Center.Service.Config.Dto.QueryConfigDetailOutput 获取配置详情输出
 */
export interface QueryConfigDetailOutput {
  /**
   * 配置Id
   */
  configId?: number;
  /**
   * 配置编码
   */
  configCode?: string;
  /**
   * 配置名称
   */
  configName?: string;
  /**
   * 配置值
   */
  configValue?: string;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 部门名称
   */
  departmentName?: string;
  /**
   * 创建人名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新人名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
  /**
   * 行版本
   */
  rowVersion?: number;
}

