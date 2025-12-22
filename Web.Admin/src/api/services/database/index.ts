import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { SyncDatabaseInput } from "./models/SyncDatabaseInput";
import { QueryMainDatabasePagedInput } from "./models/QueryMainDatabasePagedInput";
import { QueryMainDatabasePagedOutput } from "./models/QueryMainDatabasePagedOutput";
import { QueryMainDatabaseDetailOutput } from "./models/QueryMainDatabaseDetailOutput";
import { AddMainDatabaseInput } from "./models/AddMainDatabaseInput";
import { EditMainDatabaseInput } from "./models/EditMainDatabaseInput";
import { MainDatabaseIdInput } from "./models/MainDatabaseIdInput";
import { QuerySlaveDatabasePagedInput } from "./models/QuerySlaveDatabasePagedInput";
import { QuerySlaveDatabasePagedOutput } from "./models/QuerySlaveDatabasePagedOutput";
import { QuerySlaveDatabaseDetailOutput } from "./models/QuerySlaveDatabaseDetailOutput";
import { AddSlaveDatabaseInput } from "./models/AddSlaveDatabaseInput";
import { EditSlaveDatabaseInput } from "./models/EditSlaveDatabaseInput";
import { SlaveDatabaseIdInput } from "./models/SlaveDatabaseIdInput";

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
  /**
   * 获取主库模板分页列表
   */
  queryMainDatabasePaged(data: QueryMainDatabasePagedInput) {
    return axiosUtil.request<PagedResult<QueryMainDatabasePagedOutput>>({
      url: "/mainDatabase/queryMainDatabasePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取主库模板详情
   */
  queryMainDatabaseDetail(mainDatabaseId: number) {
    return axiosUtil.request<QueryMainDatabaseDetailOutput>({
      url: "/mainDatabase/queryMainDatabaseDetail",
      method: "get",
      params: {
        mainDatabaseId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加主库模板
   */
  addMainDatabase(data: AddMainDatabaseInput) {
    return axiosUtil.request({
      url: "/mainDatabase/addMainDatabase",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑主库模板
   */
  editMainDatabase(data: EditMainDatabaseInput) {
    return axiosUtil.request({
      url: "/mainDatabase/editMainDatabase",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除主库模板
   */
  deleteMainDatabase(data: MainDatabaseIdInput) {
    return axiosUtil.request({
      url: "/mainDatabase/deleteMainDatabase",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * 获取从库模板分页列表
   */
  querySlaveDatabasePaged(data: QuerySlaveDatabasePagedInput) {
    return axiosUtil.request<PagedResult<QuerySlaveDatabasePagedOutput>>({
      url: "/slaveDatabase/querySlaveDatabasePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取从库模板详情
   */
  querySlaveDatabaseDetail(slaveDatabaseId: number) {
    return axiosUtil.request<QuerySlaveDatabaseDetailOutput>({
      url: "/slaveDatabase/querySlaveDatabaseDetail",
      method: "get",
      params: {
        slaveDatabaseId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加从库模板
   */
  addSlaveDatabase(data: AddSlaveDatabaseInput) {
    return axiosUtil.request({
      url: "/slaveDatabase/addSlaveDatabase",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑从库模板
   */
  editSlaveDatabase(data: EditSlaveDatabaseInput) {
    return axiosUtil.request({
      url: "/slaveDatabase/editSlaveDatabase",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除从库模板
   */
  deleteSlaveDatabase(data: SlaveDatabaseIdInput) {
    return axiosUtil.request({
      url: "/slaveDatabase/deleteSlaveDatabase",
      method: "post",
      data,
      requestType: "delete",
    });
  },
};
