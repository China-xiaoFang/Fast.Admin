import { axiosUtil } from "@fast-china/axios";
import type { QuerySqlDiffLogPagedInput } from "./models/QuerySqlDiffLogPagedInput";
import type { SqlDiffLogModel } from "./models/SqlDiffLogModel";

/**
 * SQL 差异日志服务Api
 */
export const sqlDiffLogApi = {
	/**
	 * 获取Sql差异日志分页列表
	 */
	querySqlDiffLogPaged(data: QuerySqlDiffLogPagedInput): Promise<PagedResult<SqlDiffLogModel>> {
		return axiosUtil.request<PagedResult<SqlDiffLogModel>>({
			url: "/sqlDiffLog/querySqlDiffLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
