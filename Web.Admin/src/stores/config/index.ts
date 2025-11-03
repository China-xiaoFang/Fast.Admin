import { reactive } from "vue";
import { ElMessage } from "element-plus";
import { colorUtil, withDefineType } from "@fast-china/utils";
import { defineStore } from "pinia";
import type { FaTableDataRange } from "fast-element-plus";

export type IModeName = "Classic";

export type IAnimationName =
	| "slide-right"
	| "slide-left"
	| "el-fade-in-linear"
	| "el-fade-in"
	| "el-zoom-in-center"
	| "el-zoom-in-top"
	| "el-zoom-in-bottom";

const defaultMode: IModeName = "Classic";
const defaultAnimation: IAnimationName = "slide-right";
export const defaultThemeColor = "#409EFF";

const defaultLayoutSize = {
	navBarHeight: "35px",
	navBarTabHeight: "30px",
	headerHeight: "calc(65px + var(--el-border-width))",
	menuWidth: "150px",
	menuHeight: "55px",
	mainPadding: "5px",
	footerHeight: "30px",
};

const smallLayoutSize = {
	navBarHeight: "30px",
	navBarTabHeight: "25px",
	headerHeight: "calc(55px + var(--el-border-width))",
	menuWidth: "130px",
	menuHeight: "45px",
	mainPadding: "3px",
	footerHeight: "25px",
};

export const useConfig = defineStore(
	"config",
	() => {
		/** 布局配置 */
		const layout = reactive({
			/** 头部通知栏高度 */
			noticeBarHeight: "20px",
			/** 头部导航栏高度 */
			navBarHeight: "35px",
			/** 头部导航栏高度 */
			navBarTabHeight: "30px",
			/** 头部高度（头部导航高度 + 头部导航栏高度 + 边框高度） */
			headerHeight: "calc(65px + var(--el-border-width))",
			/** 菜单宽度 */
			menuWidth: "150px",
			/** 菜单高度 */
			menuHeight: "55px",
			/** 主页面内容 padding */
			mainPadding: "5px",
			/** 页脚高度 */
			footerHeight: "30px",
			/** 布局方式 */
			layoutMode: defaultMode,
			/** 切换动画 */
			mainAnimation: defaultAnimation,
			/** 主题颜色 */
			themeColor: defaultThemeColor,
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
