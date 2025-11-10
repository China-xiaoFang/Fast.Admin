/// <reference types="@uni-helper/vite-plugin-uni-pages/client" />
import { stringUtil } from "@fast-china/utils";
import { isNil } from "lodash-unified";
import { createRouter } from "uni-mini-router";
import { pages, subPackages } from "virtual:uni-pages";
import type { PageMetaDatum } from "@uni-helper/vite-plugin-uni-pages";
import { loginApi } from "@/api/services/login";
import { TabBarRoute } from "@/common";
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

/** 路由加载前 */
router.beforeEach(async (to, from, next) => {
	const userInfoStore = useUserInfo();

	// 判断进入的页面是否为 TabBar 页面
	if (TabBarRoute.some((item) => to.path.startsWith(item))) {
		userInfoStore.activeTabBar = to.path;
	}

	// 判断是否存在Token
	if (!userInfoStore.token) {
		try {
			const weChatCode = await userInfoStore.getWeChatCode();
			if (weChatCode) {
				await loginApi.weChatClientLogin({ weChatCode });
			} else {
				useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
			}
		} finally {
			userInfoStore.delWeChatCode();
		}
	}

	// 初始化 WebSocket
	initWebSocket();

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
	if (to.mobileRequired && isNil(userInfoStore.mobile)) {
		// 弹出授权登录弹窗
		userInfoStore.authLoginPopup = true;
		return;
	}

	next();
});

/** 路由加载后 */
// eslint-disable-next-line @typescript-eslint/no-empty-function
router.afterEach(() => {});

export default router;
