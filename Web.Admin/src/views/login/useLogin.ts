import { computed, inject, ref } from "vue";
import { ElMessage, ElMessageBox, type FormInstance } from "element-plus";
import { type FaButtonInstance, formUtil } from "fast-element-plus";
import { Local, cryptoUtil } from "@fast-china/utils";
import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { loginApi } from "@/api/services/Auth/login";
import { useUserInfo } from "@/stores";
import type { IFormData, IFormStep, ITenantData } from "./index.vue";
import type { LoginOutput } from "@/api/services/Auth/login/models/LoginOutput";
import type { LoginTenantOutput } from "@/api/services/Auth/login/models/LoginTenantOutput";
import type { Ref } from "vue";

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
	const currentTenant = computed<LoginTenantOutput>(() => tenantList.value?.find((f) => f.tenant.userKey === formData.value.userKey)?.tenant);

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
			formData.value = { account: undefined, password: undefined, userKey: undefined, rememberMe: false, encryptPassword: false };
			Local.remove(cFormKey);
			formStep.value = "Account";
		} else {
			const _tenant = tenantList.value[0];
			formData.value = { ..._tenant.formData, userKey: _tenant.tenant.userKey };
			formData.value.encryptPassword = formData.value.rememberMe;
			Local.set(cFormKey, tenantList.value);
		}
	};

	/** 租户删除 */
	const handleTenantRemove = (index: number, value: ITenantData): void => {
		ElMessageBox.confirm("您确定要移除此登录信息吗？", {
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
		formData.value = { account: undefined, password: undefined, userKey: undefined, rememberMe: false, encryptPassword: false };
	};

	/** 新账号返回 */
	const handleNewAccountBack = (): void => {
		handleRefreshTenant();
		formStep.value = "TenantAccount";
	};

	/** 账号改变 */
	const handleAccountChange = (): void => {
		formData.value.password = undefined;
		formData.value.encryptPassword = false;
	};

	/** 密码输入 */
	const handlePasswordInput = (value: string): void => {
		if (formData.value.encryptPassword) {
			const newValue = value.substring(value.length - 1);
			formData.value.password = newValue;
			formData.value.encryptPassword = false;
		}
	};

	/** 登录 */
	const handleLogin = async (_, done?: () => void): Promise<void> => {
		try {
			const { account, password, userKey, rememberMe, encryptPassword } = formData.value;
			if (!encryptPassword) {
				formData.value.password = cryptoUtil.sha1.encrypt(password);
				formData.value.encryptPassword = true;
			}
			let apiRes: LoginOutput;
			// 判断是否存在租户编号和用户Key，如果存在直接租户登录
			if (userKey) {
				apiRes = await loginApi.tenantLogin({
					userKey,
					password: formData.value.password,
				});
			} else {
				apiRes = await loginApi.login({
					account,
					password: formData.value.password,
				});
			}
			switch (apiRes.status) {
				// 登录成功
				case LoginStatusEnum.Success:
					{
						const tenantInfo = apiRes.tenantList[0];
						const fIdx = tenantList.value.findIndex((f) => f.tenant.userKey === tenantInfo.userKey);
						if (fIdx >= 0) {
							tenantList.value.splice(fIdx, 1);
						}
						tenantList.value.unshift({
							tenant: apiRes.tenantList[0],
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
					ElMessage.success(apiRes.message);
					tenantSelector.value = apiRes.tenantList;
					formStep.value = "SelectTenant";
					break;
			}
		} finally {
			done && done();
		}
	};

	/** 表单登录 */
	const handleFormLogin = (_, done?: () => void): void => {
		formUtil
			.validate(elFormRef)
			.then(() => handleLogin(_, done))
			.finally(() => done && done());
	};

	/** 回车键摁下 */
	const handleKeyupEnter = (): void => {
		faButtonRef.value.doLoading(() => handleFormLogin(null));
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
		handlePasswordInput,
		handleLogin,
		handleFormLogin,
		handleKeyupEnter,
	};
};
