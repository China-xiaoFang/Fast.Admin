/// <reference types="@uni-helper/vite-plugin-uni-pages/client" />
import { consoleError, dateUtil, stringUtil } from "@fast-china/utils";
import { createRouter } from "uni-mini-router";
import { pages, subPackages } from "virtual:uni-pages";
import type { PageMetaDatum } from "@uni-helper/vite-plugin-uni-pages";
import { CommonRoute, TabBarRoute } from "@/common";
import { useToast } from "@/hooks";
import { initWebSocket } from "@/signalR";
import { useUserInfo } from "@/stores";

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

const defaultRoutePath = [CommonRoute.Launcher, CommonRoute.WebView, CommonRoute.Login];

/** 路由加载前 */
router.beforeEach(async (to, from, next) => {
	const userInfoStore = useUserInfo();

	// 判断进入的页面是否为 TabBar 页面
	if (TabBarRoute.some((item) => to.path.startsWith(item))) {
		userInfoStore.activeTabBar = to.path;
	}

	// 判断是否存在Token
	if (!userInfoStore.token) {
		// 判断当前页面是否需要登录
		if (!to.noLogin) {
			useToast.warning("请登录");
			// 如果去的路由和来的路由一致，则携带来的路由的参数
			if (from.path === to.path) {
				next({ path: CommonRoute.Login, navType: "replaceAll", query: from.query });
			}
			// 如果是默认路由，则不处理重定向
			else if (defaultRoutePath.includes(to.path)) {
				next({ path: CommonRoute.Login, navType: "replaceAll" });
			} else {
				next({ path: CommonRoute.Login, navType: "replaceAll", query: { redirect: encodeURIComponent(to.fullPath) } });
			}
			return;
		}
	} else {
		// 判断 pinia 中的动态路由生成的状态，必须存在Token才加载
		if (!userInfoStore.hasUserInfo) {
			try {
				// 刷新用户信息
				await userInfoStore.refreshUserInfo();
				// 确保路由添加完成
				userInfoStore.hasUserInfo = true;

				// 初始化 WebSocket
				initWebSocket();

				// 延迟 0.5 秒显示欢迎信息
				setTimeout(() => {
					useToast.show({
						msg: `${dateUtil.getGreet()}${userInfoStore.employeeName}`,
						duration: 1500,
					});
				}, 500);
			} catch (error) {
				next(false);
				consoleError("InitRoute", error);
				// 退出登录
				userInfoStore.logout();
				return;
			}
		}

		// 判断去的页面是否为 Launcher 页面
		if (to.path === CommonRoute.Launcher) {
			// 如果来源页面时选择租户，则重定向到登录页面
			if (from.path === CommonRoute.SelectTenant) {
				next({ path: CommonRoute.Login, navType: "replaceAll" });
				return;
			}
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
		if (to.authForbidView) {
			// 重定向到工作台
			next({ path: CommonRoute.Workbench, navType: "pushTab" });
			return;
		}
	}

	next();
});

/** 路由加载后 */
// eslint-disable-next-line @typescript-eslint/no-empty-function
router.afterEach(() => {});

export default router;
