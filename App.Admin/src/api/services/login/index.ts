import { axiosUtil } from "@fast-china/axios";
import { LoginOutput } from "./models/LoginOutput";
import { LoginInput } from "./models/LoginInput";
import { TenantLoginInput } from "./models/TenantLoginInput";

/**
 * Fast.Center.Service.Login.LoginService 登录服务Api
 */
export const loginApi = {
  /**
   * 登录
   */
  login(data: LoginInput) {
    return axiosUtil.request<LoginOutput>({
      url: "/login",
      method: "post",
      data,
      requestType: "auth",
    });
  },
  /**
   * 租户登录
   */
  tenantLogin(data: TenantLoginInput) {
    return axiosUtil.request<LoginOutput>({
      url: "/tenantLogin",
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
