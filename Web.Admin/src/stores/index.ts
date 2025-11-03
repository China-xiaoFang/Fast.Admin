import { CACHE_PREFIX, base64Util } from "@fast-china/utils";
import { createPinia } from "pinia";
import { createPersistedState } from "pinia-plugin-persistedstate";
import type { App } from "vue";

const storageCrypto = import.meta.env.VITE_STORAGE_CRYPTO == "true";

export const pinia = createPinia();

export const loadPinia = (app: App): void => {
	pinia.use(
		createPersistedState({
			storage: {
				getItem: (key: string) => {
					const result = window.localStorage.getItem(`${CACHE_PREFIX.value}${key}`);
					if (!result) return null;
					return storageCrypto ? base64Util.base64ToStr(result) : result;
				},
				setItem: (key: string, value: string) => {
					window.localStorage.setItem(`${CACHE_PREFIX.value}${key}`, storageCrypto ? base64Util.toBase64(value) : value);
				},
			},
			// 当设置为 true 时，持久化/恢复 Store 时可能发生的任何错误都将使用 console.error 输出。
			debug: true,
		})
	);
	app.use(pinia);
};

export * from "./app";
export * from "./config";
export * from "./navTabs";
export * from "./userInfo";
