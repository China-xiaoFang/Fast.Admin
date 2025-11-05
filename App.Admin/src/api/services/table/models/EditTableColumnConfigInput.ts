import { FaTableColumnCtx } from "./FaTableColumnCtx";

/**
 * Fast.Center.Service.Table.Dto.EditTableColumnConfigInput 编辑表格列配置输入
 */
export interface EditTableColumnConfigInput {
  /**
   * 表格Id
   */
  tableId?: number;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
  /**
   * 表格列
   */
  columns?: Array<FaTableColumnCtx>;
}

