import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { SqlDiffLogModel } from "./models/SqlDiffLogModel";
import { QuerySqlDiffLogPagedInput } from "./models/QuerySqlDiffLogPagedInput";

/**
 * Fast.Center.Service.SqlDiffLog.SqlDiffLogService Sql差异日志服务Api
 */
export const sqlDiffLogApi = {
  /**
   * 获取Sql差异日志分页列表
   */
  querySqlDiffLogPaged(data: QuerySqlDiffLogPagedInput) {
    return axiosUtil.request<PagedResult<SqlDiffLogModel>>({
      url: "/sqlDiffLog/querySqlDiffLogPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
};
