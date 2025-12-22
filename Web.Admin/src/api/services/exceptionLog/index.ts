import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { QueryExceptionLogPagedOutput } from "./models/QueryExceptionLogPagedOutput";
import { QueryExceptionLogPagedInput } from "./models/QueryExceptionLogPagedInput";

/**
 * Fast.CenterLog.Service.ExceptionLog.ExceptionLogService 异常日志服务Api
 */
export const exceptionLogApi = {
  /**
   * 获取异常日志分页列表
   */
  queryExceptionLogPaged(data: QueryExceptionLogPagedInput) {
    return axiosUtil.request<PagedResult<QueryExceptionLogPagedOutput>>({
      url: "/exceptionLog/queryExceptionLogPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取异常日志详情
   */
  queryExceptionLogDetail(exceptionLogId: number) {
    return axiosUtil.request<QueryExceptionLogPagedOutput>({
      url: "/exceptionLog/queryExceptionLogDetail",
      method: "get",
      params: {
        exceptionLogId,
      },
      requestType: "query",
    });
  },
};
