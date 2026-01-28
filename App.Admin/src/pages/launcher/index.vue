<template>
	<view class="page">
		<view class="logo">
			<image v-if="appStore.logoUrl" class="img_logo" :src="appStore.logoUrl" @error="state.logoUrl = defaultLogo" />
			<image v-else class="img_logo" :src="defaultLogo" />
			<text class="app_name">{{ appStore.appName }}</text>
		</view>
		<view v-if="state.loading" class="btn_text">{{ state.btnText }}</view>
		<wd-button v-else type="primary" :round="false" @tap="appLaunch">进入</wd-button>
		<FaFooter />
	</view>
</template>

<script setup lang="ts">
import { onLoad } from "@dcloudio/uni-app";
import { consoleError, consoleLog, consoleWarn } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { reactive } from "vue";
import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { loginApi } from "@/api/services/Auth/login";
import { CommonRoute } from "@/common";
import { useLoading, useMessageBox } from "@/hooks";
import defaultLogo from "@/static/logo.png";
import { useApp, useUserInfo } from "@/stores";

definePage({
	name: "Launcher",
	layout: "layout",
	lightBackgroundImage: "/static/images/launcher_bg.png",
	darkBackgroundImage: "/static/images/launcher_bg_dark.png",
	footer: false,
	watermark: false,
	pageScroll: false,
	noLogin: true,
	style: {
		navigationBarTitleText: "Fast.App",
	},
});

const appStore = useApp();
const userInfoStore = useUserInfo();
const router = useRouter();

const state = reactive({
	/** 加载中 */
	loading: true,
	/** 按钮文字 */
	btnText: "",
	// launch 接口异常，则跳过更新
	skipUpdateCheck: false,
	/** Logo 图片 */
	logoUrl: appStore.logoUrl,
});

const openStartPage = async () => {
	state.btnText = "进入页面";
	state.loading = false;
	// 判断是否存在 token, userKey
	const { token, userKey } = userInfoStore;
	if (token && userKey) {
		useLoading.show("尝试自动登录中...");
		try {
			// 验证登录
			const loginRes = await loginApi.tryLogin({
				userKey,
			});
			if (loginRes.status === LoginStatusEnum.Success) {
				userInfoStore.login(loginRes);
				return;
			} else {
				consoleWarn("Launcher", "尝试缓存登录失败", loginRes);
			}
		} catch (error) {
			consoleError("Launcher", "尝试缓存登录失败");
			consoleError("Launcher", error);
		} finally {
			useLoading.hide();
		}

		// 尝试微信自动登录
		// #ifdef MP-WEIXIN
		const weChatCode = await userInfoStore.getWeChatCode();
		if (weChatCode) {
			useLoading.show("微信授权自动登录中...");
			const loginRes = await loginApi
				.weChatLogin({
					weChatCode,
				})
				.finally(() => useLoading.hide());
			if (loginRes.status === LoginStatusEnum.Success) {
				userInfoStore.login(loginRes);
				return;
			} else {
				useMessageBox.alert(loginRes.message).then(() => {
					router.replaceAll({
						path: CommonRoute.Login,
					});
				});
				return;
			}
		} else {
			consoleWarn("Launcher", "授权失败，无法获取您的信息。请手动授权以继续使用我们的服务。");
			router.replaceAll({
				path: CommonRoute.Login,
			});
			return;
		}

		// #endif
	}

	router.replaceAll({
		path: CommonRoute.Login,
	});
};

/** 检查小程序版本 */
const checkMiniAppVersion = () => {
	state.btnText = "检查更新";
	const updateManager = uni.getUpdateManager();
	if (!updateManager) {
		state.btnText = "更新检测异常";
	}
	// 检查小程序是否有新版本
	updateManager.onCheckForUpdate((res) => {
		if (res.hasUpdate) {
			state.btnText = "下载更新";
			updateManager.onUpdateReady(() => {
				consoleLog("Launcher", "小程序存在新版本");
				useMessageBox
					.alert({
						title: "更新提示",
						msg: "新版本已经准备好，需要重启后才能正常使用应用。",
					})
					.then(() => {
						updateManager.applyUpdate(); //调用 applyUpdate 应用新版本并重启
					});
			});
			updateManager.onUpdateFailed(() => {
				consoleError("Launcher", "小程序更新检测异常");
				useMessageBox
					.alert({
						msg: "系统异常，无法更新新版本，是否重启应用？",
						confirmButtonText: "重启应用",
					})
					.then(() => {
						updateManager.applyUpdate(); //调用 applyUpdate 应用新版本并重启
					});
			});
		} else {
			consoleLog("Launcher", "小程序已是最新");
			state.btnText = "已是最新";
			openStartPage();
		}
	});
};

