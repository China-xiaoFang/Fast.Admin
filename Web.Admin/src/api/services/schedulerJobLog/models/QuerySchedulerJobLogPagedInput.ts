import { PagedInput } from "fast-element-plus";

/**
 * Fast.CenterLog.Service.SchedulerJobLog.Dto.QuerySchedulerJobLogPagedInput 获取调度作业日志分页列表输入
 */
export interface QuerySchedulerJobLogPagedInput extends PagedInput {
  /**
   * 作业名称
   */
  jobName?: string;
  /**
   * 作业组
   */
  jobGroup?: string;
  /**
   * 是否成功
   */
  success?: boolean;
  /**
   * 开始时间
   */
  startTime?: string;
  /**
   * 结束时间
   */
  endTime?: string;
}
