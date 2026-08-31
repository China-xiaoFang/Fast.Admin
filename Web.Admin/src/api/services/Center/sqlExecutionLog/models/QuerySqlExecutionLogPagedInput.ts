import type { PagedInput } from "fast-element-plus";

/**
 * 获取 SQL 执行日志分页列表输入
 */
export interface QuerySqlExecutionLogPagedInput extends PagedInput  {
	/**
	 * 账号Id
	 */
	accountId?: number;
	/**
	 * 租户Id
	 */
	tenantId?: number;
	/**
	 * 
	 */
	readonly isOrderBy?: boolean;
}

