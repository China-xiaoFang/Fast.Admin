<template>
	<FaWatermark v-if="showWatermark" />
	<wd-config-provider :customClass="`fa-layout fa-layout__${theme}`" :theme="theme" :customStyle="configStore.layout.themeStyle">
		<view :class="['fa-main', { 'fa-main__tabBar': state.isTabBar, 'fa-main__page-scroll': state.pageScroll === false }]" :style="mainStyle">
			<slot />
			<!-- 页脚 -->
			<FaFooter v-if="showFooter" />
			<!-- 底部导航栏 -->
			<FaTabBar v-if="state.isTabBar" />
		</view>
		<!-- 消息通知 -->
		<wd-notify selector="#fast_notify" />
		<!-- 轻提示 -->
		<wd-toast selector="#fast_toast" />
		<!-- 消息弹窗 -->
		<wd-message-box selector="#fast_message_box" />
		<!-- 蒙版层加载 -->
		<FaLoading :loading="!wdHookState.loading?.fullscreen && wdHookState.loading?.state" :text="wdHookState.loading?.text" />
		<!-- 全屏加载 -->
		<FaLoadingPage :loading="wdHookState.loading?.fullscreen && wdHookState.loading?.state" :text="wdHookState.loading?.text" />
		<!-- 遮罩层 -->
		<FaOverlay :visible="wdHookState.overlay?.state" :transparent="wdHookState.overlay?.transparent" />
	</wd-config-provider>
</template>

<script setup lang="ts">
import { onHide, onShow } from "@dcloudio/uni-app";
import { computed, onBeforeMount, reactive, watch } from "vue";
import { withDefineType } from "@fast-china/utils";
import { useRoute } from "uni-mini-router";
import { useMessage, useNotify, useToast } from "wot-design-uni";
import type { WatchHandle } from "vue";
import { wdHookState } from "@/hooks";
import { useConfig } from "@/stores";

defineOptions({
	name: "Layout",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const configStore = useConfig();

const state = reactive({
	/** 页面滚动 */
	pageScroll: true,
	/** 显示页脚 */
	footer: true,
	/** 显示水印 */
	watermark: true,
	/** 导航页 */
	isTabBar: false,
	/** 背景颜色 */
	backgroundColor: "var(--wot-bg-color-page)",
	/** 浅色背景图片 */
	lightBackgroundImage: withDefineType<string>(),
	/** 深色背景图片 */
	darkBackgroundImage: withDefineType<string>(),
});

/** 主题 */
const theme = computed(() => (configStore.layout.isDark ? "dark" : "light"));

/** 显示页脚 */
const showFooter = computed(() => configStore.layout.footer && state.footer !== false);

/** 显示水印 */
const showWatermark = computed(() => configStore.layout.watermark && state.watermark !== false);

/** 主页面样式 */
const mainStyle = computed(() => {
	const style = {};
	let height = "var(--wot-window-height)";
	if (showFooter.value) {
		height += " - var(--wot-footer-height)";
		if (state.isTabBar) {
			height += " - 15px";
		}
	}
	if (state.isTabBar) {
		height += " - var(--wot-tabbar-height)";
	}
	style["--main-height"] = `calc(${height})`;
	if (!configStore.layout.isDark && state.lightBackgroundImage) {
		style["background-image"] = `url('${state.lightBackgroundImage}')`;
	} else if (configStore.layout.isDark && state.darkBackgroundImage) {
		style["background-image"] = `url('${state.darkBackgroundImage}')`;
	} else if (state.backgroundColor) {
		style["background-color"] = state.backgroundColor;
	} else {
		style["background-color"] = "var(--wot-bg-color-page)";
	}
	return style;
});

let wdNotifyWatch: WatchHandle;
const uNotify = useNotify("#fast_notify");
let wdToastWatch: WatchHandle;
const uToast = useToast("#fast_toast");
let wdMessageBoxWatch: WatchHandle;
const uMessage = useMessage("#fast_message_box");

onShow(() => {
	wdNotifyWatch = watch(
		() => wdHookState.wdNotify,
		(newValue) => {
			if (!newValue) return;
			if (newValue.type === "closeNotify") {
				uNotify.closeNotify();
			} else {
				uNotify[newValue.type](newValue.options);
			}
			wdHookState.wdNotify = undefined;
		}
	);
	wdToastWatch = watch(
		() => wdHookState.wdToast,
		(newValue) => {
			if (!newValue) return;
			if (newValue.type === "close") {
				uToast.close();
			} else {
				uToast[newValue.type](newValue.options);
			}
			wdHookState.wdToast = undefined;
		}
	);
	wdMessageBoxWatch = watch(
		() => wdHookState.wdMessageBox,
		(newValue) => {
			if (!newValue) return;
			if (newValue.type === "close") {
				uMessage.close();
			} else {
				uMessage[newValue.type](newValue.options)
					.then((res) => {
						newValue?.then(res);
					})
					.catch((error) => {
						newValue?.catch(error);
					});
			}
			wdHookState.wdMessageBox = undefined;
		}
	);
});

onHide(() => {
	/** 页面隐藏，取消监听 */
	wdNotifyWatch && wdNotifyWatch();
	wdToastWatch && wdToastWatch();
	wdMessageBoxWatch && wdMessageBoxWatch();
});

onBeforeMount(() => {
	const { pageScroll, footer, watermark, isTabBar, backgroundColor, lightBackgroundImage, darkBackgroundImage } = useRoute();
	state.pageScroll = pageScroll;
	state.footer = footer;
	state.watermark = watermark;
	state.isTabBar = isTabBar;
	state.backgroundColor = backgroundColor;
	state.lightBackgroundImage = lightBackgroundImage;
	state.darkBackgroundImage = darkBackgroundImage;
});
</script>

<style scoped lang="scss">
@import "./layout.scss";
</style>
