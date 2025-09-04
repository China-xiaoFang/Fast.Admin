import { axiosUtil } from "@fast-china/axios";
import { GetLoginUserInfoOutput } from "./models/GetLoginUserInfoOutput";

/**
 * Fast.FastCloud.Api.AuthApplication 授权Api
 */
export const authApi = {
  /**
   * 获取登录用户信息
   */
  getLoginUserInfo() {
    return axiosUtil.request<GetLoginUserInfoOutput>({
      url: "/getLoginUserInfo",
      method: "get",
      requestType: "auth",
    });
  },
};
