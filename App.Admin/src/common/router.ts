/**
 * @description 常用路由
 */
export const CommonRoute = {
	/**
	 * App加载页
	 */
	Launcher: "/pages/launcher/index",
	/**
	 * WebView页面
	 */
	WebView: "/pages/webView/index",
	/**
	 * 登录页
	 */
	Login: "/pages/login/index",
	/**
	 * 选择租户
	 */
	SelectTenant: "/pages/selectTenant/index",
	/**
	 * 首页
	 */
	Home: "/pages/tabBar/home/index",
	/**
	 * 工作台
	 */
	Workbench: "/pages/tabBar/workbench/index",
	/**
	 * 我的
	 */
	My: "/pages/tabBar/my/index",
};

/**
 * TabBar 路由
 */
export const TabBarRoute = [CommonRoute.Home, CommonRoute.Workbench, CommonRoute.My];
