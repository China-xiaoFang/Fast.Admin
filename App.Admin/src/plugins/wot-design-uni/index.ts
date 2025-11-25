import WdInput from "wot-design-uni/components/wd-input/wd-input.vue";
import WdTextarea from "wot-design-uni/components/wd-textarea/wd-textarea.vue";

/** 加载 wot-design-uni */
export function loadWotDesign(): void {
	WdInput.props = {
		...WdInput.props,
		/**
		 * 默认显示统计字数
		 */
		showWordLimit: {
			type: Boolean,
			default: true,
		},
	};

	WdTextarea.props = {
		...WdTextarea.props,
		/**
		 * 默认显示统计字数
		 */
		showWordLimit: {
			type: Boolean,
			default: true,
		},
	};
}
