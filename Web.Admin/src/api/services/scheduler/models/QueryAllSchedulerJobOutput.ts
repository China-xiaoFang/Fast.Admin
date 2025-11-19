import { SchedulerJobGroupEnum } from "@/api/enums/SchedulerJobGroupEnum";
import { SchedulerJobInfoDto } from "./SchedulerJobInfoDto";

/**
 * Fast.Scheduler.QueryAllSchedulerJobOutput 获取全部调度作业输出
 */
export interface QueryAllSchedulerJobOutput {
  /**
   * 
   */
  jobGroup?: SchedulerJobGroupEnum;
  /**
   * 作业信息
   */
  jobInfoList?: Array<SchedulerJobInfoDto>;
}

