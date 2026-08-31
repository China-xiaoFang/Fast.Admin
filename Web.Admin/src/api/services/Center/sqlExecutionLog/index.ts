import { axiosUtil } from "@fast-china/axios";
import type { PagedResult } from "fast-element-plus";
import type { QuerySqlExecutionLogPagedInput } from "./models/QuerySqlExecutionLogPagedInput";
import type { SqlExecutionLogModel } from "./models/SqlExecutionLogModel";

/**
 * SQL 执行日志服务Api
 */
export const sqlExecutionLogApi = {
	/**
	 * 获取Sql执行日志分页列表
	 */
	querySqlExecutionLogPaged(data: QuerySqlExecutionLogPagedInput): Promise<PagedResult<SqlExecutionLogModel>> {
		return axiosUtil.request<PagedResult<SqlExecutionLogModel>>({
			url: "/sqlExecutionLog/querySqlExecutionLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
