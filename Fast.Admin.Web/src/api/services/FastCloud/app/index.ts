import { axiosUtil } from "@fast-china/axios";
import { LaunchOutput } from "./models/LaunchOutput";

/**
 * Fast.FastCloud.Api.AppApplication AppApi
 */
export const appApi = {
  /**
   * Launch
   */
  launch() {
    return axiosUtil.request<LaunchOutput>({
      url: "/app/launch",
      method: "get",
      requestType: "auth",
    });
  },
};
