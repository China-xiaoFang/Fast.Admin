import { reactive } from "vue";
import { withDefineType } from "@fast-china/utils";
import type { Message, MessageOptions, MessageResult } from "wot-design-uni/components/wd-message-box/types";
import type { NotifyProps } from "wot-design-uni/components/wd-notify/types";
import type { Toast, ToastOptions } from "wot-design-uni/components/wd-toast/types";

export * from "./use-loading";
export * from "./use-message-box";
export * from "./use-notify";
export * from "./use-overlay";
export * from "./use-paging";
export * from "./use-toast";

export const wdHookState = reactive({
	loading: {
		/**
		 * 状态
		 * @default false
		 */
		state: false,
		/**
		 * 加载文字
		 * @default "加载中..."
		 */
		text: "加载中...",
		/**
		 * 全屏Loading，和默认Loading不一样
		 * @default false
		 */
		fullscreen: false,
	},
	overlay: {
		/**
		 * 状态
		 * @default false
		 */
		state: false,
		/**
		 * 透明度（0 ~ 1）
		 * @default 0
		 */
		transparent: 0,
	},
	wdNotify: {
		type: withDefineType<"showNotify" | "closeNotify">(),
		options: withDefineType<NotifyProps | string>(),
	},
	wdToast: {
		type: withDefineType<keyof Toast>(),
		options: withDefineType<ToastOptions | string>(),
	},
	wdMessageBox: {
		type: withDefineType<keyof Message>(),
		options: withDefineType<MessageOptions>(),
		then: withDefineType<(res?: MessageResult) => void>(),
		catch: withDefineType<(error?: any) => void>(),
	},
});
