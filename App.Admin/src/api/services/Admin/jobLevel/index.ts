import { axiosUtil } from "@fast-china/axios";
import { QueryJobLevelPagedOutput } from "./models/QueryJobLevelPagedOutput";
import { QueryJobLevelDetailOutput } from "./models/QueryJobLevelDetailOutput";
import { AddJobLevelInput } from "./models/AddJobLevelInput";
import { EditJobLevelInput } from "./models/EditJobLevelInput";
import { JobLevelIdInput } from "./models/JobLevelIdInput";

/**
 * Fast.Admin.Service.JobLevel.JobLevelService 职级服务Api
 */
export const jobLevelApi = {
  /**
   * 职级选择器
   */
  jobLevelSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/jobLevel/jobLevelSelector",
      method: "get",
      requestType: "query",
    });
  },
  /**
   * 获取职级分页列表
   */
  queryJobLevelPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<QueryJobLevelPagedOutput>>({
      url: "/jobLevel/queryJobLevelPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取职级详情
   */
  queryJobLevelDetail(jobLevelId: number) {
    return axiosUtil.request<QueryJobLevelDetailOutput>({
      url: "/jobLevel/queryJobLevelDetail",
      method: "get",
      params: {
        jobLevelId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加职级
   */
  addJobLevel(data: AddJobLevelInput) {
    return axiosUtil.request({
      url: "/jobLevel/addJobLevel",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑职级
   */
  editJobLevel(data: EditJobLevelInput) {
    return axiosUtil.request({
      url: "/jobLevel/editJobLevel",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除职级
   */
  deleteJobLevel(data: JobLevelIdInput) {
    return axiosUtil.request({
      url: "/jobLevel/deleteJobLevel",
      method: "post",
      data,
      requestType: "delete",
    });
  },
};
