import { PagedInput } from "fast-element-plus";

/**
 * Fast.Center.Service.SqlTimeoutLog.Dto.QuerySqlTimeoutLogPagedInput 获取Sql超时日志分页列表输入
 */
export interface QuerySqlTimeoutLogPagedInput extends PagedInput  {
  /**
   * 账号Id
   */
  accountId?: number;
  /**
   * 租户Id
   */
  tenantId?: number;
}

