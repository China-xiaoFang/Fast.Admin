import { axiosUtil } from "@fast-china/axios";
import { SyncTenantDatabaseInput } from "./models/SyncTenantDatabaseInput";

/**
 * Fast.Admin.Service.Database.DatabaseService Database 服务Api
 */
export const databaseApi = {
  /**
   * 同步租户数据库
   */
  syncTenantDatabase(data: SyncTenantDatabaseInput) {
    return axiosUtil.request({
      url: "/database/syncTenantDatabase",
      method: "post",
      data,
      requestType: "other",
    });
  },
};
