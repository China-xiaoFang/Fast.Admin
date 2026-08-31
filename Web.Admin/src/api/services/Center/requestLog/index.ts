import { axiosUtil } from "@fast-china/axios";
import type { PagedResult } from "fast-element-plus";
import type { QueryRequestLogPagedInput } from "./models/QueryRequestLogPagedInput";
import type { RequestLogModel } from "./models/RequestLogModel";

/**
 * 请求日志服务Api
 */
export const requestLogApi = {
	/**
	 * 获取请求日志分页列表
	 */
	queryRequestLogPaged(data: QueryRequestLogPagedInput): Promise<PagedResult<RequestLogModel>> {
		return axiosUtil.request<PagedResult<RequestLogModel>>({
			url: "/requestLog/queryRequestLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
