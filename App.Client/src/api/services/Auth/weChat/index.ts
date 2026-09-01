import { axiosUtil } from "@fast-china/axios";
import type { WeChatCode2PhoneNumberInput } from "./models/WeChatCode2PhoneNumberInput";
import type { WeChatCode2PhoneNumberOutput } from "./models/WeChatCode2PhoneNumberOutput";
import type { WeChatCode2SessionInput } from "./models/WeChatCode2SessionInput";
import type { WeChatCode2SessionOutput } from "./models/WeChatCode2SessionOutput";

/**
 * 微信服务Api
 */
export const weChatApi = {
	/**
	 * 换取微信用户身份信息
	 */
	weChatCode2Session(data: WeChatCode2SessionInput): Promise<WeChatCode2SessionOutput> {
		return axiosUtil.request<WeChatCode2SessionOutput>({
			url: "/weChat/weChatCode2Session",
			method: "post",
			data,
			requestType: "auth",
		});
	},
	/**
	 * 换取微信用户手机号
	 */
	weChatCode2PhoneNumber(data: WeChatCode2PhoneNumberInput): Promise<WeChatCode2PhoneNumberOutput> {
		return axiosUtil.request<WeChatCode2PhoneNumberOutput>({
			url: "/weChat/weChatCode2PhoneNumber",
			method: "post",
			data,
			requestType: "auth",
		});
	},
};
