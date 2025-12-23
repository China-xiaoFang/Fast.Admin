import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { QueryOperationLogPagedOutput } from "./models/QueryOperationLogPagedOutput";
import { QueryOperationLogPagedInput } from "./models/QueryOperationLogPagedInput";

/**
 * Fast.CenterLog.Service.OperationLog.OperationLogService 操作日志服务Api
 */
export const operationLogApi = {
  /**
   * 获取操作日志分页列表
   */
  queryOperationLogPaged(data: QueryOperationLogPagedInput) {
    return axiosUtil.request<PagedResult<QueryOperationLogPagedOutput>>({
      url: "/operationLog/queryOperationLogPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取操作日志详情
   */
  queryOperationLogDetail(operationLogId: number) {
    return axiosUtil.request<QueryOperationLogPagedOutput>({
      url: "/operationLog/queryOperationLogDetail",
      method: "get",
      params: {
        operationLogId,
      },
      requestType: "query",
    });
  },
};
