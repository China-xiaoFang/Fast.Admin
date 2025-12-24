/**
 * Fast.Center.Service.Table.Dto.QueryTableConfigPagedOutput 获取表格配置分页列表输出
 */
export interface QueryTableConfigPagedOutput {
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

