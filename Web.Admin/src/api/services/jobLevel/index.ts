import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";

/**
 * Job Level Service API
 */
export const jobLevelApi = {
  /**
   * Query job level paged list
   */
  queryJobLevelPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<any>>({
      url: "/jobLevel/queryJobLevelPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * Query job level detail
   */
  queryJobLevelDetail(jobLevelId: number) {
    return axiosUtil.request<any>({
      url: "/jobLevel/queryJobLevelDetail",
      method: "get",
      params: {
        jobLevelId,
      },
      requestType: "query",
    });
  },
  /**
   * Add job level
   */
  addJobLevel(data: any) {
    return axiosUtil.request({
      url: "/jobLevel/addJobLevel",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * Edit job level
   */
  editJobLevel(data: any) {
    return axiosUtil.request({
      url: "/jobLevel/editJobLevel",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Delete job level
   */
  deleteJobLevel(data: any) {
    return axiosUtil.request({
      url: "/jobLevel/deleteJobLevel",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * Job level selector
   */
  jobLevelSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/jobLevel/jobLevelSelector",
      method: "get",
      requestType: "query",
    });
  },
};
