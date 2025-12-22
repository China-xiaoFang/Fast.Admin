import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";

/**
 * Organization Service API
 */
export const organizationApi = {
  /**
   * Query organization paged list
   */
  queryOrganizationPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<any>>({
      url: "/organization/queryOrganizationPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * Query organization detail
   */
  queryOrganizationDetail(organizationId: number) {
    return axiosUtil.request<any>({
      url: "/organization/queryOrganizationDetail",
      method: "get",
      params: {
        organizationId,
      },
      requestType: "query",
    });
  },
  /**
   * Add organization
   */
  addOrganization(data: any) {
    return axiosUtil.request({
      url: "/organization/addOrganization",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * Edit organization
   */
  editOrganization(data: any) {
    return axiosUtil.request({
      url: "/organization/editOrganization",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Delete organization
   */
  deleteOrganization(data: any) {
    return axiosUtil.request({
      url: "/organization/deleteOrganization",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * Organization selector
   */
  organizationSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/organization/organizationSelector",
      method: "get",
      requestType: "query",
    });
  },
};
