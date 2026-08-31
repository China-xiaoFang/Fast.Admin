import { axiosUtil } from "@fast-china/axios";
import type { PagedResult } from "fast-element-plus";
import type { ExceptionLogModel } from "./models/ExceptionLogModel";
import type { QueryExceptionLogPagedInput } from "./models/QueryExceptionLogPagedInput";

/**
 * 异常日志服务Api
 */
export const exceptionLogApi = {
	/**
	 * 获取异常日志分页列表
	 */
	queryExceptionLogPaged(data: QueryExceptionLogPagedInput): Promise<PagedResult<ExceptionLogModel>> {
		return axiosUtil.request<PagedResult<ExceptionLogModel>>({
			url: "/exceptionLog/queryExceptionLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
