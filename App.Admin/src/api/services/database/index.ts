import { axiosUtil } from "@fast-china/axios";
import { SyncDatabaseInput } from "./models/SyncDatabaseInput";

/**
 * Fast.Admin.Service.Database.DatabaseService Database 服务Api
 */
export const databaseApi = {
  /**
   * 同步数据库
   */
  syncDatabase(data: SyncDatabaseInput) {
    return axiosUtil.request({
      url: "/database/syncDatabase",
      method: "post",
      data,
      requestType: "other",
    });
  },
};
