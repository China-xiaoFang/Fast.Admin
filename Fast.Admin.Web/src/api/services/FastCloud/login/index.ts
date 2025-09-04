import { axiosUtil } from "@fast-china/axios";
import { LoginInput } from "./models/LoginInput";

/**
 * Fast.FastCloud.Api.LoginApplication 登录Api
 */
export const loginApi = {
  /**
   * 登录
   */
  login(data: LoginInput) {
    return axiosUtil.request({
      url: "/login",
      method: "post",
      data,
      requestType: "auth",
    });
  },
  /**
   * 退出登录
   */
  logout() {
    return axiosUtil.request({
      url: "/logout",
      method: "post",
      requestType: "auth",
    });
  },
};
