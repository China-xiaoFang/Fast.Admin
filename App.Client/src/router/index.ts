/// <reference types="@uni-helper/vite-plugin-uni-pages/client" />
import { stringUtil } from "@fast-china/utils";
import { createRouter } from "uni-mini-router";
import { pages, subPackages } from "virtual:uni-pages";
import { TabBarRoute } from "@/common";
import { useUserInfo } from "@/stores";
import type { PageMetaDatum } from "@uni-helper/vite-plugin-uni-pages";

const generateRoutes = (): PageMetaDatum[] => {
	const routes = pages.map((page) => {
		const newPath = `/${page.path}`;
		return { ...page, path: newPath };
	});
	if (subPackages && subPackages.length > 0) {
		subPackages.forEach((subPackage) => {
			const subRoutes = subPackage.pages.map((page: any) => {
				const newPath = `/${subPackage.root}/${page.path}`;
				return { ...page, path: newPath };
			});
			routes.push(...subRoutes);
		});
	}
	return routes;
};

const router = createRouter({
	routes: generateRoutes(),
});

/** 路由加载前 */
router.beforeEach(async (to, from, next) => {
	const userInfoStore = useUserInfo();

	// 判断进入的页面是否为 TabBar 页面
	if (TabBarRoute.some((item) => to.path.startsWith(item))) {
		userInfoStore.activeTabBar = to.path;
	}

	// 判断是否存在重定向路径，如果有则跳转
	const redirect = stringUtil.deepDecodeURIComponent((from.query.redirect as string) || "");
	if (redirect && redirect != to.fullPath) {
		const _query = stringUtil.getUrlParams(redirect);
		// 判断是否为 TabBar 页面
		if (TabBarRoute.some((item) => redirect.startsWith(item))) {
			next({ path: redirect, navType: "pushTab", query: _query });
		} else {
			next({ path: redirect, navType: "replace", query: _query });
		}
		return;
	}

	// 判断登录后是否禁止查看该页面
	if (to.mobileRequired && !userInfoStore.authLoginCheck()) return;

	next();
});

/** 路由加载后 */
// eslint-disable-next-line @typescript-eslint/no-empty-function
router.afterEach(() => {});

export default router;
