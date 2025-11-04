declare module "vue-router" {
	interface RouteMeta {
		/**
		 * 模块Id
		 */
		moduleId: number;
		/**
		 * 设置该路由在侧边栏和面包屑中展示的名字
		 */
		title?: string;
		/**
		 * 是否固定在 Tab 中
		 */
		affix?: boolean;
		/**
		 * 图标
		 */
		icon?: string;
		/**
		 * 默认 true，如果设置为 false，则不会在面包屑中显示
		 */
		breadcrumb?: boolean;
		/**
		 * 登录后是否禁止查看该页面
		 * 默认是 false，为 true 是代表登录后此页面不能再进入，否则跳转到首页
		 */
		authForbidView?: boolean;
		/**
		 * 跳过登录
		 * 默认是 false，为 true 是代表不用登录即可进入该页面
		 */
		noLogin?: boolean;
		/**
		 * 默认 false，设置 true 的时候该路由不会在侧边栏出现
		 */
		hide?: boolean;
		/**
		 * 缓存页面
		 */
		keepAlive?: boolean;
	}
}

export {};
