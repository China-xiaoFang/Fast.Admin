import type { RouteRecordRaw } from "vue-router";

/**
 * Layout 模板路由
 * 必须带有 Name 属性
 * 动态添加的
 */
export const layoutRoute: RouteRecordRaw = {
	path: "/",
	name: "layout",
	component: () => import("@/layouts/indexAsync.vue"),
	// 重定向到首页
	redirect: "/dashboard",
	children: [
		{
			path: "/dashboard",
			name: "Dashboard",
			component: () => import("@/views/dashboard/index.vue"),
			meta: {
				title: "首页",
				icon: "g-icon-Dashboard",
				affix: true,
			},
		},
		// {
		// 	path: "/settings",
		// 	name: "Settings",
		// 	redirect: "/settings/account",
		// 	meta: {
		// 		title: "设置",
		// 		hide: true,
		// 	},
		// 	children: [
		// 		{
		// 			path: "/settings/account",
		// 			name: "SettingsAccount",
		// 			component: () => import("@/views/settings/account/index.vue"),
		// 			meta: {
		// 				title: "个人信息",
		// 			},
		// 		},
		// 	],
		// },
	],
};
