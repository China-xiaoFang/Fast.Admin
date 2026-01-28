import { axiosUtil } from "@fast-china/axios";
import { PagedInput, PagedResult } from "fast-element-plus";
import { PayRecordModel } from "./models/PayRecordModel";

/**
 * Fast.Center.Service.PayRecord.PayRecordService 支付记录服务Api
 */
export const payRecordApi = {
  /**
   * 获取支付记录分页列表
   */
  queryPasswordMapPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<PayRecordModel>>({
      url: "/payRecord/queryPasswordMapPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
};
