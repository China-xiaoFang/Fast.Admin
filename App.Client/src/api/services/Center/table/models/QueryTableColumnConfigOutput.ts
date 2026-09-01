/**
 * 获取表格列配置输出
 */
export interface QueryTableColumnConfigOutput {
	/**
	 * 表格Key
	 */
	tableKey?: string;
	/**
	 * 原始列
	 */
	columns?: Record<string, unknown>[];
	/**
	 * 缓存列
	 */
	cacheColumns?: Record<string, unknown>[];
	/**
	 * 更新时间
	 */
	updatedTime?: string;
	/**
	 * 是否存在改变
	 */
	change?: boolean;
	/**
	 * 是否存在缓存
	 */
	cache?: boolean;
}

