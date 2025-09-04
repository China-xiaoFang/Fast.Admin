/// <reference types="vite/client" />

/** 声明 vite 环境变量的类型（如果未声明则默认是 any） */
declare interface ImportMetaEnv {
	/**
	 * 环境
	 */
	readonly VITE_ENV: "production" | "development" | "test" | "staging";
	/**
	 * 运行 npm run dev 时绑定的端口号
	 */
	readonly VITE_PORT: number;
	/**
	 * 网站根目录
	 */
	readonly VITE_PUBLIC_PATH: string;
	/**
	 * 打包路径（静态资源）
	 */
	readonly VITE_BASE_PATH: string;
	/**
	 * CDN 地址
	 */
	readonly VITE_CDN_URL: string;
	/**
	 * 打包输出路径
	 */
	readonly VITE_OUT_DIR: string;
	/**
	 * 接口请求地址
	 */
	readonly VITE_AXIOS_REQUEST_URL: string;
	/**
	 * 接口版本
	 */
	readonly VITE_AXIOS_API_VERSION: string;
	/**
	 * 本地缓存加密
	 */
	readonly VITE_STORAGE_CRYPTO: "true" | "false";
	/**
	 * APP 版本号
	 */
	readonly VITE_APP_VERSION: string;
	/**
	 * APP 来源
	 */
	readonly VITE_APP_ORIGIN: string;
	/**
	 * 接口请求代理地址
	 */
	readonly VITE_AXIOS_PROXY_URL: string;
}
