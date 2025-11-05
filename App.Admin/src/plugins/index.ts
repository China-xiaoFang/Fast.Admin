import { type App } from "vue";
import { consoleLog, useIdentity, useStorage } from "@fast-china/utils";
import { dayjs } from "wot-design-uni";
import { loadFastAxios } from "./axios";
import { loadZPaging } from "./z-paging";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { CommonUniApp } from "@/common";
import { RegExps } from "@/constants";
import { useApp } from "@/stores";
import "dayjs/locale/zh-cn";

export function loadPlugins(app: App): void {
	const appStore = useApp();

	const uStorage = useStorage();
	// 缓存前缀
	uStorage.setPrefix("fast__");
	// 缓存是否加密
	uStorage.setCrypto(import.meta.env.VITE_STORAGE_CRYPTO == "true");

	consoleLog("App", "Env", import.meta.env);

	const deviceId = useIdentity().makeIdentity();
	consoleLog("App", "DeviceId", deviceId);

	// 获取网络信息
	uni.getNetworkType({
		success: ({ networkType }) => {
			const _networkType = networkType as INetworkType;
			consoleLog("App", "NetworkType", _networkType);
			appStore.network = {
				onLine: _networkType !== "none",
				networkType: _networkType,
			};
		},
	});

	// 监听网络变化
	uni.onNetworkStatusChange((res) => {
		consoleLog("App", "监听到网络改变", res);
		appStore.network = {
			onLine: res.isConnected,
			networkType: res.networkType as INetworkType,
		};
	});

	// 获取系统信息
	const appBaseInfo = uni.getAppBaseInfo();
	appStore.appBaseInfo = appBaseInfo;
	consoleLog("App", "AppBaseInfo", appStore.appBaseInfo);

	// 获取设备信息
	const deviceInfo = uni.getDeviceInfo();
	appStore.deviceInfo = deviceInfo;
	consoleLog("App", "DeviceInfo", appStore.deviceInfo);

	// 获取窗口信息
	appStore.windowInfo = uni.getWindowInfo();
	consoleLog("App", "WindowInfo", appStore.windowInfo);

	// AppId
	appStore.appId = appBaseInfo.appId;
	// App版本号
	appStore.appVersion = appBaseInfo.appVersion;
	appStore.menuButton = {
		width: 0,
		height: CommonUniApp.navbarCapsuleMenuButtonHeight,
		top: 0,
		right: 0,
		bottom: 0,
		left: 0,
	};

	// #ifdef MP-WEIXIN
	appStore.deviceType = AppEnvironmentEnum.WeChatMiniProgram;

	// 获取小程序菜单胶囊按钮
	appStore.menuButton = uni.getMenuButtonBoundingClientRect();

	// 获取微信小程序信息
	const accountInfo = uni.getAccountInfoSync();
	// 使用微信小程序的 AppId
	appStore.appId = accountInfo.miniProgram.appId;
	// #endif

	// #ifdef APP-PLUS
	if (deviceInfo.platform === "ios") {
		appStore.deviceType = AppEnvironmentEnum.IOS;
	} else {
		appStore.deviceType = AppEnvironmentEnum.Android;
	}

	// 判断是否存在 WGT 热更新的版本号
	if (appBaseInfo.appWgtVersion) {
		appStore.appVersion = appBaseInfo.appWgtVersion;
	}
	// #endif

	consoleLog("App", "AppId", appStore.appId);
	consoleLog("App", "AppVersion", appStore.appVersion);
	consoleLog("App", "MenuButton", appStore.menuButton);

	// 是否为 Iphone 设备
	const isIphone = RegExps.IPhone.test(deviceInfo.model);
	consoleLog("App", "IsIphone", isIphone);

	dayjs.locale("zh-cn");

	loadFastAxios();

	loadZPaging();
}
