import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";
import { QueryTenantPagedOutput } from "./models/QueryTenantPagedOutput";
import { QueryTenantPagedInput } from "./models/QueryTenantPagedInput";

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
};
