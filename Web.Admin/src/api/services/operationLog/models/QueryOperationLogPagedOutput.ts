/**
 * Fast.CenterLog.Service.OperationLog.Dto.QueryOperationLogPagedOutput 获取操作日志分页列表输出
 */
export interface QueryOperationLogPagedOutput {
  /**
   * 操作日志Id
   */
  operationLogId?: number;
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
   * 请求方法
   */
  requestMethod?: string;
  /**
   * 请求URL
   */
  requestUrl?: string;
  /**
   * IP地址
   */
  ip?: string;
  /**
   * 地点
   */
  location?: string;
  /**
   * 浏览器
   */
  browser?: string;
  /**
   * 操作系统
   */
  os?: string;
  /**
   * 耗时(毫秒)
   */
  elapsed?: number;
  /**
   * 操作时间
   */
  operationTime?: Date;
}
