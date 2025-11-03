/**
 * Fast.Center.Service.Table.Dto.QueryTableConfigDetailOutput 获取表格配置详情输出
 */
export interface QueryTableConfigDetailOutput {
  /**
   * 表格Id
   */
  tableId?: number;
  /**
   * 表格Key
   */
  tableKey?: string;
  /**
   * 表格名称
   */
  tableName?: string;
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

