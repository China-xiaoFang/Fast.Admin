import { PagedInput } from "fast-element-plus";

/**
 * Fast.Center.Service.SqlExceptionLog.Dto.QuerySqlExceptionLogPagedInput 获取Sql异常日志分页列表输入
 */
export interface QuerySqlExceptionLogPagedInput extends PagedInput  {
  /**
   * 账号Id
   */
  accountId?: number;
  /**
   * 租户Id
   */
  tenantId?: number;
  /**
   * 
   */
  readonly isOrderBy?: boolean;
}

