import { PagedInput } from "fast-element-plus";

/**
 * Fast.Center.Service.ExceptionLog.Dto.QueryExceptionLogPagedInput 获取异常日志分页列表输入
 */
export interface QueryExceptionLogPagedInput extends PagedInput  {
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

