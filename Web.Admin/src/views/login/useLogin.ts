import { computed, inject, ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { formUtil } from "fast-element-plus";
import { Local } from "@fast-china/utils";
import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { loginApi } from "@/api/services/Auth/login";
import { useUserInfo } from "@/stores";
import type { FormInstance } from "element-plus";
import type { FaButtonInstance } from "fast-element-plus";
import type { Ref } from "vue";
import type { LoginInput } from "@/api/services/Auth/login/models/LoginInput";
import type { LoginOutput } from "@/api/services/Auth/login/models/LoginOutput";
import type { LoginTenantOutput } from "@/api/services/Auth/login/models/LoginTenantOutput";
import type { TenantLoginInput } from "@/api/services/Auth/login/models/TenantLoginInput";

export type IFormData = {
	/** 记住登录信息 */
	rememberMe?: boolean;
} & LoginInput &
	TenantLoginInput;

export type ITenantData = {
	/** 租户 */
	tenant: LoginTenantOutput & { userKey: string };
	/** 表单数据 */
	formData: IFormData;
};

export type IFormStep = "Account" | "TenantAccount" | "SelectTenant" | "NewAccount";

/** 登录服务 */
// eslint-disable-next-line @typescript-eslint/explicit-function-return-type, @typescript-eslint/explicit-module-boundary-types
export const useLogin = (elFormRef: Ref<FormInstance>, faButtonRef: Ref<FaButtonInstance>) => {
	const userInfoStore = useUserInfo();

	/** 表单数据 */
	const formData = inject<Ref<IFormData>>("formData");
	/** 租户集合 */
	const tenantList = inject<Ref<ITenantData[]>>("tenantList");
	/** 表单步骤 */
	const formStep = inject<Ref<IFormStep>>("formStep");
	/** 缓存Key */
	const cFormKey = inject<string>("cFormKey");

	/** 租户选择器 */
	const tenantSelector = ref<LoginTenantOutput[]>([]);
	/** 当前选择租户 */
	const currentTenant = computed<LoginTenantOutput>(() => tenantList.value.find((item) => item.tenant.userKey === formData.value.userKey)?.tenant);

	/** 租户改变 */
	const handleTenantChange = (value: string): void => {
		const fInfo = tenantList.value.find((f) => f.tenant.userKey === value);
		if (!fInfo) {
			ElMessage.error("租户信息不存在");
			return;
		}
		formData.value = { ...fInfo.formData, userKey: fInfo.tenant.userKey };
	};

	/** 租户刷新 */
	const handleRefreshTenant = (): void => {
		if (tenantList.value.length === 0) {
			formData.value = { account: undefined, password: undefined, userKey: undefined, rememberMe: false };
			Local.remove(cFormKey);
			formStep.value = "Account";
		} else {
			const tenant = tenantList.value[0];
			formData.value = { ...tenant.formData, userKey: tenant.tenant.userKey };
			Local.set(cFormKey, tenantList.value);
		}
	};

	/** 租户删除 */
	const handleTenantRemove = (index: number, value: ITenantData): void => {
		void ElMessageBox.confirm("您确定要移除此登录信息吗？", {
			dangerouslyUseHTMLString: true,
		}).then(() => {
			if (value.tenant.userKey === formData.value.userKey) {
				formData.value.userKey = undefined;
			}
			tenantList.value.splice(index, 1);
			handleRefreshTenant();
		});
	};

	/** 新账号 */
	const handleNewAccount = (): void => {
		formStep.value = "NewAccount";
		formData.value = { account: undefined, password: undefined, userKey: undefined, rememberMe: false };
	};

	/** 新账号返回 */
	const handleNewAccountBack = (): void => {
		handleRefreshTenant();
		formStep.value = tenantList.value.length > 0 ? "TenantAccount" : "Account";
	};

	/** 账号改变 */
	const handleAccountChange = (): void => {
		formData.value.password = undefined;
	};

	/** 登录 */
	const handleLogin = async (_event?: MouseEvent | null, done?: () => void): Promise<void> => {
		try {
			const { account, password, userKey, rememberMe } = formData.value;
			if (!password) {
				ElMessage.warning("请输入密码");
				return;
			}
			let apiRes: LoginOutput;
			// 判断是否存在租户编号和用户Key，如果存在直接租户登录
			if (userKey) {
				apiRes = await loginApi.tenantLogin({
					userKey,
					password,
				});
			} else {
				apiRes = await loginApi.login({
					account,
					password,
				});
			}
			switch (apiRes.status) {
				// 登录成功
				case LoginStatusEnum.Success:
					{
						const tenantInfo = apiRes.tenantList?.[0];
						if (!tenantInfo?.userKey) {
							throw new Error("登录成功响应缺少租户信息");
						}
						const fIdx = tenantList.value.findIndex((f) => f.tenant.userKey === tenantInfo.userKey);
						if (fIdx >= 0) {
							tenantList.value.splice(fIdx, 1);
						}
						tenantList.value.unshift({
							tenant: { ...tenantInfo, userKey: tenantInfo.userKey },
							formData: rememberMe
								? formData.value
								: {
										...formData.value,
										rememberMe: false,
										password: undefined,
									},
						});
						Local.set(cFormKey, tenantList.value);
						userInfoStore.login();
					}
					break;
				// 选择租户登录
				case LoginStatusEnum.SelectTenant:
					if (!apiRes.tenantList?.length) {
						throw new Error("登录响应要求选择租户，但未返回租户列表");
					}
					ElMessage.success(apiRes.message || "请选择租户");
					tenantSelector.value = apiRes.tenantList;
					formStep.value = "SelectTenant";
					break;
				default:
					ElMessage.error(apiRes.message || "登录失败，请稍后重试");
			}
		} finally {
			done?.();
		}
	};

	/** 表单登录 */
	const handleFormLogin = (event?: MouseEvent | null, done?: () => void): void => {
		void formUtil
			.validate(elFormRef)
			.then(() => handleLogin(event, done))
			.finally(() => done?.());
	};

	/** 回车键摁下 */
	const handleKeyupEnter = (): void => {
		faButtonRef.value?.doLoading(() => handleFormLogin(null));
	};

	return {
		formData,
		tenantList,
		formStep,
		tenantSelector,
		currentTenant,
		handleTenantChange,
		handleTenantRemove,
		handleNewAccount,
		handleNewAccountBack,
		handleAccountChange,
		handleLogin,
		handleFormLogin,
		handleKeyupEnter,
	};
};
