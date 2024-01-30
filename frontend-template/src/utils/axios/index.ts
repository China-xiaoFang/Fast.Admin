import type { AxiosRequestConfig, AxiosResponse, InternalAxiosRequestConfig } from "axios";
import axios from "axios";
import type { Options, LoadingInstance } from "@/utils/axios/interface";
import { ElLoading, ElMessage, ElMessageBox, type LoadingOptions } from "element-plus";
import { useConfig } from "@/stores/config";
import { useUserInfo } from "@/stores/userInfo";
import { getAxiosDefaultConfig } from "@/utils/axios/config";
import { downloadFile } from "@/utils";
import base64 from "@/utils/base64";

const pendingMap = new Map();

/**
 * 登录弹出层的变量
 */
let loginCallBack = false;

/**
 * 加载实例
 */
const loadingInstance: LoadingInstance = {
    target: null,
    count: 0,
};

function createAxios<Data = any, T = ApiPromise<Data>>(axiosConfig: AxiosRequestConfig, options: Options = {}, loading: LoadingOptions = {}): T {
    const timestamp = new Date().getTime();
    const configStore = useConfig();
    const userInfoStore = useUserInfo();
    // 获取 Axios 默认配置
    const axiosDefaultConfig = getAxiosDefaultConfig();

    // 创建 Axios 请求
    const Axios = axios.create({
        baseURL: import.meta.env.VITE_AXIOS_BASE_URL,
        timeout: import.meta.env.VITE_AXIOS_API_TIMEOUT,
        headers: {
            // 携带浏览器语言环境表示
            "Fast-Lang": configStore.lang.defaultLang,
            // 携带接口来源
            "Fast-Api-Origin": "Web",
            // 携带请求来源
            "Fast-App-Origin": window.location.origin,
        },
        responseType: "json",
    });

    // 合并选项
    options = Object.assign(configStore.axios.options, options);

    /**
     * 请求拦截
     */
    Axios.interceptors.request.use(
        (config: InternalAxiosRequestConfig<any>) => {
            // 删除重复请求
            removePending(config);

            // 判断是否开启取消重复请求
            options.cancelDuplicateRequest && addPending(config);

            // 判断是否显示loading层
            if (options.loading) {
                loadingInstance.count++;
                if (loadingInstance.count === 1) {
                    // 合并 Loading 配置
                    loading = Object.assign(axiosDefaultConfig.loading, loading);

                    loadingInstance.target = ElLoading.service(loading);
                }
            }

            if (config.headers) {
                // Token 处理
                const { token, refreshToken, tokenData } = userInfoStore.resolveToken();
                if (token) {
                    config.headers["Authorization"] = token;
                    config.headers["Fast-TenantNo"] = tokenData["TenantNo"] && base64.toBase64(tokenData["TenantNo"]);
                    config.headers["Fast-JobNumber"] = tokenData["JobNumber"] && base64.toBase64(tokenData["JobNumber"]);
                }
                // 刷新 Token
                refreshToken && (config.headers["X-Authorization"] = refreshToken);

                // TODO：这里待增加设备ID
            }

            // TODO:请求参数加密

            // GET 请求缓存处理
            if (config.method.toUpperCase() === "GET") {
                config.params = config.params || {};
                config.params._ = timestamp;
            }

            return config;
        },
        (error) => {
            return Promise.reject(error);
        }
    );

    /**
     * 响应拦截
     */
    Axios.interceptors.response.use(
        (response: AxiosResponse) => {
            // 删除重复请求标识
            removePending(response.config);
            // 关闭loading层
            options.loading && closeLoading(options);
            // 判断是否包含错误白名单路径
            if (axiosDefaultConfig.errorWhiteUrls.includes(response.config.url)) {
                return Promise.resolve(true);
            }
            // 设置 Token
            userInfoStore.setToken(response);

            // 重新登录处理
            if (
                reloadLoginHandle(
                    response,
                    axiosDefaultConfig.reloadLoginCodes,
                    axiosDefaultConfig.reloadLoginMessage,
                    axiosDefaultConfig.reloadLoginButtonText,
                    () => {
                        userInfoStore.logout();
                    }
                )
            ) {
                return Promise.reject(false);
            }

            switch (response.config.responseType) {
                // 配置了blob，不处理直接返回文件流
                case "blob":
                    if (response.status === 200) {
                        // 判断是否自动下载
                        if (configStore.axios.options.autoDownloadFile === true) {
                            // 下载文件
                            downloadFile(response);
                        }
                        return Promise.resolve(response);
                    } else {
                        ElMessage.error(axiosDefaultConfig.errorCodeMessages["fileDownloadError"]);
                        return Promise.reject(false);
                    }
                // 正常 JSON 格式响应处理
                case "json":
                    // 获取 Restful 风格返回数据
                    const data = response.data;
                    const code = data.code;
                    if (code < 200 || code > 299) {
                        // 判断是否显示错误消息
                        if (options.showCodeMessage) {
                            // 判断返回的 message 是否为对象类型
                            if (typeof data.message === "object" && data.message) {
                                ElMessage.error(JSON.stringify(data.message));
                            } else {
                                ElMessage.error(data.message);
                            }
                        }
                        return Promise.resolve(data);
                    }

                    // TODO:请求响应解密

                    // 简洁响应
                    return options.simpleDataFormat ? Promise.resolve(data) : Promise.resolve(response);
                default:
                    // 简洁响应
                    return options.simpleDataFormat ? Promise.resolve(response.data) : Promise.resolve(response);
            }
        },
        (error) => {
            // 删除重复请求标识
            error?.config && removePending(error?.config);
            // 关闭loading层
            options.loading && closeLoading(options);
            // 判断是否包含错误白名单路径
            if (axiosDefaultConfig.errorWhiteUrls.includes(error?.config?.url)) {
                return Promise.resolve(true);
            }
            // 重新登录处理
            if (
                reloadLoginHandle(
                    error?.response,
                    axiosDefaultConfig.reloadLoginCodes,
                    axiosDefaultConfig.reloadLoginMessage,
                    axiosDefaultConfig.reloadLoginButtonText,
                    () => {
                        userInfoStore.logout();
                    }
                )
            ) {
                return Promise.reject(false);
            }
            // 处理错误状态码
            options.showErrorMessage && httpErrorStatusHandle(error, axiosDefaultConfig.errorCodeMessages);
            // 错误继续返回给到具体页面
            return Promise.reject(error);
        }
    );

    return Axios(axiosConfig) as T;
}

