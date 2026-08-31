import { axiosUtil } from "@fast-china/axios";
import type { PagedInput, PagedResult } from "fast-element-plus";
import type { PayRecordModel } from "./models/PayRecordModel";

/**
 * 支付记录服务Api
 */
export const payRecordApi = {
	/**
	 * 获取支付记录分页列表
	 */
	queryPayRecordPaged(data: PagedInput): Promise<PagedResult<PayRecordModel>> {
		return axiosUtil.request<PagedResult<PayRecordModel>>({
			url: "/payRecord/queryPayRecordPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
