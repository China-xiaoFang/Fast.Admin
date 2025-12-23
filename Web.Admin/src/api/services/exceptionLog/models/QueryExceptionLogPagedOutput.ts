/**
 * Fast.CenterLog.Service.ExceptionLog.Dto.QueryExceptionLogPagedOutput 获取异常日志分页列表输出
 */
export interface QueryExceptionLogPagedOutput {
  /**
   * 异常日志Id
   */
  exceptionLogId?: number;
  /**
   * 控制器名称
   */
  controllerName?: string;
  /**
   * 操作名称
   */
  actionName?: string;
  /**
   * 显示名称
   */
  displayName?: string;
  /**
   * 异常类型
   */
  exceptionType?: string;
  /**
   * 异常消息
   */
  exceptionMessage?: string;
  /**
   * 堆栈跟踪
   */
  stackTrace?: string;
  /**
   * 请求URL
   */
  requestUrl?: string;
  /**
   * IP地址
   */
  ip?: string;
  /**
   * 异常时间
   */
  exceptionTime?: Date;
}
