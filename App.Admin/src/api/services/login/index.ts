import { axiosUtil } from "@fast-china/axios";
import { LoginOutput } from "./models/LoginOutput";
import { LoginInput } from "./models/LoginInput";
import { LoginTenantOutput } from "./models/LoginTenantOutput";
import { TenantLoginInput } from "./models/TenantLoginInput";
import { WeChatLoginInput } from "./models/WeChatLoginInput";
import { WeChatAuthLoginInput } from "./models/WeChatAuthLoginInput";
import { TryLoginInput } from "./models/TryLoginInput";
import { WeChatClientLoginOutput } from "./models/WeChatClientLoginOutput";

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
			loading: true,
			loadingText: "登录中...",
    });
  },
  /**
   * 获取登录用户根据账号
   */
  queryLoginUserByAccount(accountKey: string) {
    return axiosUtil.request<LoginTenantOutput[]>({
      url: "/queryLoginUserByAccount",
      method: "get",
      params: {
        accountKey,
      },
      requestType: "query",
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
			loading: true,
			loadingText: "租户登录中...",
    });
  },
  /**
   * 微信登录
   */
  weChatLogin(data: WeChatLoginInput) {
    return axiosUtil.request<LoginOutput>({
      url: "/weChatLogin",
      method: "post",
      data,
      requestType: "auth",
			loading: true,
			loadingText: "登录中...",
    });
  },
  /**
   * 微信授权登录
   */
  weChatAuthLogin(data: WeChatAuthLoginInput) {
    return axiosUtil.request<LoginOutput>({
      url: "/weChatAuthLogin",
      method: "post",
      data,
      requestType: "auth",
			loading: true,
			loadingText: "登录中...",
    });
  },
  /**
   * 尝试登录
   */
  tryLogin(data: TryLoginInput) {
    return axiosUtil.request<LoginOutput>({
      url: "/tryLogin",
      method: "post",
      data,
      requestType: "auth",
			showCodeMessage: false,
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
			loading: true,
    });
  },
  /**
   * 微信客户端登录
   */
  weChatClientLogin(data: WeChatLoginInput) {
    return axiosUtil.request<WeChatClientLoginOutput>({
      url: "/weChatClientLogin",
      method: "post",
      data,
      requestType: "auth",
    });
  },
};
