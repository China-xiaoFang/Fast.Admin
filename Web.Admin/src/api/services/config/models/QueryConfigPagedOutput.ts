/**
 * Fast.Center.Service.Config.Dto.QueryConfigPagedOutput 获取配置分页列表输出
 */
export interface QueryConfigPagedOutput {
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
}

