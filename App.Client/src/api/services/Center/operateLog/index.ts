import { axiosUtil } from "@fast-china/axios";
import { OperateLogModel } from "./models/OperateLogModel";
import { QueryOperateLogPagedInput } from "./models/QueryOperateLogPagedInput";

/**
 * Fast.Admin.Service.OperateLog.OperateLogService 操作日志服务Api
 */
export const operateLogApi = {
  /**
   * 获取操作日志分页列表
   */
  queryOperateLogPaged(data: QueryOperateLogPagedInput) {
    return axiosUtil.request<PagedResult<OperateLogModel>>({
      url: "/operateLog/queryOperateLogPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 删除90天前的操作日志
   */
  deleteOperateLog() {
    return axiosUtil.request({
      url: "/operateLog/deleteOperateLog",
      method: "post",
      requestType: "delete",
    });
  },
};
