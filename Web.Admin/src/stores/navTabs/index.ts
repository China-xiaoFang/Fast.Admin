import { reactive } from "vue";
import { withDefineType } from "@fast-china/utils";
import { defineStore } from "pinia";
import type { RouteLocationNormalized, Router } from "vue-router";
import router, { routerUtil } from "@/router";

export type INavBarTab = Partial<Pick<RouteLocationNormalized, "name" | "path" | "query" | "fullPath" | "meta" | "params">>;

export const useNavTabs = defineStore(
	"navTabs",
	() => {
		const state = reactive({
			/** 激活tab的index */
			activeIndex: -1,
			/** 最后一个激活tab的index */
			lastActiveIndex: -1,
			/** 激活的tab */
			activeTab: withDefineType<INavBarTab>(null),
			/** 导航栏tab列表 */
			navBarTabs: withDefineType<INavBarTab[]>([]),
			/** keep-alive缓存组件名称集合 */
			keepAliveComponentNameList: withDefineType<string[]>([]),
			/** 内容区放大 */
			contentLarge: false,
			/** 当前tab是否全屏 */
			contentFull: false,
		});

		/**
		 * 重置
		 */
		const $reset = (): void => {
			state.activeIndex = -1;
			state.lastActiveIndex = -1;
			state.activeTab = null;
			state.navBarTabs = [];
			state.keepAliveComponentNameList = [];
			state.contentLarge = false;
			state.contentFull = false;
		};

		/**
		 * 刷新 Tab
		 */
		const refreshTab = (route: INavBarTab): void => {
			const fIdx = state.keepAliveComponentNameList.findIndex((f) => f === route.name.toString());
			if (fIdx >= 0) {
				state.keepAliveComponentNameList.splice(fIdx, 1);
			}
			routerUtil.routePushSafe(router, { path: `/redirect${route.path}`, query: route.query });
		};

		/**
		 * 添加 Tab
		 */
		const addTab = (route: INavBarTab): void => {
			const fRouteIdx = state.navBarTabs.findIndex((f) => f.path == route.path);
			//  判断警告页面数量
			if (fRouteIdx === -1) {
				state.navBarTabs.push(routerUtil.pickByRoute(route));
				if (route.meta.keepAlive != false) {
					if (!state.keepAliveComponentNameList.includes(route.name.toString())) {
						state.keepAliveComponentNameList.push(route.name.toString());
					}
				} else {
					const fIdx = state.keepAliveComponentNameList.findIndex((f) => f === route.name.toString());
					if (fIdx >= 0) {
						state.keepAliveComponentNameList.splice(fIdx, 1);
					}
				}
			} else {
				// 存在更新
				state.navBarTabs[fRouteIdx] = routerUtil.pickByRoute(route);
				if (route.meta.keepAlive !== false) {
					if (!state.keepAliveComponentNameList.includes(route.name.toString())) {
						state.keepAliveComponentNameList.push(route.name.toString());
					}
				} else {
					const fIdx = state.keepAliveComponentNameList.findIndex((f) => f === route.name.toString());
					if (fIdx >= 0) {
						state.keepAliveComponentNameList.splice(fIdx, 1);
					}
				}
			}
		};

		/**
		 * 前往最后一个 Tab
		 */
		const toLastTab = (): void => {
			const lastTab = state.navBarTabs.slice(-1)[0];
			if (lastTab) {
				router.push(lastTab?.fullPath ?? lastTab?.path);
			} else {
				router.push({ path: "/" });
			}
		};

		/**
		 * 关闭 Tab
		 */
		const closeTab = (route: INavBarTab): void => {
			if (route?.meta?.affix === true) return;
			const findIndex = state.navBarTabs.findIndex((f) => f.path === route.path);
			if (findIndex >= 0) {
				state.navBarTabs.splice(findIndex, 1);
			}
			const fIdx = state.keepAliveComponentNameList.findIndex((f) => f === route.name.toString());
			if (fIdx >= 0) {
				state.keepAliveComponentNameList.splice(fIdx, 1);
			}
			if (state.lastActiveIndex !== -1 && state.lastActiveIndex !== state.activeIndex && state.lastActiveIndex < state.navBarTabs.length) {
				const lastTab = state.navBarTabs[state.lastActiveIndex];
				router.push(lastTab?.fullPath ?? lastTab?.path);
			} else {
				toLastTab();
			}
		};

		/**
		 * 关闭多个 Tab
		 * @param retainRoute 保留的路由，否则关闭全部标签
		 */
		const closeTabs = (retainRoute: INavBarTab | false = false): void => {
			const affixNavBarTabs = state.navBarTabs.filter((f) => f?.meta?.affix === true);
			state.keepAliveComponentNameList = affixNavBarTabs.filter((f) => f?.meta?.keepAlive !== false).map((m) => m.name.toString());
			if (retainRoute) {
				state.navBarTabs = [...affixNavBarTabs, retainRoute];
				if (retainRoute.meta.keepAlive != false) {
					state.keepAliveComponentNameList.push(retainRoute.name.toString());
				}
			} else {
				state.navBarTabs = [...affixNavBarTabs];
			}
			toLastTab();
		};

		/**
		 * 设置活动路由
		 */
		const setActiveRoute = (route: INavBarTab): void => {
			const fIdx = state.navBarTabs.findIndex((f) => f.path == route.path);
			if (fIdx === -1) return;
			state.activeTab = routerUtil.pickByRoute(route);
			state.lastActiveIndex = state.activeIndex;
			state.activeIndex = fIdx;
		};

		/**
		 * 设置放大
		 */
		const setContentLarge = (contentLarge: boolean): void => {
			state.contentLarge = contentLarge;
		};

		/**
		 * 设置全屏
		 */
		const setContentFull = (contentFull: boolean): void => {
			state.contentFull = contentFull;
		};

		/**
		 * 初始化
		 */
		const initNavBarTabs = (router: Router): void => {
			// 直接默认查找 layout 下的路由，其余的直接忽略
			const allRoutes = router
				.getRoutes()
				.find((f) => f.name === "layout")
				?.children.filter((f) => !f.meta.hide);

			// 扁平化获取固定的标签
			const flRoutes = routerUtil.flattenRoutes(allRoutes);

			const affixNavBarTabs = flRoutes.filter((f) => f.meta?.affix);
			affixNavBarTabs.forEach((item) => {
				if (!state.keepAliveComponentNameList.includes(item.name.toString())) {
					state.keepAliveComponentNameList.push(item.name.toString());
				}
			});

			const oldNavBarTabs = state.navBarTabs.filter((f) => !f.meta?.affix);
			oldNavBarTabs.forEach((item) => {
				if (!state.keepAliveComponentNameList.includes(item.name.toString())) {
					state.keepAliveComponentNameList.push(item.name.toString());
				}
			});

			state.navBarTabs = [...affixNavBarTabs, ...oldNavBarTabs];
		};

		return { state, $reset, refreshTab, addTab, closeTab, closeTabs, setActiveRoute, setContentLarge, setContentFull, initNavBarTabs };
	},
	{
		persist: {
			key: "store-nav-tabs",
		},
	}
);
