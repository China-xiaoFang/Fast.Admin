import { SchedulerJobTypeEnum } from "@/api/enums/Scheduler/SchedulerJobTypeEnum";
import { TriggerTypeEnum } from "@/api/enums/Scheduler/TriggerTypeEnum";
import { HttpRequestMethodEnum } from "@/api/enums/Scheduler/HttpRequestMethodEnum";
import { TriggerState } from "@/api/enums/Scheduler/TriggerState";

/**
 * 调度作业信息
 */
export interface SchedulerJobInfoDto {
  /**
   * 作业名称
   */
  jobName?: string;
  /**
   * 上次执行时间
   */
  previousFireTime?: Date;
  /**
   * 下次执行时间
   */
  nextFireTime?: Date;
  /**
   * 
   */
  jobType?: SchedulerJobTypeEnum;
  /**
   * 开始时间
   */
  beginTime?: Date;
  /**
   * 结束时间
   */
  endTime?: Date;
  /**
   * 
   */
  triggerType?: TriggerTypeEnum;
  /**
   * 时间间隔
   */
  interval?: string;
  /**
   * 描述
   */
  description?: string;
  /**
   * 请求Url
   */
  requestUrl?: string;
  /**
   * 
   */
  requestMethod?: HttpRequestMethodEnum;
  /**
   * 
   */
  triggerState?: TriggerState;
  /**
   * 运行次数
   */
  runNumber?: number;
  /**
   * 异常
   */
  exception?: string;
}

