import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput } from "fast-element-plus";
import { QueryOrganizationDetailOutput } from "./models/QueryOrganizationDetailOutput";
import { AddOrganizationInput } from "./models/AddOrganizationInput";
import { EditOrganizationInput } from "./models/EditOrganizationInput";
import { OrganizationIdInput } from "./models/OrganizationIdInput";

/**
 * Fast.Admin.Service.Organization.OrganizationService 机构服务Api
 */
export const organizationApi = {
  /**
   * 机构选择器
   */
  organizationSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/organization/organizationSelector",
      method: "get",
      requestType: "query",
    });
  },
  /**
   * 获取机构详情
   */
  queryOrganizationDetail(orgId: number) {
    return axiosUtil.request<QueryOrganizationDetailOutput>({
      url: "/organization/queryOrganizationDetail",
      method: "get",
      params: {
        orgId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加机构
   */
  addOrganization(data: AddOrganizationInput) {
    return axiosUtil.request({
      url: "/organization/addOrganization",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑机构
   */
  editOrganization(data: EditOrganizationInput) {
    return axiosUtil.request({
      url: "/organization/editOrganization",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除机构
   */
  deleteOrganization(data: OrganizationIdInput) {
    return axiosUtil.request({
      url: "/organization/deleteOrganization",
      method: "post",
      data,
      requestType: "delete",
    });
  },
};
