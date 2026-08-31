import type { ColumnAdvancedTypeEnum } from "@/api/enums/ColumnAdvancedTypeEnum";

/**
 * FastElementPlus FaTable 列高级选项上下文
 */
export interface FaTableColumnAdvancedCtx {
	/**
	 * 字段名称
	 */
	prop?: string;
	/**
	 * 
	 */
	type?: ColumnAdvancedTypeEnum;
	/**
	 * 值
	 */
	value?: string;
}

