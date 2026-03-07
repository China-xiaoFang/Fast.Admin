import { ElNotification } from "element-plus";
import { consoleError, stringUtil } from "@fast-china/utils";
import { cloneDeep, pick } from "lodash";
import { NavigationFailureType, isNavigationFailure } from "vue-router";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import router from "@/router";
import { useUserInfo } from "@/stores";
import { layoutRoute } from "../modules/layoutRoute";
import type { AuthMenuInfoDto } from "@/api/services/Auth/auth/models/AuthMenuInfoDto";
import type { NavigationFailure, RouteLocationNormalized, RouteLocationRaw, RouteRecordRaw, Router } from "vue-router";

const modules = import.meta.glob("/src/views/**/*.vue");

/** 加载组件 */
const loadComponent = (component: string): any => {
	if (component) {
		if (component.includes("/")) {
			return modules[`/src/views/${component}.vue`];
		}
		return [`/src/views/${component}/index.vue`];
	} else {
		return () => import("@/views/common/empty/index.vue");
	}
};

/** 加载组件名称 */
export const loadComponentName = (name: string): string => {
	if (name) {
		if (name.includes("/")) {
			const cArr = name.replace(/(^|\/)index(?=\/|$)/gi, "").split("/");
			let result = "";
			cArr.forEach((item) => {
				result += item.slice(0, 1).toUpperCase() + item.slice(1);
			});
			return result;
		}
		return name;
	} else {
		return stringUtil.generateRandomString(8);
	}
};

/** 解析路由地址，分离路径和查询参数 */
export const parseRouterUrl = (routerUrl: string): { path: string; query?: Record<string, string> } => {
	if (!routerUrl) return { path: routerUrl };
	const qIdx = routerUrl.indexOf("?");
	if (qIdx === -1) return { path: routerUrl };
	const queryString = routerUrl.substring(qIdx + 1);
	const query = queryString ? Object.fromEntries(new URLSearchParams(queryString)) : undefined;
	return { path: routerUrl.substring(0, qIdx), query };
};

/**
 * 组装路由
 */
const packageMenu = (menuList: AuthMenuInfoDto[]): RouteRecordRaw[] => {
	const routeList: RouteRecordRaw[] = [];

	for (const item of menuList) {
		if ((item.menuType & (MenuTypeEnum.Catalog | MenuTypeEnum.Menu)) == 0) {
			continue;
		}
		// 解析路由地址，分离路径和查询参数，避免 Vue Router 将查询参数作为路径的一部分导致 404
		const { path: routerBasePath } = parseRouterUrl(item.router ?? "");
		const routeInfo = {
			path: item.menuType === MenuTypeEnum.Catalog ? stringUtil.generateRandomString(8) : routerBasePath,
			// 使用 menuCode 作为路由 name，确保每个菜单项拥有唯一 name（即使多个菜单共用同一组件）
			// 这样导航时 to.name / to.meta 能正确反映当前菜单项信息
			name: loadComponentName(item.menuCode || item.component),
			component: loadComponent(item.component),
			redirect: undefined,
			meta: {
				moduleId: item.moduleId,
				title: item.menuTitle || item.menuName,
				icon: item.icon,
				tab: item.tab,
				hide: item.visible,
				keepAlive: item.keepAlive,
				// 保存组件名称用于 keep-alive（keep-alive 的 include 需要匹配组件的 name 选项，而非路由 name）
				componentName: loadComponentName(item.component || item.menuCode),
			},
			children: [],
		};

		// 判断是否存在子节点
		if (item.children && item.children.length > 0) {
			const childrenRoutes = packageMenu(item.children);
			routeInfo.children.push(...childrenRoutes);
			routeInfo.redirect = routeInfo.children[0].path;
			delete routeInfo.component;
			routeInfo.meta.keepAlive = false;
		}

		routeList.push(routeInfo);
	}

	return routeList;
};

/**
 * 处理动态路由
 */
export const handleDynamicRoute = (): void => {
	const userInfoStore = useUserInfo();

	const deepLayoutRoute = cloneDeep(layoutRoute);

	// 组装路由，循环添加到 layout 中
	const layoutRoutes = packageMenu(userInfoStore.menuList.flatMap((m) => m.children));
	layoutRoutes.forEach((rItem) => {
		deepLayoutRoute.children.push(rItem);
	});

	// 尝试移除
	if (router.hasRoute(deepLayoutRoute.name)) {
		router.removeRoute(deepLayoutRoute.name);
	}

	router.addRoute(deepLayoutRoute);
};

/**
 * 路由工具类
 */
export const routerUtil = {
	/**
	 * 路由跳转，带错误检查
	 * @param router 路由对象 useRouter()，因必须在 setup 中获取才存在值
	 * @param to 导航位置，同 router.push
	 */
	routePushSafe(router: Router, to: RouteLocationRaw): Promise<NavigationFailure | void | undefined> {
		if (!router) {
			consoleError("routerUtil", new Error("useRouter undefined."));
			return Promise.resolve();
		}
		router
			.push(to)
			.then((failure) => {
				if (failure) {
					if (isNavigationFailure(failure, NavigationFailureType.aborted)) {
						ElNotification({
							message: "导航失败，导航守卫拦截！",
							type: "error",
						});
					} else if (isNavigationFailure(failure, NavigationFailureType.duplicated)) {
						ElNotification({
							message: "导航失败，已在导航目标位置！",
							type: "warning",
						});
					}
				}
				return Promise.resolve(failure);
			})
			.catch((error) => {
				ElNotification({
					message: "导航失败，路由无效！",
					type: "error",
				});
				consoleError("routerUtil", error);
				return Promise.reject(error);
			});
	},
	/**
	 * route 部分属性，解决警告
	 */
	pickByRoute(route: Partial<RouteLocationNormalized>): Partial<RouteLocationNormalized> {
		return pick(route, ["name", "path", "query", "fullPath", "meta", "params"]);
	},
	/**
	 * 扁平化路由
	 */
	flattenRoutes(routes: RouteRecordRaw[]): RouteRecordRaw[] {
		const resRoutes: RouteRecordRaw[] = [];

		routes.forEach((item) => {
			if (item?.children?.length > 0) {
				const newItem = { ...item };
				delete newItem?.children;
				resRoutes.push(newItem);
				resRoutes.push(...this.flattenRoutes(item?.children));
			} else {
				resRoutes.push(item);
			}
		});

		return resRoutes;
	},
};
