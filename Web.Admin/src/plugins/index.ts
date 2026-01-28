import { type App, type ComponentPublicInstance, nextTick } from "vue";
import { ElNotification } from "element-plus";
import { useIdentity, useStorage } from "@fast-china/utils";
import { registerComponents } from "@/components";
import { loadFastAxios } from "./axios";
import { loadElementPlus } from "./element-plus";

export function loadPlugins(app: App): void {
	// 全局异常捕获
	app.config.errorHandler = (err: any, instance: ComponentPublicInstance | null, info: string): void => {
		if (!err) return;
		const errorMap: any = {
			InternalError: "Javascript引擎内部错误",
			ReferenceError: "未找到对象",
			TypeError: "使用了错误的类型或对象",
			RangeError: "使用内置对象时，参数超范围",
			SyntaxError: "语法错误",
			EvalError: "错误的使用了Eval",
			URIError: "URI错误",
			AggregateError: "未知的多个错误",
			TimeoutError: "操作超时",
			NetworkError: "网络错误",
			OutOfMemoryError: "内存溢出",
			DOMException: "DOM 操作异常",
			SecurityError: "安全错误，可能涉及跨域或 CSP 限制",
			EventError: "事件处理错误",
		};
		if (err === "cancel") {
			console.warn("操作已取消");
		} else if (err?.name === "AxiosError") {
			return;
		} else {
			const errorName = errorMap[err?.name] || "未知错误";
			console.error(err);
			// instance && consoleError("Handler", instance);
			// info && consoleError("Handler", info);
			nextTick(() => {
				ElNotification({
					title: "系统错误",
					message: errorName,
					duration: 3000,
					position: "top-right",
				});
			});
		}
	};

	const uStorage = useStorage();
	// 缓存前缀
	uStorage.setPrefix("fast__");
	// 缓存是否加密
	uStorage.setCrypto(import.meta.env.VITE_STORAGE_CRYPTO == "true");

	useIdentity().makeIdentity();

	loadElementPlus(app);

	loadFastAxios();

	/** 注册本地全局组件 */
	registerComponents(app);
}
