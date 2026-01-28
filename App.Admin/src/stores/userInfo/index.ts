import { Local, base64Util, consoleError } from "@fast-china/utils";
import { defineStore } from "pinia";
import { reactive, ref, toRefs } from "vue";
import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { authApi } from "@/api/services/Auth/auth";
import { loginApi } from "@/api/services/Auth/login";
import { CommonRoute } from "@/common";
import { useMessageBox, useToast } from "@/hooks";
import router from "@/router";
import { closeWebSocket } from "@/signalR";
import { useApp } from "../app";
import type { GetLoginUserInfoOutput } from "@/api/services/Auth/auth/models/GetLoginUserInfoOutput";
import type { LoginOutput } from "@/api/services/Auth/login/models/LoginOutput";
import type { AxiosResponse } from "axios";

type IState = {
	/** Token */
	token: string;
	/** Refresh Token */
	refreshToken: string;
	/** 激活的 TabBar 页面 */
	activeTabBar: string;
};

export const useUserInfo = defineStore(
	"userInfo",
	() => {
		const state = reactive<IState & GetLoginUserInfoOutput>({
			token: "",
			refreshToken: "",
			activeTabBar: CommonRoute.Workbench,
			accountKey: "",
			mobile: "",
			nickName: "",
			avatar: "",
			tenantNo: "",
			tenantName: "",
			userKey: "",
			account: "",
			employeeNo: "",
			employeeName: "",
			departmentId: 0,
			departmentName: "",
			isSuperAdmin: false,
			isAdmin: false,
			roleNameList: [],
			buttonCodeList: [],
			menuList: [],
		});

		/** 是否存在用户信息 */
		const hasUserInfo = ref(false);

		/** WebSocket 是否连接 */
		const hasWebSocket = ref(false);

		const tabBars = reactive<ITabBar[]>([
			{
				path: CommonRoute.Home,
				icon: "home",
				title: "首页",
				disable: true,
			},
			{
				path: CommonRoute.Workbench,
				icon: "workbench",
				title: "工作台",
				bulge: true,
			},
			{
				path: CommonRoute.My,
				icon: "my",
				title: "我的",
			},
		]);

		/** 设置用户信息 */
		const setUserInfo = (uInfo: GetLoginUserInfoOutput): void => {
			Object.keys(uInfo).forEach((key) => {
				state[key] = uInfo[key];
			});
		};

		/** 删除 Token */
		const removeToken = (): void => {
			state.token = "";
			state.refreshToken = "";
		};

		/** 设置 Token */
		const setToken = (axiosResponse: AxiosResponse): void => {
			if (!axiosResponse) return;
			// 从请求头部中获取 Token
			const token = (axiosResponse.headers.get as (headerName: string) => string)("access-token");
			// 从请求头部中获取 Refresh Token
			const refreshToken = (axiosResponse.headers.get as (headerName: string) => string)("x-access-token");
			// 判断是否为无效 Token
			if (token === "invalid_token") {
				// 删除 Token
				removeToken();
			} else if (token && refreshToken && refreshToken !== "invalid_token") {
				// 设置 Token
				state.token = token;
				state.refreshToken = refreshToken;
			}
		};

		/**
		 * 获取 Token
		 * @description 从缓存中获取
		 */
		const getToken = (): { token: string; refreshToken: string } => {
			return { token: state.token, refreshToken: state.refreshToken };
		};

		/**
		 * 解析Token
		 * @description 如果Token过期，会解析不出来
		 * @param token 可以传入，也可以直接获取 pinia 中的
		 * @param refreshToken 可以传入，也可以直接获取 pinia 中的
		 */
		const resolveToken = (token: string = null, refreshToken: string = null): { token: string; refreshToken: string; tokenData: any } => {
			token ??= state.token;
			refreshToken ??= state.refreshToken;
			if (token) {
				const jwtToken = decodeURIComponent(encodeURIComponent(base64Util.atob(token.replace(/_/g, "/").replace(/-/g, "+").split(".")[1])));
				const jwtTokenData = JSON.parse(jwtToken);
				// 获取 Token 的过期时间
				const exp = new Date(jwtTokenData.exp * 1000);
				if (new Date() >= exp) {
					return { token: `Bearer ${token}`, refreshToken: `Bearer ${refreshToken}`, tokenData: jwtTokenData };
				}
				return { token: `Bearer ${token}`, refreshToken: null, tokenData: jwtTokenData };
			}
			return { token: null, refreshToken: null, tokenData: null };
		};

		/** 假登录 @description 缓存 Account 相关信息 */
		const fakeLogin = (loginRes: LoginOutput): void => {
			if (!loginRes) return;
			state.accountKey = loginRes.accountKey;
			state.nickName = loginRes.nickName;
			state.avatar = loginRes.avatar;
		};

		/** 登录 */
		const login = (loginRes: LoginOutput): void => {
			if (!loginRes && loginRes.status !== LoginStatusEnum.Success) return;
			// 优先缓存一些数据
			state.accountKey = loginRes.accountKey;
			state.nickName = loginRes.nickName;
			state.avatar = loginRes.avatar;
			const userRes = loginRes.tenantList[0];
			state.userKey = userRes.userKey;
			state.tenantName = userRes.tenantName;
			state.employeeNo = userRes.employeeNo;
			state.employeeName = userRes.employeeName;
			useToast.success("登录成功");
			// 确保 getLoginUser 获取用户信息
			hasUserInfo.value = false;
			// 跳转到工作台
			router.pushTab(CommonRoute.Workbench);
		};

		const logoutClear = (): void => {
			removeToken();
			// 删除 HTTP 缓存数据
			Local.removeByPrefix("HTTP_CACHE_");

			// 排除登录页面报错的问题
			if (router.route.value.path === CommonRoute.Login) {
				router.push(CommonRoute.Login);
			} else {
				router.replaceAll({ path: CommonRoute.Login, query: { redirect: encodeURIComponent(router.route.value.fullPath) } });
			}
		};

		/** 退出登录 */
		const logout = async (
			data: {
				/**
				 * 1 强制下线
				 * 2 其他地方登录
				 */
				type: 1 | 2;
				message: string;
			} = null
		): Promise<void> => {
			if (data?.type === 2) {
				useMessageBox.alert(data?.message);
				try {
					// 关闭 WebSocket 连接
					await closeWebSocket();
				} catch (error) {
					consoleError("Logout", error);
				}
				logoutClear();
			} else {
				try {
					// 关闭 WebSocket 连接
					await closeWebSocket();
				} catch (error) {
					consoleError("Logout", error);
				}
				try {
					await loginApi.logout();
				} catch (error) {
					consoleError("Logout", error);
				} finally {
					logoutClear();
					if (data !== null) {
						useMessageBox.alert(data?.message);
					}
				}
			}
		};

		/** 刷新用户信息 */
		const refreshUserInfo = async (): Promise<void> => {
			const apiRes = await authApi.getLoginUserInfo();
			setUserInfo(apiRes);
		};

		/** 刷新应用 */
		const refreshApp = async (): Promise<void> => {
			// 删除 HTTP 缓存数据
			Local.removeByPrefix("HTTP_CACHE_");

			// 刷新用户信息
			await refreshUserInfo();

			// 刷新字典
			const appStore = useApp();
			await appStore.setDictionary();
		};

		/** 切换登录 @description 调用此方法下次才不会 tryLogin */
		const switchLogin = (): void => {
			// 确保下次会自动刷新用户信息
			hasUserInfo.value = false;
			Object.keys(state).forEach((key) => {
				delete state[key];
			});
			logoutClear();
		};

		/** 获取微信Code */
		const getWeChatCode = (): Promise<string> => {
			return new Promise((resolve, reject) => {
				// #ifdef MP-WEIXIN
				return uni.login({
					success: (res) => {
						return resolve(res.code);
					},
					fail: (error) => {
						useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
						return reject();
					},
				});
				// #endif

				// #ifndef MP-WEIXIN
				return resolve("");
				// #endif
			});
		};

		return {
			...toRefs(state),
			hasUserInfo,
			hasWebSocket,
			tabBars,
			setUserInfo,
			removeToken,
			setToken,
			getToken,
			resolveToken,
			fakeLogin,
			login,
			logout,
			logoutClear,
			refreshUserInfo,
			refreshApp,
			switchLogin,
			getWeChatCode,
		};
	},
	{
		persist: {
			key: "store-user-info",
			// 这里是配置 pinia 只需要持久化 state 中的 Token 即可，而不是整个 store
			paths: [
				"token",
				"refreshToken",
				"accountKey",
				"nickName",
				"avatar",
				"tenantName",
				"userKey",
				"employeeNo",
				"employeeName",
				"departmentName",
				"isSuperAdmin",
				"isAdmin",
			],
		},
	}
);
