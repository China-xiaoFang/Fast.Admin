/**
 * Fast.CenterLog.Service.SchedulerJobLog.Dto.QuerySchedulerJobLogPagedOutput 获取调度作业日志分页列表输出
 */
export interface QuerySchedulerJobLogPagedOutput {
  /**
   * 调度作业日志Id
   */
  schedulerJobLogId?: number;
  /**
   * 作业名称
   */
  jobName?: string;
  /**
   * 作业组
   */
  jobGroup?: string;
  /**
   * 触发器名称
   */
  triggerName?: string;
  /**
   * 触发器组
   */
  triggerGroup?: string;
  /**
   * 是否成功
   */
  success?: boolean;
  /**
   * 执行结果
   */
  result?: string;
  /**
   * 异常信息
   */
  exception?: string;
  /**
   * 耗时(毫秒)
   */
  elapsed?: number;
  /**
   * 执行时间
   */
  executeTime?: Date;
}
