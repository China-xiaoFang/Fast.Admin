import type { RouteRecordRaw } from "vue-router";

/**
 * 默认路由
 * 必须带有 Name 属性
 */
export const defaultRoute: RouteRecordRaw[] = [
	{
		path: "/login",
		name: "Login",
		component: () => import("@/views/login/index.vue"),
		meta: {
			title: "登录",
			authForbidView: true,
			noLogin: true,
		},
	},
	{
		path: "/403",
		component: () => import("@/views/common/403/index.vue"),
		meta: {
			title: "无权限操作",
		},
	},
	{
		path: "/404",
		component: () => import("@/views/common/404/index.vue"),
		meta: {
			title: "页面找不到了",
		},
	},
	{
		path: "/empty",
		component: () => import("@/views/common/empty/index.vue"),
		meta: {
			title: "空页面",
		},
	},
	{
		path: "/redirect/:path(.*)",
		component: () => import("@/views/common/redirect/index.vue"),
		meta: {
			title: "重定向",
			noLogin: true,
		},
	},
	{
		path: "/:path(.*)*",
		redirect: "/404",
	},
];
