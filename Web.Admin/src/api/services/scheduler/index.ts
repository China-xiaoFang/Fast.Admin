import { axiosUtil } from "@fast-china/axios";
import { QuerySchedulerDetailOutput } from "./models/QuerySchedulerDetailOutput";
import { SchedulerJobKeyInput } from "./models/SchedulerJobKeyInput";
import { QueryAllSchedulerJobOutput } from "./models/QueryAllSchedulerJobOutput";
import { SchedulerJobGroupEnum } from "@/api/enums/SchedulerJobGroupEnum";
import { SchedulerJobInfo } from "./models/SchedulerJobInfo";
import { AddSchedulerJobInput } from "./models/AddSchedulerJobInput";
import { EditSchedulerJobInput } from "./models/EditSchedulerJobInput";

/**
 * Fast.Scheduler.Applications.SchedulerApplication 调度作业Api
 */
export const schedulerApi = {
  /**
   * 运行并验证Cron表达式
   */
  runVerifyCron(cron: string) {
    return axiosUtil.request<string[]>({
      url: "http://127.0.0.1:38082/scheduler/runVerifyCron",
      method: "get",
      params: {
        cron,
      },
      requestType: "other",
    });
  },
  /**
   * 获取调度器详情
   */
  querySchedulerDetail(tenantId: number) {
    return axiosUtil.request<QuerySchedulerDetailOutput>({
      url: "http://127.0.0.1:38082/scheduler/querySchedulerDetail",
      method: "get",
      params: {
        tenantId,
      },
      requestType: "query",
    });
  },
  /**
   * 启动调度器
   */
  startScheduler(tenantId: number) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/startScheduler",
      method: "post",
      params: {
        tenantId,
      },
      requestType: "other",
    });
  },
  /**
   * 停止调度器
   */
  stopScheduler(tenantId: number) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/stopScheduler",
      method: "post",
      params: {
        tenantId,
      },
      requestType: "other",
    });
  },
  /**
   * 暂停调度作业
   */
  stopSchedulerJob(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/stopSchedulerJob",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "other",
    });
  },
  /**
   * 恢复调度作业
   */
  resumeSchedulerJob(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/resumeSchedulerJob",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "other",
    });
  },
  /**
   * 立即执行调度作业
   */
  triggerSchedulerJob(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/triggerSchedulerJob",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "other",
    });
  },
  /**
   * 获取调度作业日志
   */
  querySchedulerJobLogs(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request<string[]>({
      url: "http://127.0.0.1:38082/scheduler/querySchedulerJobLogs",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "query",
    });
  },
  /**
   * 获取调度作业运行次数
   */
  querySchedulerJobRunNumber(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request<number>({
      url: "http://127.0.0.1:38082/scheduler/querySchedulerJobRunNumber",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "query",
    });
  },
  /**
   * 获取全部调度作业
   */
  queryAllSchedulerJob(jobGroup: SchedulerJobGroupEnum, tenantId: number) {
    return axiosUtil.request<QueryAllSchedulerJobOutput[]>({
      url: "http://127.0.0.1:38082/scheduler/queryAllSchedulerJob",
      method: "get",
      params: {
        jobGroup,
        tenantId,
      },
      requestType: "query",
    });
  },
  /**
   * 获取调度作业
   */
  querySchedulerJob(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request<SchedulerJobInfo>({
      url: "http://127.0.0.1:38082/scheduler/querySchedulerJob",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "query",
    });
  },
  /**
   * 添加调度作业
   */
  addSchedulerJob(data: AddSchedulerJobInput) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/addSchedulerJob",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑调度作业
   */
  editSchedulerJob(data: EditSchedulerJobInput) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/editSchedulerJob",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除调度作业
   */
  deleteSchedulerJob(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/deleteSchedulerJob",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "delete",
    });
  },
  /**
   * 移除调度作业异常信息
   */
  deleteSchedulerJobException(tenantId: number, data: SchedulerJobKeyInput) {
    return axiosUtil.request({
      url: "http://127.0.0.1:38082/scheduler/deleteSchedulerJobException",
      method: "post",
      params: {
        tenantId,
      },
      data,
      requestType: "delete",
    });
  },
};
