import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { QueryLoginLogPagedOutput } from "./models/QueryLoginLogPagedOutput";
import { QueryLoginLogPagedInput } from "./models/QueryLoginLogPagedInput";

/**
 * Fast.CenterLog.Service.LoginLog.LoginLogService 登录日志服务Api
 */
export const loginLogApi = {
  /**
   * 获取登录日志分页列表
   */
  queryLoginLogPaged(data: QueryLoginLogPagedInput) {
    return axiosUtil.request<PagedResult<QueryLoginLogPagedOutput>>({
      url: "/loginLog/queryLoginLogPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取登录日志详情
   */
  queryLoginLogDetail(loginLogId: number) {
    return axiosUtil.request<QueryLoginLogPagedOutput>({
      url: "/loginLog/queryLoginLogDetail",
      method: "get",
      params: {
        loginLogId,
      },
      requestType: "query",
    });
  },
};
