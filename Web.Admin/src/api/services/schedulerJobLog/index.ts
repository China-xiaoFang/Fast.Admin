import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { QuerySchedulerJobLogPagedOutput } from "./models/QuerySchedulerJobLogPagedOutput";
import { QuerySchedulerJobLogPagedInput } from "./models/QuerySchedulerJobLogPagedInput";

/**
 * Fast.CenterLog.Service.SchedulerJobLog.SchedulerJobLogService 调度作业日志服务Api
 */
export const schedulerJobLogApi = {
  /**
   * 获取调度作业日志分页列表
   */
  querySchedulerJobLogPaged(data: QuerySchedulerJobLogPagedInput) {
    return axiosUtil.request<PagedResult<QuerySchedulerJobLogPagedOutput>>({
      url: "/schedulerJobLog/querySchedulerJobLogPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取调度作业日志详情
   */
  querySchedulerJobLogDetail(schedulerJobLogId: number) {
    return axiosUtil.request<QuerySchedulerJobLogPagedOutput>({
      url: "/schedulerJobLog/querySchedulerJobLogDetail",
      method: "get",
      params: {
        schedulerJobLogId,
      },
      requestType: "query",
    });
  },
};
