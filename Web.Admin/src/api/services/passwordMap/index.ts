import { axiosUtil } from "@fast-china/axios";
import { PagedInput, PagedResult } from "fast-element-plus";
import { PasswordMapModel } from "./models/PasswordMapModel";

/**
 * Fast.Center.Service.PasswordMap.PasswordMapService 密码映射服务Api
 */
export const passwordMapApi = {
  /**
   * 获取密码映射分页列表
   */
  queryPasswordMapPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<PasswordMapModel>>({
      url: "/passwordMap/queryPasswordMapPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
};
