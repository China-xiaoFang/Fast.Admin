declare module "uni-mini-router" {
	interface Route {
		/**
		 * 背景颜色
		 * - 默认是 var(--wot-bg-color-page)
		 */
		backgroundColor?: string;
		/**
		 * 浅色背景图片
		 */
		lightBackgroundImage?: string;
		/**
		 * 深色背景图片
		 */
		darkBackgroundImage?: string;
		/**
		 * 显示页脚
		 * - 默认是 true，为 false 是代表不显示页脚，优先级最高
		 */
		footer?: boolean;
		/**
		 * 显示水印
		 * - 默认是 true，为 false 是代表不显示水印，优先级最高
		 */
		watermark?: boolean;
		/**
		 * 导航页
		 * - 默认是 false，为 true 是显示底部导航栏
		 */
		isTabBar?: boolean;
		/**
		 * 页面滚动
		 * - 默认是 true，为 false 是代表页面高度固定，使用内置滚动
		 */
		pageScroll?: boolean;
		/**
		 * 登录后是否禁止查看该页面
		 * - 默认是 false，为 true 是代表登录后此页面不能再进入，否则跳转到首页
		 */
		authForbidView?: boolean;
		/**
		 * 跳过登录
		 * - 默认是 false，为 true 是代表不用登录即可进入该页面
		 */
		noLogin?: boolean;
	}
}

declare module "@uni-helper/vite-plugin-uni-pages" {
	interface PageMetaDatum {
		/**
		 * 布局组件名称
		 * - 'layout'
		 */
		layout?: string | boolean;
		/**
		 * 背景颜色
		 * - 默认是 var(--wot-bg-color-page)
		 */
		backgroundColor?: string;
		/**
		 * 浅色背景图片
		 */
		lightBackgroundImage?: string;
		/**
		 * 深色背景图片
		 */
		darkBackgroundImage?: string;
		/**
		 * 显示页脚
		 * - 默认是 true，为 false 是代表不显示页脚，优先级最高
		 */
		footer?: boolean;
		/**
		 * 显示水印
		 * - 默认是 true，为 false 是代表不显示水印，优先级最高
		 */
		watermark?: boolean;
		/**
		 * 导航页
		 * - 默认是 false，为 true 是显示底部导航栏
		 */
		isTabBar?: boolean;
		/**
		 * 页面滚动
		 * - 默认是 true，为 false 是代表页面高度固定，使用内置滚动
		 */
		pageScroll?: boolean;
		/**
		 * 登录后是否禁止查看该页面
		 * - 默认是 false，为 true 是代表登录后此页面不能再进入，否则跳转到首页
		 */
		authForbidView?: boolean;
		/**
		 * 跳过登录
		 * - 默认是 false，为 true 是代表不用登录即可进入该页面
		 */
		noLogin?: boolean;
	}
}

export {};
