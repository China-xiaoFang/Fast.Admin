import { axiosUtil } from "@fast-china/axios";
import type { PagedResult } from "fast-element-plus";
import type { QueryPasswordRecordPagedInput } from "./models/QueryPasswordRecordPagedInput";
import type { QueryPasswordRecordPagedOutput } from "./models/QueryPasswordRecordPagedOutput";

/**
 * 密码记录服务Api
 */
export const passwordRecordApi = {
	/**
	 * 获取密码记录分页列表
	 */
	queryPasswordRecordPaged(data: QueryPasswordRecordPagedInput): Promise<PagedResult<QueryPasswordRecordPagedOutput>> {
		return axiosUtil.request<PagedResult<QueryPasswordRecordPagedOutput>>({
			url: "/passwordRecord/queryPasswordRecordPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
