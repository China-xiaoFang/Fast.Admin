import { reactive, toRefs } from "vue";
import { ElMessageBox } from "element-plus";
import { useFastAxios } from "@fast-china/axios";
import { consoleError } from "@fast-china/utils";
import { useTitle } from "@vueuse/core";
import { defineStore } from "pinia";
import { useConfig } from "../config";
import type { LaunchOutput } from "@/api/services/app/models/LaunchOutput";
import type { FaTableEnumColumnCtx } from "fast-element-plus";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";
import { appApi } from "@/api/services/app";
import { dictionaryApi } from "@/api/services/dictionary";

export type ILoginComponent = "ClassicLogin";

type IState = {
	/** 是否存在 Launch 数据 */
	hasLaunch: boolean;
	/** 字典 */
	dictionary: Map<string, FaTableEnumColumnCtx[]>;
};

export const useApp = defineStore(
	"app",
	() => {
		const state = reactive<IState & LaunchOutput>({
			edition: EditionEnum.None,
			appNo: "",
			appName: "Fast.Admin",
			themeColor: "#409EFF",
			icpSecurityCode: "",
			publicSecurityCode: "",
			userAgreement: "",
			privacyAgreement: "",
			serviceAgreement: "",
			appType: AppEnvironmentEnum.Web,
			environmentType: EnvironmentTypeEnum.Development,
			loginComponent: "ClassicLogin",
			webSocketUrl: "",
			requestTimeout: 6000,
			requestEncipher: false,
			hasLaunch: false,
			dictionary: new Map<string, FaTableEnumColumnCtx[]>(),
		});

		/** Launch */
		const launch = async (): Promise<void> => {
			try {
				const apiRes = await appApi.launch();
				Object.assign(state, apiRes);
				if (!state.loginComponent) {
					state.loginComponent = "ClassicLogin";
				}
				state.hasLaunch = true;
				const configStore = useConfig();
				configStore.setTheme(state.themeColor);

				// 处理数据字典
				const dictionary = await dictionaryApi.queryDictionary();
				Object.entries(dictionary).forEach(([key, value]) => {
					state.dictionary.set(key, value);
				});
			} catch (error) {
				consoleError("App", error);
				// 避免 Launch 接口出现问题，如果存在缓存，也正常进入
				if (!state.hasLaunch) {
					ElMessageBox.alert("系统初始化失败，请稍后刷新浏览器重试。", {
						title: "系统错误",
						type: "error",
						showClose: false,
						confirmButtonText: "重试",
					});
				}
			} finally {
				// 判断是否存在 Launch 数据
				if (state.hasLaunch) {
					/** 刷新页面标题 */
					const title = useTitle();
					title.value = state.appName;

					const fastAxios = useFastAxios();
					fastAxios.setOptions({
						timeout: state.requestTimeout,
						requestCipher: state.requestEncipher,
					});
				}
			}
		};

		/** 获取字典 */
		const getDictionary = (key: string, throwError = true): FaTableEnumColumnCtx[] => {
			if (!state.dictionary.has(key)) {
				if (throwError) {
					consoleError("app", `字典 [${key}] 不存在`);
				}
				return;
			}
			return state.dictionary.get(key);
		};

		return {
			...toRefs(state),
			launch,
			getDictionary,
		};
	},
	{
		persist: {
			key: "store-app",
			// 这里是配置 pinia 不需要持久化 dictionary
			omit: ["dictionary"],
		},
	}
);
