import { type App } from "vue";
import { consoleLog, useIdentity, useStorage } from "@fast-china/utils";
import { dayjs } from "wot-design-uni";
import { loadFastAxios } from "./axios";
import { loadWotDesign } from "./wot-design-uni";
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

	// #ifdef MP-WEIXIN || H5 || APP-PLUS
	// 获取系统信息
	const appBaseInfo = uni.getAppBaseInfo();
	appStore.appBaseInfo = appBaseInfo;

	// 获取设备信息
	const deviceInfo = uni.getDeviceInfo();
	appStore.deviceInfo = deviceInfo;

	// 获取窗口信息
	appStore.windowInfo = uni.getWindowInfo();
	// #endif

	// #ifndef MP-WEIXIN || H5 || APP-PLUS
	const systemInfo = uni.getSystemInfoSync();
	appStore.appBaseInfo = {
		appId: systemInfo.appId,
		appName: systemInfo.appName,
		appVersion: systemInfo.appVersion,
		appVersionCode: systemInfo.appVersionCode,
		appWgtVersion: systemInfo.appWgtVersion,
		language: systemInfo.language,
		version: systemInfo.version,
		hostName: systemInfo.hostName,
		hostVersion: systemInfo.hostVersion,
		hostLanguage: systemInfo.hostLanguage,
		hostTheme: systemInfo.hostTheme,
		hostPackageName: systemInfo.hostPackageName,
		theme: systemInfo.theme,
		SDKVersion: systemInfo.SDKVersion,
		enableDebug: systemInfo.enableDebug,
		host: systemInfo.host,
		appLanguage: systemInfo.appLanguage,
		hostFontSizeSetting: systemInfo.hostFontSizeSetting,
		hostSDKVersion: systemInfo.hostSDKVersion,
	};
	appStore.deviceInfo = {
		deviceBrand: systemInfo.deviceBrand,
		deviceModel: systemInfo.deviceModel,
		deviceId: systemInfo.deviceId,
		deviceType: systemInfo.deviceType,
		devicePixelRatio: systemInfo.devicePixelRatio,
		deviceOrientation: systemInfo.deviceOrientation,
		brand: systemInfo.brand,
		model: systemInfo.model,
		system: systemInfo.system,
		platform: systemInfo.platform,
	};
	appStore.windowInfo = {
		pixelRatio: systemInfo.pixelRatio,
		screenWidth: systemInfo.screenWidth,
		screenHeight: systemInfo.screenHeight,
		windowWidth: systemInfo.windowWidth,
		windowHeight: systemInfo.windowHeight,
		statusBarHeight: systemInfo.statusBarHeight,
		windowTop: systemInfo.windowTop,
		windowBottom: systemInfo.windowBottom,
		safeArea: systemInfo.safeArea,
		safeAreaInsets: systemInfo.safeAreaInsets,
		screenTop: 0,
	};
	// #endif

	// AppId
	appStore.appId = appStore.appBaseInfo.appId;
	// App版本号
	appStore.appVersion = appStore.appBaseInfo.appVersion;
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
	if (appStore.deviceInfo.platform === "ios") {
		appStore.deviceType = AppEnvironmentEnum.IOS;
	} else {
		appStore.deviceType = AppEnvironmentEnum.Android;
	}

	// 判断是否存在 WGT 热更新的版本号
	if (appStore.appBaseInfo.appWgtVersion) {
		appStore.appVersion = appStore.appBaseInfo.appWgtVersion;
	}
	// #endif

	// 是否为 Iphone 设备
	const isIphone = RegExps.IPhone.test(appStore.deviceInfo.model);
	appStore.isIphone = isIphone;

	const isClient =
		appStore.deviceInfo.platform === "windows" || appStore.deviceInfo.platform === "mac" || appStore.deviceInfo.platform === "devtools";
	appStore.isClient = isClient;

	consoleLog("App", "AppBaseInfo", appStore.appBaseInfo);
	consoleLog("App", "DeviceInfo", appStore.deviceInfo);
	consoleLog("App", "WindowInfo", appStore.windowInfo);
	consoleLog("App", "AppId", appStore.appId);
	consoleLog("App", "AppVersion", appStore.appVersion);
	consoleLog("App", "MenuButton", appStore.menuButton);
	consoleLog("App", "IsIphone", isIphone);
	consoleLog("App", "IsClient", isClient);

	dayjs.locale("zh-cn");

	loadFastAxios();

	loadWotDesign();

	loadZPaging();
}
