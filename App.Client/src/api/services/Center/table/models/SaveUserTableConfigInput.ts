import type { SaveUserTableColumnConfigDto } from "./SaveUserTableColumnConfigDto";

/**
 * 保存用户表格配置输入
 */
export interface SaveUserTableConfigInput {
	/**
	 * 表格Key
	 */
	tableKey?: string;
	/**
	 * 表格列
	 */
	columns?: SaveUserTableColumnConfigDto[];
}

