<template>
	<wd-popup
		v-bind="wdPopupProps"
		:customClass="`fa-popup ${props.customClass}`"
		:customStyle="`${state.style} ${props.customStyle}`"
		v-model="state.visible"
		:transition="state.transition"
		safeAreaInsetBottom
		@before-enter="() => emit('beforeEnter')"
		@enter="() => emit('enter')"
		@after-enter="() => emit('afterEnter')"
		@before-leave="() => emit('beforeLeave')"
		@leave="() => emit('leave')"
		@after-leave="() => emit('afterLeave')"
		@click-modal="() => emit('clickModal')"
		@close="handleClose"
	>
		<slot />
		<FaLoading mask :loading="state.loading" />
	</wd-popup>
</template>

<script setup lang="ts">
import { addUnit, consoleError, definePropType, execFunction, useProps } from "@fast-china/utils";
import { computed, nextTick, reactive } from "vue";
import { popupProps } from "wot-design-uni/components/wd-popup/types";
import FaLoading from "../loading/index.vue";

defineOptions({
	name: "Popup",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const props = defineProps({
	...popupProps,
	/** @description 宽度 */
	width: [String, Number],
	/** @description 高度 */
	height: [String, Number],
	/** @description 显示关闭回调 */
	showBeforeClose: Boolean,
	/** @description 打开之后 */
	afterOpen: {
		type: definePropType<() => void>(Function),
	},
});

const wdPopupProps = useProps(props, popupProps);

const emit = defineEmits({
	/** @description 进入前触发 */
	beforeEnter: (): boolean => true,
	/** @description 进入时触发 */
	enter: (): boolean => true,
	/** @description 进入后触发 */
	afterEnter: (): boolean => true,
	/** @description 离开前触发 */
	beforeLeave: (): boolean => true,
	/** @description 离开时触发 */
	leave: (): boolean => true,
	/** @description 离开后触发 */
	afterLeave: (): boolean => true,
	/** @description 点击遮罩时触发 */
	clickModal: (): boolean => true,
	/** @description 弹出层打开时触发 */
	open: (): boolean => true,
	/** @description 弹出层关闭时触发 */
	close: (): boolean => true,
});

const state = reactive({
	loading: false,
	visible: false,
	style: computed(() => {
		let result = "";
		if (props.width) {
			result += `width: ${addUnit(props.width)};`;
		}
		if (props.height) {
			result += `height: ${addUnit(props.height)};`;
		}

		return result;
	}),
	transition: computed(() => {
		switch (props.position) {
			case "center":
				return "zoom-in";
			case "top":
				return "fade-down";
			case "bottom":
				return "fade-up";
			case "left":
				return "fade-left";
			case "right":
				return "fade-right";
			default:
				return "fade";
		}
	}),
});

const handleOpen = (openFunction?: () => void | Promise<void>): void => {
	state.visible = true;
	nextTick(() => {
		state.loading = true;
		execFunction(props.afterOpen ?? openFunction)
			.then(() => {
				emit("open");
			})
			.catch((error) => {
				consoleError("FaPopup", error);
				// 自动关闭
				state.visible = false;
			})
			.finally(() => {
				state.loading = false;
			});
	});
};

const handleClose = (closeFunction?: () => void | Promise<void>): void => {
	state.loading = true;
	execFunction(closeFunction)
		.then(() => {
			emit("close");
			state.visible = false;
		})
		.catch((error) => {
			consoleError("FaPopup", error);
		})
		.finally(() => {
			state.loading = false;
		});
};

const handleLoading = (loadingFunction: () => void | Promise<void>): void => {
	state.loading = true;
	execFunction(loadingFunction)
		.then()
		.catch((error) => {
			consoleError("FaPopup", error);
		})
		.finally(() => {
			state.loading = false;
		});
};

defineExpose({
	/** @description 加载状态 */
	loading: computed(() => state.loading),
	/** @description 是否显示 */
	visible: computed(() => state.visible),
	/** @description 打开弹窗 */
	open: handleOpen,
	/** @description 关闭弹窗 */
	close: handleClose,
	/** @description 弹窗加载 */
	doLoading: handleLoading,
});
</script>
