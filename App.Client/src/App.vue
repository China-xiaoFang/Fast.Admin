<script setup lang="ts">
import { onLaunch } from "@dcloudio/uni-app";
import { axiosUtil } from "@fast-china/axios";
import { consoleError, consoleLog } from "@fast-china/utils";
import { CommonRoute } from "@/common";
import { useMessageBox, useToast } from "@/hooks";
import { useApp, useConfig } from "@/stores";

/** 检查小程序版本 */
const checkMiniAppVersion = () => {
	const updateManager = uni.getUpdateManager();
	// 检查小程序是否有新版本
	updateManager.onCheckForUpdate((res) => {
		if (res.hasUpdate) {
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
		}
	});
};

/** 热更新 */
const plusUpdate = (url: string) => {
	consoleLog("Launcher", `热更新：${url}`);
	axiosUtil
		.request<string>({
			url,
			method: "download",
			timeout: 5000,
			requestType: "download",
			simpleDataFormat: false,
			requestCipher: false,
			loading: true,
			loadingText: "下载资源文件中...",
		})
		.then((res) => {
			plus.runtime.install(
				res,
				{},
				() => {
					consoleLog("Launcher", "APP存在新版本");
					useMessageBox
						.alert({
							title: "更新提示",
							msg: "新版本已经准备好，需要重启后才能正常使用应用。",
						})
						.then(() => {
							plus.runtime.restart();
						});
				},
				(e) => {
					consoleError("Launcher", "APP更新检测异常");
					useMessageBox.alert({
						title: "资源文件更新失败",
						msg: e.message,
					});
				}
			);
		})
		.catch((error) => {
			useToast.error("资源文件下载失败");
			consoleError("Launcher", "热更新文件下载失败");
			consoleError("Launcher", error);
		});
};

/** 整包更新 */
const fullUpdate = (url: string) => {
	consoleLog("Launcher", `整包更新：${url}`);
	useMessageBox
		.alert({
			title: "更新提示",
			msg: "开始整包更新",
		})
		.then(() => {
			plus.runtime.openURL(url);
		});
};

/** 检查App版本 */
const checkAppVersion = () => {
	plus.runtime.getProperty(plus.runtime.appid, (widgetInfo) => {
		// type appCheckUpdateResult = {
		// 	Detail: {
		// 		Status: number;
		// 		Version: string;
		// 		Note: string;
		// 		WgtUrl: string;
		// 		PkgUrl: string;
		// 	};
		// 	IsSuccess: boolean;
		// 	Value: boolean;
		// };
		// axiosUtil
		// 	.request<appCheckUpdateResult>({
		// 		url: `${configStore.launchData.apiUrl}/client/app/checkupdate`,
		// 		method: "post",
		// 		timeout: 5000,
		// 		data: {
		// 			version: widgetInfo.version,
		// 			appid: GejiaApp.appId,
		// 		},
		// 		requestType: "query",
		// 		simpleDataFormat: false,
		// 		requestCipher: false,
		// 		loading: true,
		// 		loadingText: "获取App更新信息中...",
		// 	})
		// 	.then((res) => {
		// 		consoleLog("Launcher", "更新检测", res);
		// 		switch (res.Detail.Status) {
		// 			case 1:
		// 				{
		// 					const downloadUrl = `${configStore.launchData.apiUrl}/${res.Detail.WgtUrl}`;
		// 					plusUpdate(downloadUrl);
		// 				}
		// 				break;
		// 			case 2:
		// 				{
		// 					const downloadUrl = `${configStore.launchData.apiUrl}/${res.Detail.PkgUrl}`;
		// 					fullUpdate(downloadUrl);
		// 				}
		// 				break;
		// 			default:
		// 				openStartPage();
		// 				break;
		// 		}
		// 	})
		// 	.catch((error) => {
		// 		openStartPage();
		// 		consoleError("Launcher", "更新检查失败");
		// 		consoleError("Launcher", error);
		// 	});
	});
};

onLaunch((options) => {
	const appStore = useApp();
	const configStore = useConfig();

	consoleLog("App", `成功加载【${appStore.appBaseInfo.appName}】`, new Date());

	// #ifdef APP-PLUS
	// App端需要调用setUIStyle，否则无法使用深色模式
	plus.nativeUI.setUIStyle("auto");
	// #endif

	// 设置应用名称
	appStore.setAppName(appStore.appBaseInfo.appName);

	// 初始化主题
	configStore.initTheme();

	// Launch
	appStore
		.launch()
		.then(() => {
			// #ifdef MP-WEIXIN
			checkMiniAppVersion();
			// #endif

			// #ifdef APP-PLUS
			checkAppVersion();
			// #endif
		})
		.catch((error) => {
			consoleError("Launcher", "launch接口异常");
			consoleError("Launcher", error);
		});
});
</script>
