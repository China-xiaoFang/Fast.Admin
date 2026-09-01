import { axiosUtil } from "@fast-china/axios";
import type { RefundRecordModel } from "./models/RefundRecordModel";

/**
 * 退款记录服务Api
 */
export const refundRecordApi = {
	/**
	 * 获取退款记录分页列表
	 */
	queryRefundRecordPaged(data: PagedInput): Promise<PagedResult<RefundRecordModel>> {
		return axiosUtil.request<PagedResult<RefundRecordModel>>({
			url: "/refundRecord/queryRefundRecordPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
};
