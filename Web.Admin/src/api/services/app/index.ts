import { axiosUtil } from "@fast-china/axios";
import { LaunchOutput } from "./models/LaunchOutput";

/**
 * Fast.Center.Service.App.AppService 应用服务Api
 */
export const appApi = {
  /**
   * Launch
   */
  launch() {
    return axiosUtil.request<LaunchOutput>({
      url: "/launch",
      method: "post",
      requestType: "auth",
    });
  },
};
