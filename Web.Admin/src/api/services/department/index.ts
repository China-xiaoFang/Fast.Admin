import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";

/**
 * Department Service API
 */
export const departmentApi = {
  /**
   * Query department paged list
   */
  queryDepartmentPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<any>>({
      url: "/department/queryDepartmentPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * Query department detail
   */
  queryDepartmentDetail(departmentId: number) {
    return axiosUtil.request<any>({
      url: "/department/queryDepartmentDetail",
      method: "get",
      params: {
        departmentId,
      },
      requestType: "query",
    });
  },
  /**
   * Add department
   */
  addDepartment(data: any) {
    return axiosUtil.request({
      url: "/department/addDepartment",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * Edit department
   */
  editDepartment(data: any) {
    return axiosUtil.request({
      url: "/department/editDepartment",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Delete department
   */
  deleteDepartment(data: any) {
    return axiosUtil.request({
      url: "/department/deleteDepartment",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * Department selector
   */
  departmentSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/department/departmentSelector",
      method: "get",
      requestType: "query",
    });
  },
};
