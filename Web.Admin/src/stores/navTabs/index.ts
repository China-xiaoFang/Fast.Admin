import { reactive, toRefs } from "vue";
import { withDefineType } from "@fast-china/utils";
import { defineStore } from "pinia";
import type { RouteLocationNormalized, Router } from "vue-router";
import router, { routerUtil } from "@/router";

export type INavTab = Partial<Pick<RouteLocationNormalized, "name" | "path" | "query" | "fullPath" | "meta" | "params">>;

export const useNavTabs = defineStore(
	"navTabs",
	() => {
		const state = reactive({
			/** 激活模块Id */
			activeModuleId: withDefineType<string | number>(),
			/** 激活tab的index */
			activeIndex: -1,
			/** 最后一个激活tab的index */
			lastActiveIndex: -1,
			/** 激活的tab */
			activeTab: withDefineType<INavTab>(null),
			/** 导航栏tab列表 */
			navTabs: withDefineType<INavTab[]>([]),
			/** keep-alive缓存组件名称集合 */
			keepAliveComponentNameList: withDefineType<string[]>([]),
			/** 内容区放大 */
			contentLarge: false,
			/** 当前tab是否全屏 */
			contentFull: false,
		});

		/** 重置 */
		const $reset = (): void => {
			state.activeIndex = -1;
			state.lastActiveIndex = -1;
			state.activeModuleId = router.currentRoute.value.meta?.moduleId;
			state.activeTab = null;
			state.navTabs = [];
			state.keepAliveComponentNameList = [];
			state.contentLarge = false;
			state.contentFull = false;
		};

		/** 刷新 Tab */
		const refreshTab = (route: INavTab): void => {
			const fIdx = state.keepAliveComponentNameList.findIndex((f) => f === route.name.toString());
			if (fIdx >= 0) {
				state.keepAliveComponentNameList.splice(fIdx, 1);
			}
			routerUtil.routePushSafe(router, { path: `/redirect${route.path}`, query: route.query });
		};

		/** 添加 Tab */
		const addTab = (route: INavTab): void => {
			if (route.meta?.breadcrumb === false) return;
			const fRouteIdx = state.navTabs.findIndex((f) => f.path == route.path);
			//  判断警告页面数量
			if (fRouteIdx === -1) {
				state.navTabs.push(routerUtil.pickByRoute(route));
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
				state.navTabs[fRouteIdx] = routerUtil.pickByRoute(route);
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

		/** 前往最后一个 Tab */
		const toLastTab = (): void => {
			const lastTab = state.navTabs.slice(-1)[0];
			if (lastTab) {
				router.push(lastTab?.fullPath ?? lastTab?.path);
			} else {
				router.push({ path: "/" });
			}
		};

		/** 关闭 Tab */
		const closeTab = (route: INavTab): void => {
			if (route?.meta?.affix === true) return;
			const findIndex = state.navTabs.findIndex((f) => f.path === route.path);
			if (findIndex >= 0) {
				state.navTabs.splice(findIndex, 1);
			}
			const fIdx = state.keepAliveComponentNameList.findIndex((f) => f === route.name.toString());
			if (fIdx >= 0) {
				state.keepAliveComponentNameList.splice(fIdx, 1);
			}
			if (state.lastActiveIndex !== -1 && state.lastActiveIndex !== state.activeIndex && state.lastActiveIndex < state.navTabs.length) {
				const lastTab = state.navTabs[state.lastActiveIndex];
				router.push(lastTab?.fullPath ?? lastTab?.path);
			} else {
				toLastTab();
			}
		};

		/**
		 * 关闭多个 Tab
		 * @param retainRoute 保留的路由，否则关闭全部标签
		 * @param direction 方向，可选：'left' | 'right' | false
		 */
		const closeTabs = (retainRoute: INavTab | false = false, direction: "left" | "right" | false = false): void => {
			const affixNavTabs = state.navTabs.filter((f) => f?.meta?.affix === true);
			if (retainRoute) {
				const retainRouteIndex = state.navTabs.findIndex((f) => f.path === retainRoute.path);
				let newTabs: INavTab[] = [];
				if (retainRouteIndex === -1) {
					state.navTabs = [...affixNavTabs];
					state.keepAliveComponentNameList = affixNavTabs.filter((f) => f?.meta?.keepAlive !== false).map((m) => m.name.toString());
				} else if (direction === "left") {
					newTabs = [...affixNavTabs, ...state.navTabs.slice(retainRouteIndex)];
				} else if (direction === "right") {
					newTabs = [...affixNavTabs, ...state.navTabs.slice(0, retainRouteIndex + 1)];
				} else {
					newTabs = [...affixNavTabs, retainRoute];
				}

				const _newTabs = newTabs.filter((item, idx, arr) => arr.findIndex((f) => f.path === item.path) === idx);
				state.navTabs = _newTabs;
				state.keepAliveComponentNameList = _newTabs.filter((f) => f?.meta?.keepAlive !== false).map((m) => m.name.toString());
			} else {
				state.navTabs = [...affixNavTabs];
				state.keepAliveComponentNameList = affixNavTabs.filter((f) => f?.meta?.keepAlive !== false).map((m) => m.name.toString());
			}
			toLastTab();
		};

		/** 设置活动路由 */
		const setActiveRoute = (route: INavTab): void => {
			const fIdx = state.navTabs.findIndex((f) => f.path == route.path);
			if (fIdx === -1) return;
			state.activeModuleId = route.meta?.moduleId;
			state.activeTab = routerUtil.pickByRoute(route);
			state.lastActiveIndex = state.activeIndex;
			state.activeIndex = fIdx;
		};

		/** 设置放大 */
		const setContentLarge = (contentLarge: boolean): void => {
			state.contentLarge = contentLarge;
		};

		/** 设置全屏 */
		const setContentFull = (contentFull: boolean): void => {
			state.contentFull = contentFull;
		};

		/** 初始化 */
		const initNavTabs = (router: Router): void => {
			// 直接默认查找 layout 下的路由，其余的直接忽略
			const allRoutes = router
				.getRoutes()
				.find((f) => f.name === "layout")
				?.children.filter((f) => !f.meta.hide);

			// 扁平化获取固定的标签
			const flRoutes = routerUtil.flattenRoutes(allRoutes);

			const affixNavTabs = flRoutes.filter((f) => f.meta?.affix);
			affixNavTabs.forEach((item) => {
				if (!state.keepAliveComponentNameList.includes(item.name.toString())) {
					state.keepAliveComponentNameList.push(item.name.toString());
				}
			});

			const oldNavTabs = state.navTabs.filter((f) => !f.meta?.affix);
			oldNavTabs.forEach((item) => {
				if (!state.keepAliveComponentNameList.includes(item.name.toString())) {
					state.keepAliveComponentNameList.push(item.name.toString());
				}
			});

			state.navTabs = [...affixNavTabs, ...oldNavTabs];

			state.activeModuleId = router.currentRoute.value.meta?.moduleId;
		};

		return {
			...toRefs(state),
			$reset,
			refreshTab,
			addTab,
			closeTab,
			closeTabs,
			setActiveRoute,
			setContentLarge,
			setContentFull,
			initNavTabs,
		};
	},
	{
		persist: {
			key: "store-nav-tabs",
		},
	}
);