// /** 热更新 */
// const plusUpdate = (url: string) => {
// 	consoleLog("Launcher", `热更新：${url}`);
// 	axiosUtil
// 		.request<string>({
// 			url,
// 			method: "download",
// 			timeout: 5000,
// 			requestType: "download",
// 			simpleDataFormat: false,
// 			requestCipher: false,
// 			loading: true,
// 			loadingText: "下载资源文件中...",
// 		})
// 		.then((res) => {
// 			plus.runtime.install(
// 				res,
// 				{},
// 				() => {
// 					consoleLog("Launcher", "APP存在新版本");
// 					useMessageBox
// 						.alert({
// 							title: "更新提示",
// 							msg: "新版本已经准备好，需要重启后才能正常使用应用。",
// 						})
// 						.then(() => {
// 							plus.runtime.restart();
// 						});
// 				},
// 				(e) => {
// 					consoleError("Launcher", "APP更新检测异常");
// 					useMessageBox
// 						.alert({
// 							title: "资源文件更新失败",
// 							msg: e.message,
// 						})
// 						.then(() => {
// 							openStartPage();
// 						});
// 				}
// 			);
// 		})
// 		.catch((error) => {
// 			useToast.error("资源文件下载失败");
// 			consoleError("Launcher", "热更新文件下载失败");
// 			consoleError("Launcher", error);
// 			openStartPage();
// 		});
// };

// /** 整包更新 */
// const fullUpdate = (url: string) => {
// 	consoleLog("Launcher", `整包更新：${url}`);
// 	useMessageBox
// 		.alert({
// 			title: "更新提示",
// 			msg: "开始整包更新",
// 		})
// 		.then(() => {
// 			plus.runtime.openURL(url);
// 			openStartPage();
// 		});
// };

/** 检查App版本 */
const checkAppVersion = () => {
	state.btnText = "检查更新";
	// plus.runtime.getProperty(plus.runtime.appid, (widgetInfo) => {
	// 	type appCheckUpdateResult = {
	// 		Detail: {
	// 			Status: number;
	// 			Version: string;
	// 			Note: string;
	// 			WgtUrl: string;
	// 			PkgUrl: string;
	// 		};
	// 		IsSuccess: boolean;
	// 		Value: boolean;
	// 	};
	// 	axiosUtil
	// 		.request<appCheckUpdateResult>({
	// 			url: `${configStore.launchData.apiUrl}/client/app/checkupdate`,
	// 			method: "post",
	// 			timeout: 5000,
	// 			data: {
	// 				version: widgetInfo.version,
	// 				appid: GejiaApp.appId,
	// 			},
	// 			requestType: "query",
	// 			simpleDataFormat: false,
	// 			requestCipher: false,
	// 			loading: true,
	// 			loadingText: "获取App更新信息中...",
	// 		})
	// 		.then((res) => {
	// 			consoleLog("Launcher", "更新检测", res);
	// 			switch (res.Detail.Status) {
	// 				case 1:
	// 					{
	// 						const downloadUrl = `${configStore.launchData.apiUrl}/${res.Detail.WgtUrl}`;
	// 						plusUpdate(downloadUrl);
	// 					}
	// 					break;
	// 				case 2:
	// 					{
	// 						const downloadUrl = `${configStore.launchData.apiUrl}/${res.Detail.PkgUrl}`;
	// 						fullUpdate(downloadUrl);
	// 					}
	// 					break;
	// 				default:
	// 					openStartPage();
	// 					break;
	// 			}
	// 		})
	// 		.catch((error) => {
	// 			openStartPage();
	// 			consoleError("Launcher", "更新检查失败");
	// 			consoleError("Launcher", error);
	// 		});
	// });
};

const appCheckUpdate = () => {
	if (state.skipUpdateCheck) {
		openStartPage();
	} else {
		// #ifdef MP-WEIXIN
		checkMiniAppVersion();
		// #endif

		// #ifdef APP-PLUS
		checkAppVersion();
		// #endif

		// #ifdef H5
		openStartPage();
		// #endif
	}
};

const appLaunch = () => {
	state.loading = true;
	state.btnText = "正在加载";

	appStore
		.launch()
		.then(() => {
			state.btnText = "成功进入";
			appCheckUpdate();
		})
		.catch((error) => {
			consoleError("Launcher", "launch接口异常");
			consoleError("Launcher", error);

			// launch 接口异常，则跳过更新
			state.skipUpdateCheck = true;

			// 判断是否存在缓存数据
			if (appStore.hasLaunch) {
				state.btnText = "尝试进入";
				consoleWarn("Launcher", `尝试缓存数据加载应用`);
				// 等待3秒进入应用
				setTimeout(() => {
					state.btnText = "成功进入";
					appCheckUpdate();
				}, 3000);
			}
		})
		.finally(() => {
			// 不管成功与否，3秒后，确保能再次点击调用，并且直接跳过更新
			setTimeout(() => {
				state.loading = false;
				state.skipUpdateCheck = true;
			}, 3000);
		});
};

onLoad(() => {
	appLaunch();
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
