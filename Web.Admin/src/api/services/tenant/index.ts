import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";
import { QueryTenantPagedOutput } from "./models/QueryTenantPagedOutput";
import { QueryTenantPagedInput } from "./models/QueryTenantPagedInput";
import { QueryTenantDetailOutput } from "./models/QueryTenantDetailOutput";
import { AddTenantInput } from "./models/AddTenantInput";
import { EditTenantInput } from "./models/EditTenantInput";
import { TenantIdInput } from "./models/TenantIdInput";

/**
 * Fast.Center.Service.Tenant.TenantService 租户服务Api
 */
export const tenantApi = {
  /**
   * 租户选择器
   */
  tenantSelector(data: PagedInput) {
    return axiosUtil.request<PagedResult<ElSelectorOutput<number>>>({
      url: "/tenant/tenantSelector",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取租户分页列表
   */
  queryTenantPaged(data: QueryTenantPagedInput) {
    return axiosUtil.request<PagedResult<QueryTenantPagedOutput>>({
      url: "/tenant/queryTenantPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取租户详情
   */
  queryTenantDetail(tenantId: number) {
    return axiosUtil.request<QueryTenantDetailOutput>({
      url: "/tenant/queryTenantDetail",
      method: "get",
      params: {
        tenantId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加租户
   */
  addTenant(data: AddTenantInput) {
    return axiosUtil.request({
      url: "/tenant/addTenant",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑租户
   */
  editTenant(data: EditTenantInput) {
    return axiosUtil.request({
      url: "/tenant/editTenant",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 租户更改状态
   */
  changeStatus(data: TenantIdInput) {
    return axiosUtil.request({
      url: "/tenant/changeStatus",
      method: "post",
      data,
      requestType: "edit",
    });
  },
};
