import { axiosUtil } from "@fast-china/axios";
import { InitDatabaseInput } from "./models/InitDatabaseInput";

/**
 * Fast.Admin.Service.TenantDatabase.TenantDatabaseService 租户 Database 服务Api
 */
export const tenantDatabaseApi = {
  /**
   * 初始化数据库
   */
  initDatabase(data: InitDatabaseInput) {
    return axiosUtil.request({
      url: "/tenantDatabase/initDatabase",
      method: "post",
      data,
      requestType: "other",
    });
  },
};
