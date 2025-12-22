import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";

/**
 * Employee Service API
 */
export const employeeApi = {
  /**
   * Query employee paged list
   */
  queryEmployeePaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<any>>({
      url: "/employee/queryEmployeePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * Query employee detail
   */
  queryEmployeeDetail(employeeId: number) {
    return axiosUtil.request<any>({
      url: "/employee/queryEmployeeDetail",
      method: "get",
      params: {
        employeeId,
      },
      requestType: "query",
    });
  },
  /**
   * Add employee
   */
  addEmployee(data: any) {
    return axiosUtil.request({
      url: "/employee/addEmployee",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * Edit employee
   */
  editEmployee(data: any) {
    return axiosUtil.request({
      url: "/employee/editEmployee",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Delete employee
   */
  deleteEmployee(data: any) {
    return axiosUtil.request({
      url: "/employee/deleteEmployee",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * Set employee organization
   */
  setEmployeeOrganization(data: any) {
    return axiosUtil.request({
      url: "/employee/setEmployeeOrganization",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Set employee role
   */
  setEmployeeRole(data: any) {
    return axiosUtil.request({
      url: "/employee/setEmployeeRole",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Query employee organizations
   */
  queryEmployeeOrganizations(employeeId: number) {
    return axiosUtil.request<any[]>({
      url: "/employee/queryEmployeeOrganizations",
      method: "get",
      params: {
        employeeId,
      },
      requestType: "query",
    });
  },
  /**
   * Query employee roles
   */
  queryEmployeeRoles(employeeId: number) {
    return axiosUtil.request<number[]>({
      url: "/employee/queryEmployeeRoles",
      method: "get",
      params: {
        employeeId,
      },
      requestType: "query",
    });
  },
  /**
   * Employee selector
   */
  employeeSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/employee/employeeSelector",
      method: "get",
      requestType: "query",
    });
  },
};
