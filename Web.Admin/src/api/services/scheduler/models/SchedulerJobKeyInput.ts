import { SchedulerJobGroupEnum } from "@/api/enums/Scheduler/SchedulerJobGroupEnum";

/**
 * Fast.Scheduler.SchedulerJobKeyInput 调度作业Key输入
 */
export interface SchedulerJobKeyInput {
  /**
   * 作业名称
   */
  jobName?: string;
  /**
   * 
   */
  jobGroup?: SchedulerJobGroupEnum;
}