/**
 * 默认导出
 */
export default createAxios;

/**
 * Http 错误状态码处理
 * @param error
 * @param errorCodeMessages
 */
function httpErrorStatusHandle(error: any, errorCodeMessages: anyObj) {
    // 判断请求是否被取消
    if (axios.isCancel(error)) {
        console.error(errorCodeMessages["cancelDuplicate"]);
        return;
    }
    let message = "";
    // 判断是否断网
    if (!window.navigator.onLine) {
        message = errorCodeMessages["offLine"];
    } else {
        // 其他错误码处理
        // 尝试获取 Restful 风格返回Code，或者获取响应状态码
        const code = error?.response?.data?.code || error?.response?.status || "default";
        message = errorCodeMessages[code];
    }
    ElMessage.error(message);
}

/**
 * 重新登录处理
 * @param response
 * @param reloadLoginCodes
 * @param reloadLoginMessage
 * @param reloadLoginButtonText
 * @param loginCallBackFunc
 */
function reloadLoginHandle(
    response: anyObj,
    reloadLoginCodes: Array<number>,
    reloadLoginMessage: string,
    reloadLoginButtonText: string,
    loginCallBackFunc: Function
): boolean {
    // 尝试获取 Restful 风格返回Code，或者获取响应状态码
    const code = response?.data?.code || response?.status;
    if (code && reloadLoginCodes.includes(code)) {
        if (!loginCallBack) {
            loginCallBack = true;
            ElMessageBox.alert(reloadLoginMessage, {
                type: "warning",
                showClose: false,
                confirmButtonText: reloadLoginButtonText,
                callback: () => {
                    loginCallBack = false;
                    loginCallBackFunc();
                },
            });
        }
        return true;
    }
    return false;
}

/**
 * 关闭Loading层实例
 */
function closeLoading(options: Options) {
    if (options.loading && loadingInstance.count > 0) loadingInstance.count--;
    if (loadingInstance.count === 0) {
        loadingInstance.target.close();
        loadingInstance.target = null;
    }
}

/**
 * 储存每个请求的唯一cancel回调, 以此为标识
 */
function addPending(axiosConfig: AxiosRequestConfig) {
    const pendingKey = getPendingKey(axiosConfig);
    axiosConfig.cancelToken =
        axiosConfig.cancelToken ||
        new axios.CancelToken((cancel) => {
            if (!pendingMap.has(pendingKey)) {
                pendingMap.set(pendingKey, cancel);
            }
        });
}

/**
 * 删除重复的请求
 */
function removePending(axiosConfig: AxiosRequestConfig) {
    const pendingKey = getPendingKey(axiosConfig);
    if (pendingMap.has(pendingKey)) {
        const cancelToken = pendingMap.get(pendingKey);
        cancelToken(pendingKey);
        pendingMap.delete(pendingKey);
    }
}

/**
 * 生成每个请求的唯一key
 */
function getPendingKey(axiosConfig: AxiosRequestConfig) {
    let { data } = axiosConfig;
    const { url, method, params, headers } = axiosConfig;
    // response里面返回的config.data是个字符串对象
    if (typeof data === "string") data = JSON.parse(data);
    return [
        url,
        method,
        headers && (headers as anyObj)["Authorization"] ? (headers as anyObj)["Authorization"] : "",
        headers && (headers as anyObj)["X-Authorization"] ? (headers as anyObj)["X-Authorization"] : "",
        JSON.stringify(params),
        JSON.stringify(data),
    ].join("&");
}
