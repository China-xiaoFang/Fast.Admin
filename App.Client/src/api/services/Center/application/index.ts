import { axiosUtil } from "@fast-china/axios";
import { QueryApplicationPagedOutput } from "./models/QueryApplicationPagedOutput";
import { QueryApplicationPagedInput } from "./models/QueryApplicationPagedInput";
import { QueryApplicationDetailOutput } from "./models/QueryApplicationDetailOutput";
import { AddApplicationInput } from "./models/AddApplicationInput";
import { EditApplicationInput } from "./models/EditApplicationInput";
import { AppIdInput } from "./models/AppIdInput";

/**
 * Fast.Center.Service.Application.ApplicationService 应用服务Api
 */
export const applicationApi = {
  /**
   * 应用选择器
   */
  applicationSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/application/applicationSelector",
      method: "get",
      requestType: "query",
    });
  },
  /**
   * 获取应用分页列表
   */
  queryApplicationPaged(data: QueryApplicationPagedInput) {
    return axiosUtil.request<PagedResult<QueryApplicationPagedOutput>>({
      url: "/application/queryApplicationPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取应用详情
   */
  queryApplicationDetail(appId: number) {
    return axiosUtil.request<QueryApplicationDetailOutput>({
      url: "/application/queryApplicationDetail",
      method: "get",
      params: {
        appId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加应用
   */
  addApplication(data: AddApplicationInput) {
    return axiosUtil.request({
      url: "/application/addApplication",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑应用
   */
  editApplication(data: EditApplicationInput) {
    return axiosUtil.request({
      url: "/application/editApplication",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除应用
   */
  deleteApplication(data: AppIdInput) {
    return axiosUtil.request({
      url: "/application/deleteApplication",
      method: "post",
      data,
      requestType: "delete",
    });
  },
};
