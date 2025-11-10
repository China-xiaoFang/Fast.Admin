import { wdHookState } from "../index";

export const useOverlay = {
	/**
	 * 显示
	 * @param transparent 透明度
	 */
	show(transparent = 0): void {
		wdHookState.overlay = {
			state: true,
			transparent,
		};
	},
	/**
	 * 隐藏
	 */
	hide(): void {
		if (wdHookState.overlay?.state) {
			wdHookState.overlay = undefined;
		}
	},
};
