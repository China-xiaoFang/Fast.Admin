import { axiosUtil } from "@fast-china/axios";
import { QueryDatabasePagedOutput } from "./models/QueryDatabasePagedOutput";
import { QueryDatabasePagedInput } from "./models/QueryDatabasePagedInput";
import { QueryDatabaseDetailOutput } from "./models/QueryDatabaseDetailOutput";
import { AddDatabaseInput } from "./models/AddDatabaseInput";
import { EditDatabaseInput } from "./models/EditDatabaseInput";
import { MainIdInput } from "./models/MainIdInput";

/**
 * Fast.Center.Service.Database.DatabaseService 数据库服务Api
 */
export const databaseApi = {
  /**
   * 获取数据库分页列表
   */
  queryDatabasePaged(data: QueryDatabasePagedInput) {
    return axiosUtil.request<PagedResult<QueryDatabasePagedOutput>>({
      url: "/database/queryDatabasePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取数据库详情
   */
  queryDatabaseDetail(mainId: number) {
    return axiosUtil.request<QueryDatabaseDetailOutput>({
      url: "/database/queryDatabaseDetail",
      method: "get",
      params: {
        mainId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加数据库
   */
  addDatabase(data: AddDatabaseInput) {
    return axiosUtil.request({
      url: "/database/addDatabase",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑数据库
   */
  editDatabase(data: EditDatabaseInput) {
    return axiosUtil.request({
      url: "/database/editDatabase",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除数据库
   */
  deleteDatabase(data: MainIdInput) {
    return axiosUtil.request({
      url: "/database/deleteDatabase",
      method: "post",
      data,
      requestType: "delete",
    });
  },
};
