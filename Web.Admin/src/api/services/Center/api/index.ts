import { axiosUtil } from "@fast-china/axios";
import type { PagedInput, PagedResult } from "fast-element-plus";
import type { ApiInfoModel } from "./models/ApiInfoModel";

/**
 * API 服务Api
 */
export const apiApi = {
	/**
	 * 获取接口分页列表
	 */
	queryApiPaged(data: PagedInput): Promise<PagedResult<ApiInfoModel>> {
		return axiosUtil.request<PagedResult<ApiInfoModel>>({
			url: "/api/queryApiPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
