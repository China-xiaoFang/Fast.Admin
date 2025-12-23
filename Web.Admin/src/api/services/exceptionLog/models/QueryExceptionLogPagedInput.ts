import { PagedInput } from "fast-element-plus";

/**
 * Fast.CenterLog.Service.ExceptionLog.Dto.QueryExceptionLogPagedInput 获取异常日志分页列表输入
 */
export interface QueryExceptionLogPagedInput extends PagedInput {
  /**
   * 异常类型
   */
  exceptionType?: string;
  /**
   * 开始时间
   */
  startTime?: string;
  /**
   * 结束时间
   */
  endTime?: string;
}
