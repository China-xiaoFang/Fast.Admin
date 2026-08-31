import { axiosUtil } from "@fast-china/axios";
import type { PagedResult } from "fast-element-plus";
import type { OperateLogModel } from "./models/OperateLogModel";
import type { QueryOperateLogPagedInput } from "./models/QueryOperateLogPagedInput";

/**
 * 操作日志服务Api
 */
export const operateLogApi = {
	/**
	 * 获取操作日志分页列表
	 */
	queryOperateLogPaged(data: QueryOperateLogPagedInput): Promise<PagedResult<OperateLogModel>> {
		return axiosUtil.request<PagedResult<OperateLogModel>>({
			url: "/operateLog/queryOperateLogPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
