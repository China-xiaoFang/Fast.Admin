import { PagedInput } from "fast-element-plus";

/**
 * Fast.CenterLog.Service.OperationLog.Dto.QueryOperationLogPagedInput 获取操作日志分页列表输入
 */
export interface QueryOperationLogPagedInput extends PagedInput {
  /**
   * 控制器名称
   */
  controllerName?: string;
  /**
   * 操作名称
   */
  actionName?: string;
  /**
   * 开始时间
   */
  startTime?: string;
  /**
   * 结束时间
   */
  endTime?: string;
}
