import type { FaTableColumnCtx } from "./FaTableColumnCtx";

/**
 * 编辑表格列配置输入
 */
export interface EditTableColumnConfigInput {
	/**
	 * 表格Id
	 */
	tableId?: string;
	/**
	 * 表格列
	 */
	columns?: FaTableColumnCtx[];
	/**
	 * 
	 */
	rowVersion?: string;
}

