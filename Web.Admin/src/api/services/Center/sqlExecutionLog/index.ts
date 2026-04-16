import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { SqlExecutionLogModel } from "./models/SqlExecutionLogModel";
import { QuerySqlExecutionLogPagedInput } from "./models/QuerySqlExecutionLogPagedInput";

/**
 * Fast.Center.Service.SqlExecutionLog.SqlExecutionLogService Sql执行日志服务Api
 */
export const sqlExecutionLogApi = {
  /**
   * 获取Sql执行日志分页列表
   */
  querySqlExecutionLogPaged(data: QuerySqlExecutionLogPagedInput) {
    return axiosUtil.request<PagedResult<SqlExecutionLogModel>>({
      url: "/sqlExecutionLog/querySqlExecutionLogPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
};
