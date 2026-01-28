import { reactive, ref, toRefs } from "vue";
import { useFastAxios } from "@fast-china/axios";
import { Local, consoleError, consoleLog, useIdentity } from "@fast-china/utils";
import { defineStore } from "pinia";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";
import { appApi } from "@/api/services/Center/app";
import { dictionaryApi } from "@/api/services/Center/dictionary";
import { useToast } from "@/hooks";
import type { LaunchOutput } from "@/api/services/Center/app/models/LaunchOutput";

export type ILoginComponent = "ClassicLogin";

type IState = {
	/** 环境 */
	env: ViteEnv;
	/** 设备类型 */
	deviceType: AppEnvironmentEnum;
	/** 当前应用的AppId，获取到的 */
	appId: string;
	/**
	 * - manifest.json 中应用版本名称
	 * - 如果存在热更新则为 应用资源（wgt）的版本名称
	 */
	appVersion: string;
	/** 是否为 IPhone 设备 */
	isIphone: boolean;
	/** 是否为客户端（Pc） */
	isClient: boolean;
	/** 设备基础信息 */
	appBaseInfo: UniNamespace.GetAppBaseInfoResult;
	/** 设备信息 */
	deviceInfo: UniNamespace.GetDeviceInfoResult;
	/** 窗口信息 */
	windowInfo: UniNamespace.GetWindowInfoResult;
	/**
	 * 小程序胶囊按钮信息
	 * @description 非小程序平台下，只有默认高度
	 */
	menuButton: UniNamespace.GetMenuButtonBoundingClientRectRes;
	/** 网络 */
	network: {
		/** 是否在线 */
		onLine: boolean;
		/** 网络类型 */
		networkType: INetworkType;
	};
	/** 是否存在 Launch 数据 */
	hasLaunch: boolean;
};

export const useApp = defineStore(
	"app",
	() => {
		const state = reactive<IState & LaunchOutput>({
			edition: EditionEnum.None,
			appNo: "",
			appName: "Fast.App",
			logoUrl: "",
			themeColor: "#409EFF",
			icpSecurityCode: "",
			publicSecurityCode: "",
			userAgreement: "",
			privacyAgreement: "",
			serviceAgreement: "",
			appType: AppEnvironmentEnum.MobileThree,
			environmentType: EnvironmentTypeEnum.Development,
			loginComponent: "",
			webSocketUrl: "",
			requestTimeout: 6000,
			requestEncipher: true,
			statusBarImageUrl: "",
			contactPhone: "",
			latitude: null,
			longitude: null,
			address: null,
			bannerImages: [],
			tenantName: "",
			env: "production",
			deviceType: AppEnvironmentEnum.MobileThree,
			appId: "",
			appVersion: "",
			isIphone: false,
			isClient: false,
			appBaseInfo: null,
			deviceInfo: null,
			windowInfo: null,
			menuButton: null,
			network: {
				onLine: false,
				networkType: "none",
			},
			hasLaunch: false,
		});

		/** 字典 */
		const dictionary = ref<Map<string, FaTableEnumColumnCtx[]>>(new Map());

		/** 设置App名称 */
		const setAppName = (appName: string): void => {
			state.appName = appName;
		};

		/** 设置字典 */
		const setDictionary = async (): Promise<void> => {
			// 判断是否存在 Launch 数据
			if (!state.hasLaunch) return;
			try {
				dictionary.value.clear();
				// 处理数据字典
				const _dictionary = await dictionaryApi.queryDictionary();
				Object.entries(_dictionary).forEach(([key, value]) => {
					dictionary.value.set(key, value);
				});
			} catch {
				consoleError("App", "字典加载失败");
			}
		};

		/** 设置 FastAxios */
		const setFastAxios = (): void => {
			// 判断是否存在 Launch 数据
			if (!state.hasLaunch) return;
			const fastAxios = useFastAxios();
			fastAxios.setOptions({
				timeout: state.requestTimeout,
				requestCipher: state.requestEncipher,
			});
		};

		/** Launch */
		const launch = async (): Promise<void> => {
			try {
				const apiRes = await appApi.launch();
				consoleLog("App", "Launch", apiRes);
				Object.assign(state, apiRes);
				state.hasLaunch = true;
			} finally {
				if (!state.loginComponent) {
					state.loginComponent = "ClassicLogin";
				}

				setFastAxios();

				// 处理数据字典
				await setDictionary();
			}
		};

		/** 获取字典 */
		const getDictionary = (key: string, isAll = false): FaTableEnumColumnCtx[] => {
			if (!dictionary.value.has(key)) {
				consoleError("app", `字典 [${key}] 不存在`);
				return;
			}
			let result = dictionary.value.get(key);
			if (isAll) {
				result = [
					{
						label: "全部",
						value: null,
					},
					...result.slice(),
				];
			}
			return result;
		};

		/** 拨打电话 */
		const makePhoneCall = (): void => {
			if (!state.contactPhone) {
				useToast.warning("未配置联系电话");
				return;
			}
			uni.makePhoneCall({
				phoneNumber: state.contactPhone,
			});
		};

		/** 打开位置 */
		const openLocation = (): void => {
			if (!state.latitude && !state.longitude) {
				useToast.warning("未配置位置信息");
				return;
			}
			uni.openLocation({
				latitude: state.latitude,
				longitude: state.longitude,
				name: state.appName,
				address: state.address,
				fail: () => {
					useToast.warning("无法打开位置");
				},
			});
		};

		/** 清除 App 缓存 */
		const clearAppCache = (): void => {
			// 获取设备Id，这里按理来说不应该不存在的
			const uIdentity = useIdentity();
			// 清空 Local 缓存
			Local.clear();
			// 重新设置设备Id
			uIdentity.makeIdentity(uIdentity.deviceId);
		};

		return {
			...toRefs(state),
			setAppName,
			setDictionary,
			setFastAxios,
			launch,
			getDictionary,
			makePhoneCall,
			openLocation,
			clearAppCache,
		};
	},
	{
		persist: {
			key: "store-app",
		},
	}
);
