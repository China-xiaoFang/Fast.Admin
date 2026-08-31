import { axiosUtil } from "@fast-china/axios";
import type { PagedResult } from "fast-element-plus";
import type { QuerySqlExceptionLogPagedInput } from "./models/QuerySqlExceptionLogPagedInput";
import type { SqlExceptionLogModel } from "./models/SqlExceptionLogModel";

/**
 * SQL 异常日志服务Api
 */
export const sqlExceptionLogApi = {
	/**
	 * 获取Sql异常日志分页列表
	 */
	querySqlExceptionLogPaged(data: QuerySqlExceptionLogPagedInput): Promise<PagedResult<SqlExceptionLogModel>> {
		return axiosUtil.request<PagedResult<SqlExceptionLogModel>>({
			url: "/sqlExceptionLog/querySqlExceptionLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
