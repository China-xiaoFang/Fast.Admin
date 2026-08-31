import { ElLoading, ElMessage, ElMessageBox } from "element-plus";
import { createFastAxios } from "@fast-china/axios";
import { AESDecrypt, AESEncrypt, Local, installationIdentity, logger, withDefineType } from "@fast-china/utils";
import { isAxiosError } from "axios";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { useUserInfo } from "@/stores";
import type { ApiResponse } from "@fast-china/axios";
import type { AxiosHeaders, AxiosRequestConfig, AxiosResponse } from "axios";

/** 加载实例 */
const loadingInstance = {
	// ElLoading 的实例信息
	target: withDefineType<ReturnType<typeof ElLoading.service>>(null),
	// 总数
	count: 0,
};

/** 登录回调 */
let loginCallBack = false;

/** 处理重新登录 */
const handleReloadLogin = (response: AxiosResponse): boolean => {
	// 尝试获取 Restful 风格返回Code，或者获取响应状态码
	const code = response?.data?.code || response?.status;
	if (code === 401) {
		if (!loginCallBack) {
			loginCallBack = true;
			ElMessageBox.alert("登录已失效，请重新登录！", {
				title: "温馨提示",
				type: "warning",
				confirmButtonText: "重新登录",
			})
				.then(async () => {
					await useUserInfo().logout();
				})
				.catch((error) => logger.error("Admin", "重新登录处理失败。", error))
				.finally(() => {
					loginCallBack = false;
				});
		}
		return true;
	}
	return false;
};

/** 加载 FastAxios */
export function loadFastAxios(): void {
	let baseUrl = import.meta.env.VITE_API_BASE_URL;
	if (baseUrl?.endsWith("/")) {
		baseUrl = baseUrl.slice(0, -1);
	}

	const fastAxios = createFastAxios({
		baseUrl,
		headers: {
			"Fast-Origin": import.meta.env.DEV ? import.meta.env.VITE_APP_ORIGIN || window.location.host : window.location.host,
			"Fast-Device-Type": Object.entries(AppEnvironmentEnum).find(([, value]) => value === AppEnvironmentEnum.Web)?.[0],
			"Fast-Device-Id": installationIdentity.deviceId,
		},
		requestCipher: true,
	});

	fastAxios.loading.show.use((text) => {
		loadingInstance.count++;
		if (loadingInstance.count === 1) {
			// 合并 Loading 配置
			loadingInstance.target = ElLoading.service({
				fullscreen: true,
				lock: true,
				text: text ?? "加载中...",
				background: "rgba(0, 0, 0, 0.7)",
			});
		} else {
			loadingInstance.target.setText(text ?? "加载中...");
		}
	});
	fastAxios.loading.close.use((_options) => {
		if (loadingInstance.count > 0) loadingInstance.count--;
		if (loadingInstance.count === 0) {
			loadingInstance.target.close();
			loadingInstance.target = null;
		}
	});

	fastAxios.message.success.use((message) => ElMessage.success(message));
	fastAxios.message.warning.use((message) => ElMessage.warning(message));
	fastAxios.message.info.use((message) => ElMessage.info(message));
	fastAxios.message.error.use((message) => ElMessage.error(message));

	fastAxios.cache.get.use((key) => Local.get(`HTTP_CACHE_${key}`));
	fastAxios.cache.set.use((key, value) => Local.set(`HTTP_CACHE_${key}`, value, { ttlMs: 24 * 60 * 60 * 1000 }));

	fastAxios.crypto.encrypt.use((config: AxiosRequestConfig, timestamp) => {
		const requestData = config.data ?? config.params;
		const dataStr = JSON.stringify(requestData);
		if (dataStr !== undefined && dataStr !== "" && dataStr !== "{}") {
			logger.debug("Fast-Axios", `HTTP request data("${config.url}")`, requestData);
			const decryptData = AESEncrypt(dataStr, `${timestamp}`, `FIV${timestamp}`);
			// 组装请求格式
			const encryptedRequestData = {
				data: decryptData,
				timestamp,
			};
			switch (config.method.toUpperCase()) {
				case "GET":
				case "DELETE":
				case "HEAD":
					config.params = encryptedRequestData;
					break;
				case "POST":
				case "PUT":
				case "PATCH":
					config.data = encryptedRequestData;
					break;
				case "OPTIONS":
				case "CONNECT":
				case "TRACE":
					throw new Error("This request mode is not supported.");
			}
			// 请求头部增加加密标识
			config.headers["Fast-Request-Encipher"] = "true";
		}
	});

	fastAxios.crypto.decrypt.use((response, _options) => {
		const restfulData = response.data as ApiResponse;
		const responseHeader = response.headers as AxiosHeaders;
		// 判断响应头部是否有加密标识
		if (responseHeader.get("Fast-Response-Encipher")?.toString()?.toLowerCase() === "true") {
			if (!restfulData?.data) {
				return restfulData;
			}
			restfulData.data = AESDecrypt(restfulData.data as string, `${restfulData.timestamp}`, `FIV${restfulData.timestamp}`).parseJson();
			// 处理 ""xxx"" 这种数据
			if (typeof restfulData.data === "string" && restfulData.data.startsWith('"') && restfulData.data.endsWith('"')) {
				restfulData.data = restfulData.data.replace(/"/g, "");
			}
			logger.debug("Fast-Axios", `HTTP response data("${response.config.url}")`, restfulData.data);
		}
		return restfulData;
	});

	fastAxios.interceptors.request.use((config) => {
		const userInfoStore = useUserInfo();
		const { token, refreshToken } = userInfoStore.resolveToken();
		if (token) {
			config.headers["Authorization"] = token;
		}
		// 刷新 Token
		refreshToken && (config.headers["X-Authorization"] = refreshToken);
	});

	fastAxios.interceptors.response.use((response, _options) => {
		const userInfoStore = useUserInfo();
		userInfoStore.setToken(response);
		return handleReloadLogin(response) ? (response?.data ?? response) : undefined;
	});

	fastAxios.interceptors.responseError.use((error, _options) => {
		if (isAxiosError(error) && error.response) {
			// 避免报错的同时刷新Token
			const userInfoStore = useUserInfo();
			userInfoStore.setToken(error.response);
		}
		return isAxiosError(error) && error.response && handleReloadLogin(error.response) ? (error.response.data ?? error.response) : undefined;
	});
}
