import { reactive } from "vue";
import { ElMessage } from "element-plus";
import { colorUtil, withDefineType } from "@fast-china/utils";
import { defineStore } from "pinia";
import type { componentSizes } from "element-plus";
import type { FaTableDataRange } from "fast-element-plus";

export type IModeName = "Classic";

export type IAnimationName =
	| "slide-right"
	| "slide-left"
	| "slide-bottom"
	| "slide-top"
	| "el-fade-in-linear"
	| "el-fade-in"
	| "el-zoom-in-center"
	| "el-zoom-in-top"
	| "el-zoom-in-bottom";

const defaultSize: (typeof componentSizes)[number] = "default";
const defaultMode: IModeName = "Classic";
const defaultAnimation: IAnimationName = "slide-right";
export const defaultThemeColor = "#409EFF";

const defaultLayoutSize = {
	navBarHeight: 45,
	navBarTabHeight: 35,
	menuWidth: 210,
	menuHeight: 55,
	mainPadding: 5,
	footerHeight: 30,
};

const smallLayoutSize = {
	navBarHeight: 40,
	navBarTabHeight: 30,
	menuWidth: 180,
	menuHeight: 45,
	mainPadding: 3,
	footerHeight: 25,
};

export const useConfig = defineStore(
	"config",
	() => {
		/** 布局配置 */
		const layout = reactive({
			/** 跟随分辨率自动切换大小 */
			autoSize: true,
			/** 布局大小 */
			layoutSize: withDefineType<(typeof componentSizes)[number]>(defaultSize),
			/** 头部导航栏高度 */
			navBarHeight: 45,
			/** 头部导航栏标签高度 */
			navBarTabHeight: 35,
			/** 菜单宽度 */
			menuWidth: 210,
			/** 菜单高度 */
			menuHeight: 55,
			/** 主页面内容 padding */
			mainPadding: 5,
			/** 页脚高度 */
			footerHeight: 30,
			/** 布局方式 */
			layoutMode: withDefineType<IModeName>(defaultMode),
			/** 切换动画 */
			mainAnimation: withDefineType<IAnimationName>(defaultAnimation),
			/** 主题颜色 */
			themeColor: withDefineType<string>(defaultThemeColor),
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
			/** Table显示搜索 */
			showSearch: true,
			/** Table抽屉式高级搜索 */
			advancedSearchDrawer: false,
			/** Table高级搜索默认折叠 */
			defaultCollapsedSearch: true,
			/** Table隐藏图片 */
			hideImage: true,
			/** Table默认时间搜索范围 */
			dataSearchRange: withDefineType<FaTableDataRange>("Past3D"),
		});

		/** 屏幕 */
		const screen = reactive({
			/** 锁屏密码 */
			password: "",
			/** 屏幕锁定 */
			screenLock: false,
		});

		/** 设置布局方式 */
		const setLayoutMode = (mode: IModeName): void => {
			// 暂且这里只赋值
			layout.layoutMode = mode;
		};

		/** 设置主题色 */
		const setTheme = (color: string): void => {
			if (!color) {
				color = defaultThemeColor;
				ElMessage({ type: "success", message: `主题颜色已重置为 ${defaultThemeColor}` });
			}
			// 计算主题颜色变化
			document.documentElement.style.setProperty("--el-color-primary", color);
			for (let i = 1; i <= 9; i++) {
				const primaryColor = layout.isDark ? `${colorUtil.getDarkColor(color, i / 10)}` : `${colorUtil.getLightColor(color, i / 10)}`;
				document.documentElement.style.setProperty(`--el-color-primary-light-${i}`, primaryColor);
			}
			for (let i = 1; i <= 9; i++) {
				const primaryColor = layout.isDark ? `${colorUtil.getDarkColor(color, i / 10)}` : `${colorUtil.getLightColor(color, i / 10)}`;
				document.documentElement.style.setProperty(`--el-color-primary-dark-${i}`, primaryColor);
			}
			layout.themeColor = color;
		};

		const darkModeMediaQuery = window.matchMedia("(prefers-color-scheme: dark)");

		/** 切换深色模式 */
		const switchDark = (): void => {
			const html = document.documentElement;
			if (layout.isDark) {
				html.classList.add("dark");
			} else {
				html.classList.remove("dark");
			}
			setTheme(layout.themeColor);
		};

		/** 切换跟随系统变化自动设置浅色/深色模式 */
		const switchAutoThemMode = (): void => {
			if (layout.autoThemMode) {
				// 判断是否启用深色模式
				if (darkModeMediaQuery.matches) {
					layout.isDark = true;
				} else {
					layout.isDark = false;
				}
				switchDark();
			}
		};

		/** 切换置灰或色弱模式 */
		const switchGreyOrWeak = (type: "grey" | "weak", value: boolean): void => {
			const body = document.body;
			if (!value) return body.removeAttribute("style");
			const styles: Record<"grey" | "weak", string> = {
				grey: "filter: grayscale(1)",
				weak: "filter: invert(80%)",
			};
			body.setAttribute("style", styles[type]);
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
		};

		/** 初始化主题 */
		const initTheme = (): void => {
			switchAutoThemMode();
			darkModeMediaQuery.addEventListener("change", switchAutoThemMode);
			switchDark();
			if (layout.isGrey) switchGreyOrWeak("grey", true);
			if (layout.isWeak) switchGreyOrWeak("weak", true);
		};

		/** 重置 */
		const reset = (): void => {
			layout.autoSize = true;
			layout.layoutSize = defaultSize;
			layout.menuWidth = 210;
			layout.menuHeight = 55;
			layout.mainAnimation = defaultAnimation;
			layout.themeColor = defaultThemeColor;
			layout.autoThemMode = true;
			layout.isDark = false;
			layout.isGrey = false;
			layout.isWeak = false;
			initTheme();
			tableLayout.showSearch = true;
			tableLayout.advancedSearchDrawer = false;
			tableLayout.defaultCollapsedSearch = true;
			tableLayout.hideImage = true;
			tableLayout.dataSearchRange = "Past3D";
		};

		/** 设置默认布局大小 */
		const setDefaultLayoutSize = (): void => {
			Object.keys(defaultLayoutSize).forEach((key) => {
				layout[key] = defaultLayoutSize[key];
			});
		};

		/** 设置小的布局大小 */
		const setSmallLayoutSize = (): void => {
			Object.keys(smallLayoutSize).forEach((key) => {
				layout[key] = smallLayoutSize[key];
			});
		};

		return {
			layout,
			tableLayout,
			screen,
			setLayoutMode,
			setTheme,
			switchAutoThemMode,
			switchDark,
			switchGreyOrWeak,
			initTheme,
			reset,
			setDefaultLayoutSize,
			setSmallLayoutSize,
		};
	},
	{
		persist: {
			key: "store-config",
		},
	}
);
