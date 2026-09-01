import { axiosUtil } from "@fast-china/axios";
import type { QueryVisitLogPagedInput } from "./models/QueryVisitLogPagedInput";
import type { VisitLogModel } from "./models/VisitLogModel";

/**
 * 访问日志服务Api
 */
export const visitLogApi = {
	/**
	 * 获取访问日志分页列表
	 */
	queryVisitLogPaged(data: QueryVisitLogPagedInput): Promise<PagedResult<VisitLogModel>> {
		return axiosUtil.request<PagedResult<VisitLogModel>>({
			url: "/visitLog/queryVisitLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
