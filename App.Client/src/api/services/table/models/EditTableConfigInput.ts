/**
 * Fast.Center.Service.Table.Dto.EditTableConfigInput 编辑表格配置输入
 */
export interface EditTableConfigInput {
  /**
   * 表格Id
   */
  tableId?: number;
  /**
   * 表格名称
   */
  tableName?: string;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}

