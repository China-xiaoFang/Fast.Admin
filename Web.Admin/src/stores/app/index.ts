import { reactive, ref, toRefs } from "vue";
import { ElMessageBox } from "element-plus";
import { useFastAxios } from "@fast-china/axios";
import { consoleError, consoleLog } from "@fast-china/utils";
import { useTitle } from "@vueuse/core";
import { defineStore } from "pinia";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";
import { appApi } from "@/api/services/Center/app";
import { dictionaryApi } from "@/api/services/Center/dictionary";
import { useConfig } from "@/stores/config";
import type { LaunchOutput } from "@/api/services/Center/app/models/LaunchOutput";
import type { FaTableColumnCtx, FaTableEnumColumnCtx } from "fast-element-plus";

export type ILoginComponent = "ClassicLogin";

type IState = {
	/** 是否存在 Launch 数据 */
	hasLaunch: boolean;
};

export const useApp = defineStore(
	"app",
	() => {
		const state = reactive<IState & LaunchOutput>({
			edition: EditionEnum.None,
			appNo: "",
			appName: "Fast.Admin",
			logoUrl: "",
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
			requestEncipher: true,
			statusBarImageUrl: "",
			contactPhone: "",
			latitude: null,
			longitude: null,
			address: null,
			bannerImages: [],
			tenantName: "",
			hasLaunch: false,
		});

		/** 字典 */
		const dictionary = ref<Map<string, FaTableEnumColumnCtx[]>>(new Map());

		/** 表格列 */
		const tableColumns = ref<Map<string, FaTableColumnCtx[]>>(new Map());

		/** Launch */
		const launch = async (): Promise<void> => {
			try {
				const apiRes = await appApi.launch();
				consoleLog("App", "Launch", apiRes);
				Object.assign(state, apiRes);
				state.hasLaunch = true;
			} catch (error) {
				consoleError("App", error);
				// 避免 Launch 接口出现问题，如果存在缓存，也正常进入
				if (!state.hasLaunch) {
					ElMessageBox.alert("系统初始化失败，请稍后刷新浏览器重试。", {
						title: "系统错误",
						type: "error",
						showClose: false,
					});
				}
			} finally {
				if (!state.loginComponent) {
					state.loginComponent = "ClassicLogin";
				}

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

				try {
					// 处理数据字典
					dictionary.value.clear();
					const _dictionary = await dictionaryApi.queryDictionary();
					Object.entries(_dictionary).forEach(([key, value]) => {
						dictionary.value.set(key, value);
					});
				} catch {
					consoleError("App", "字典加载失败");
				}

				const configStore = useConfig();
				configStore.setTheme(state.themeColor);
			}
		};

		/** 获取字典 */
		const getDictionary = (key: string, throwError = true): FaTableEnumColumnCtx[] => {
			if (!dictionary.value.has(key)) {
				if (throwError) {
					consoleError("app", `字典 [${key}] 不存在`);
				}
				return;
			}
			return dictionary.value.get(key);
		};

		/** 获取表格列 */
		const getTableColumns = (tableKey: string, throwError = true): FaTableColumnCtx[] => {
			if (!tableColumns.value.has(tableKey)) {
				if (throwError) {
					consoleError("app", `表格列 [${tableKey}] 不存在`);
				}
				return;
			}
			return tableColumns.value.get(tableKey);
		};

		/** 设置或更新表格列 */
		const setTableColumns = (tableKey: string, columns: FaTableColumnCtx[]): void => {
			if (tableColumns.value.has(tableKey)) {
				tableColumns.value.delete(tableKey);
			}
			tableColumns.value.set(tableKey, columns);
		};

		/** 删除表格列 */
		const deleteTableColumns = (tableKey: string): void => {
			if (tableColumns.value.has(tableKey)) {
				tableColumns.value.delete(tableKey);
			}
		};

		return {
			...toRefs(state),
			launch,
			getDictionary,
			getTableColumns,
			setTableColumns,
			deleteTableColumns,
		};
	},
	{
		persist: {
			key: "store-app",
		},
	}
);
