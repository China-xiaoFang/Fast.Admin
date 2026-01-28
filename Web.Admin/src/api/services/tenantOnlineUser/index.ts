import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { TenantOnlineUserModel } from "./models/TenantOnlineUserModel";
import { QueryTenantOnlineUserPagedInput } from "./models/QueryTenantOnlineUserPagedInput";
import { ForceOfflineInput } from "./models/ForceOfflineInput";

/**
 * Fast.Center.Service.TenantOnlineUser.TenantOnlineUserService 在线用户服务Api
 */
export const tenantOnlineUserApi = {
  /**
   * 获取在线用户分页列表
   */
  queryTenantOnlineUserPaged(data: QueryTenantOnlineUserPagedInput) {
    return axiosUtil.request<PagedResult<TenantOnlineUserModel>>({
      url: "/tenantOnlineUser/queryTenantOnlineUserPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 强制下线
   */
  forceOffline(data: ForceOfflineInput) {
    return axiosUtil.request({
      url: "/tenantOnlineUser/forceOffline",
      method: "post",
      data,
      requestType: "query",
    });
  },
};
