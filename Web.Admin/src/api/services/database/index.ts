import { axiosUtil } from "@fast-china/axios";
import { InitDatabaseInput } from "./models/InitDatabaseInput";

/**
 * Fast.Admin.Service.Database.DatabaseService Database 服务Api
 */
export const databaseApi = {
  /**
   * 初始化数据库
   */
  initDatabase(data: InitDatabaseInput) {
    return axiosUtil.request({
      url: "/database/initDatabase",
      method: "post",
      data,
      requestType: "other",
    });
  },
};
