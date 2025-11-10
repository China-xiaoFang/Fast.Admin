import { isString } from "lodash-unified";
import { wdHookState } from "../index";
import type { MessageOptions, MessageResult } from "wot-design-uni/components/wd-message-box/types";

const defaultTitle = "温馨提示";

export const useMessageBox = {
	/**
	 * 显示弹框
	 * @param options 选项
	 */
	show(options: MessageOptions | string): Promise<MessageResult> {
		return new Promise((resolve, reject) => {
			wdHookState.wdMessageBox = {
				type: "show",
				options: isString(options) ? { title: defaultTitle, msg: options } : { ...options, title: options.title ?? defaultTitle },
				then: (res): void => resolve(res),
				catch: (error): void => reject(error),
			};
		});
	},
	/**
	 * Alert 弹框
	 * @param options 选项
	 */
	alert(options: MessageOptions | string): Promise<MessageResult> {
		return new Promise((resolve, reject) => {
			wdHookState.wdMessageBox = {
				type: "alert",
				options: isString(options) ? { title: defaultTitle, msg: options } : { ...options, title: options.title ?? defaultTitle },
				then: (res): void => resolve(res),
				catch: (error): void => reject(error),
			};
		});
	},
	/**
	 * Confirm 弹框
	 * @param options 选项
	 */
	confirm(options: MessageOptions | string): Promise<MessageResult> {
		return new Promise((resolve, reject) => {
			wdHookState.wdMessageBox = {
				type: "confirm",
				options: isString(options) ? { title: defaultTitle, msg: options } : { ...options, title: options.title ?? defaultTitle },
				then: (res): void => resolve(res),
				catch: (error): void => reject(error),
			};
		});
	},
	/**
	 * Prompt 弹框
	 * @param options 选项
	 */
	prompt(options: MessageOptions | string): Promise<MessageResult> {
		return new Promise((resolve, reject) => {
			wdHookState.wdMessageBox = {
				type: "prompt",
				options: isString(options) ? { title: defaultTitle, msg: options } : { ...options, title: options.title ?? defaultTitle },
				then: (res): void => resolve(res),
				catch: (error): void => reject(error),
			};
		});
	},
	/**
	 * 关闭弹框
	 */
	close(): void {
		wdHookState.wdMessageBox = {
			type: "close",
			options: undefined,
			then: undefined,
			catch: undefined,
		};
	},
};
