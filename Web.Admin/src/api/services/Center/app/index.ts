import { axiosUtil } from "@fast-china/axios";
import type { LaunchOutput } from "./models/LaunchOutput";

/**
 * AppApi
 */
export const appApi = {
	/**
	 * Launch
	 */
	launch(): Promise<LaunchOutput> {
		return axiosUtil.request<LaunchOutput>({
			url: "/launch",
			method: "post",
			requestType: "auth",
		});
	},
};
