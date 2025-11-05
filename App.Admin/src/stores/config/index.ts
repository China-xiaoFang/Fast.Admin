import { reactive } from "vue";
import { colorUtil, consoleLog, styleToString, withDefineType } from "@fast-china/utils";
import { defineStore } from "pinia";
import { useApp } from "../app";
import { CommonUniApp } from "@/common";
import { useToast } from "@/hooks";

export const defaultThemeColor = "#409EFF";

export const useConfig = defineStore(
	"config",
	() => {
		/** 布局配置 */
		const layout = reactive({
			/** 头部导航栏高度 */
			navBarHeight: 48,
			/** 底部标签栏高度 */
			tabBarHeight: 55,
			/** 页脚高度 */
			footerHeight: 40,
			/** 主题颜色 */
			themeColor: defaultThemeColor,
			/** 主题样式 */
			themeStyle: "",
			/** 导航栏是否跟随主题色自动切换 */
			autoThemeNavBar: false,
			/** 跟随系统设置，自动切换浅色/深色模式 */
			autoThemMode: true,
			/** 是否深色模式 */
			isDark: false,
			/** 是否置灰模式 */
			isGrey: false,
			/** 是否色弱模式 */
			isWeak: false,
			/** 是否显示页脚 */
			footer: true,
			/** 是否显示水印 */
			watermark: true,
		});

		/** 表格配置 */
		const tableLayout = reactive({
			/** Table隐藏图片 */
			hideImage: true,
			/** Table默认时间搜索范围 */
			dataSearchRange: withDefineType<FaTableDataRange>("Past3D"),
		});

		/** 设置主题色 */
		const setTheme = (color: string): void => {
			if (!color) {
				color = defaultThemeColor;
				useToast.success(`主题颜色已重置为 ${defaultThemeColor}`);
			}
			let navbarBgColor;
			if (layout.isDark) {
				navbarBgColor = "#141414";
				uni.setNavigationBarColor({
					frontColor: "#ffffff",
					backgroundColor: navbarBgColor,
				});
			} else {
				navbarBgColor = layout.autoThemeNavBar ? color : "#f8f8f8";
				uni.setNavigationBarColor({
					frontColor: layout.autoThemeNavBar ? "#ffffff" : "#000000",
					backgroundColor: navbarBgColor,
				});
			}

			const appStore = useApp();
			// 头部胶囊padding
			let navbarCapsulePadding = CommonUniApp.navbarCapsuleMenuButtonPadding;
			// #ifdef MP-WEIXIN
			navbarCapsulePadding = appStore.windowInfo.windowWidth - (appStore.menuButton?.right ?? 0);
			// #endif

			const styles = {
				"--wot-color-theme": color,
				"--wot-color-primary": color,
				"--wot-window-width": `${appStore.windowInfo.windowWidth}px`,
				// 如果存在安全距离，页面 100vh 会导致失效，所以这里通过动态计算计算页面剩余高度
				"--wot-window-height": `calc(100vh - ${appStore.windowInfo.safeAreaInsets.bottom}px)`,
				// 状态栏高度
				"--wot-status-bar-height": `${appStore.windowInfo.statusBarHeight}px`,
				// 头部 navbar 背景颜色
				"--wot-navbar-bg-color": navbarBgColor,
				// 头部 navbar 高度
				"--wot-navbar-height": layout.navBarHeight,
				// 头部胶囊按钮边距
				"--wot-navbar-capsule-padding": `${navbarCapsulePadding}px`,
				// 头部胶囊按钮宽度
				"--wot-navbar-capsule-width": `${appStore.menuButton?.width ?? 0}px`,
				// 头部胶囊按钮高度，-2 边框
				"--wot-navbar-capsule-height": `${appStore.menuButton.height - 2}px`,
				/* 导航栏文字颜色 */
				"--wot-navbar-color": "#ffffff",
				// 底部 tabbar 高度
				"--wot-tabbar-height": layout.tabBarHeight,
				// 页脚高度，如果不存在底部安全区域，则默认 + 10px
				"--wot-footer-height": `calc(${layout.footerHeight} + ${appStore.windowInfo.safeAreaInsets.bottom > 0 ? "0px" : "10px"})`,
				// 安全距离
				"--wot-area-inset-left": `${appStore.windowInfo.safeAreaInsets.left}px`,
				"--wot-area-inset-right": `${appStore.windowInfo.safeAreaInsets.right}px`,
				"--wot-area-inset-top": `${appStore.windowInfo.safeAreaInsets.top}px`,
				"--wot-area-inset-bottom": `${appStore.windowInfo.safeAreaInsets.bottom}px`,
			};

			// 计算主题颜色变化
			for (let i = 1; i <= 9; i++) {
				const primaryColor = layout.isDark ? `${colorUtil.getDarkColor(color, i / 10)}` : `${colorUtil.getLightColor(color, i / 10)}`;
				styles[`--wot-color-primary-light-${i}`] = primaryColor;
			}
			for (let i = 1; i <= 9; i++) {
				const primaryColor = layout.isDark ? `${colorUtil.getDarkColor(color, i / 10)}` : `${colorUtil.getLightColor(color, i / 10)}`;
				styles[`--wot-color-primary-dark-${i}`] = primaryColor;
			}

			// 判断是否为置灰模式
			if (layout.isGrey) {
				styles["filter"] = "grayscale(1)";
			}

			// 判断是否为色弱模式
			if (layout.isWeak) {
				styles["filter"] = "invert(80%)";
			}

			layout.themeColor = color;
			layout.themeStyle = styleToString(styles);
		};

		/** 切换深色模式 */
		const switchDark = (): void => {
			if (layout.isDark) {
				// #ifdef APP-PLUS
				plus.nativeUI.setUIStyle("dark");
				plus.navigator.setStatusBarStyle("dark");
				// #endif
			} else {
				// #ifdef APP-PLUS
				plus.nativeUI.setUIStyle("light");
				plus.navigator.setStatusBarStyle("light");
				// #endif
			}
			setTheme(layout.themeColor);
		};

		/** 切换跟随系统变化自动设置浅色/深色模式 */
		const switchAutoThemMode = (): void => {
			if (layout.autoThemMode) {
				// #ifdef APP-PLUS
				plus.nativeUI.setUIStyle("auto");
				// #endif

				// 判断是否启用深色模式
				if (uni.getAppBaseInfo().theme === "dark") {
					layout.isDark = true;
					// #ifdef APP-PLUS
					plus.navigator.setStatusBarStyle("dark");
					// #endif
				} else {
					layout.isDark = false;
					// #ifdef APP-PLUS
					plus.navigator.setStatusBarStyle("light");
					// #endif
				}
				setTheme(layout.themeColor);
			}
		};

		/** 切换置灰或色弱模式 */
		const switchGreyOrWeak = (type: "grey" | "weak", value: boolean): void => {
			if (value) {
				switch (type) {
					case "grey":
						layout.isGrey = true;
						layout.isWeak = false;
						break;
					case "weak":
						layout.isGrey = false;
						layout.isWeak = true;
						break;
				}
			} else {
				layout.isGrey = false;
				layout.isWeak = false;
			}
			setTheme(layout.themeColor);
		};

		/** 初始化主题 */
		const initTheme = (): void => {
			const appStore = useApp();
			if (appStore.isIphone) {
				layout.navBarHeight = CommonUniApp.iosNavBarHeight;
			} else {
				layout.navBarHeight = CommonUniApp.androidNavBarHeight;
			}
			switchAutoThemMode();
			uni.onThemeChange((res) => {
				consoleLog("useConfig", "监听到主题改变", res);
				switchAutoThemMode();
			});
			switchDark();
			if (layout.isGrey) switchGreyOrWeak("grey", true);
			if (layout.isWeak) switchGreyOrWeak("weak", true);
		};

		/** 重置 */
		const reset = (): void => {
			layout.tabBarHeight = 55;
			layout.footerHeight = 40;
			layout.themeColor = defaultThemeColor;
			layout.autoThemeNavBar = false;
			layout.autoThemMode = true;
			layout.isDark = false;
			layout.isGrey = false;
			layout.isWeak = false;
			layout.footer = true;
			layout.watermark = true;
			initTheme();
			tableLayout.hideImage = true;
			tableLayout.dataSearchRange = "Past3D";
		};

		return {
			layout,
			tableLayout,
			screen,
			setTheme,
			switchAutoThemMode,
			switchDark,
			switchGreyOrWeak,
			initTheme,
			reset,
		};
	},
	{
		persist: {
			key: "store-config",
		},
	}
);
