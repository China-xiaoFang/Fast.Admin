import { axiosUtil } from "@fast-china/axios";
import type { PagedResult } from "fast-element-plus";
import type { QuerySqlTimeoutLogPagedInput } from "./models/QuerySqlTimeoutLogPagedInput";
import type { SqlTimeoutLogModel } from "./models/SqlTimeoutLogModel";

/**
 * SQL 超时日志服务Api
 */
export const sqlTimeoutLogApi = {
	/**
	 * 获取Sql超时日志分页列表
	 */
	querySqlTimeoutLogPaged(data: QuerySqlTimeoutLogPagedInput): Promise<PagedResult<SqlTimeoutLogModel>> {
		return axiosUtil.request<PagedResult<SqlTimeoutLogModel>>({
			url: "/sqlTimeoutLog/querySqlTimeoutLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
