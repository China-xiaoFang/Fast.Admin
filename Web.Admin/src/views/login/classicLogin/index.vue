<template>
	<el-container :style="{ background: props.background }">
		<slot name="help" />
		<el-main>
			<div class="login-card">
				<div class="login-card__left">
					<div class="left__title">
						<img :src="logoImage" />
						{{ appStore.appName }}
					</div>
					<div class="left__ikon">
						<img :src="loginIkonImage" />
					</div>
				</div>
				<transition mode="out-in" name="slide-left">
					<!-- 账号 -->
					<div v-if="formStep === 'Account'" class="login-card__right">
						<div style="height: 80px; line-height: 80px">
							<span class="right__title">欢迎登录</span>
							<span>{{ appStore.appName }}平台管理系统</span>
						</div>
						<el-form
							ref="elFormRef"
							labelPosition="top"
							:model="formData"
							:rules="props.formRules"
							size="default"
							labelSuffix=""
							@keyup.enter="handleKeyupEnter"
						>
							<el-form-item prop="account" label="账号">
								<el-input
									v-model.trim="formData.account"
									placeholder="账号"
									type="text"
									tabindex="1"
									:prefixIcon="User"
									@change="handleAccountChange"
								/>
							</el-form-item>
							<el-form-item prop="password" label="密码">
								<el-input
									v-model.trim="formData.password"
									placeholder="密码"
									type="password"
									tabindex="2"
									:showPassword="!formData.encryptPassword"
									:prefixIcon="Lock"
									@input="handlePasswordInput"
								/>
							</el-form-item>
							<el-form-item prop="rememberMe">
								<el-checkbox v-model.checked="formData.rememberMe" size="default">记住密码</el-checkbox>
							</el-form-item>
							<FaButton ref="faButtonRef" class="w100" type="primary" size="default" @click="handleFormLogin">登录</FaButton>
						</el-form>
					</div>
					<!-- 租户账号 -->
					<div v-else-if="formStep === 'TenantAccount'" class="login-card__right">
						<div style="height: 50px; line-height: 50px">
							<span class="right__title">欢迎回来</span>
							<span>
								{{ state.currentTenant?.tenantName }} -
								<span class="bold">{{ state.currentTenant?.employeeName }}</span>
							</span>
						</div>
						<el-form
							ref="elFormRef"
							labelPosition="top"
							:model="formData"
							:rules="props.formRules"
							size="default"
							labelSuffix=""
							@keyup.enter="handleKeyupEnter"
						>
							<el-form-item v-if="tenantList.length > 0" prop="userKey">
								<el-select
									popperClass="tenant__select__popper"
									v-model="formData.userKey"
									placeholder="租户"
									@change="handleTenantChange"
								>
									<el-option
										v-for="(item, idx) in tenantList"
										:key="idx"
										:label="item.tenant.tenantName"
										:value="item.tenant.userKey"
									>
										<div class="tenant__warp">
											<div class="tenant__top">
												<div class="top__name">
													<img :src="item.tenant.logoUrl" />
													<span>{{ item.tenant.tenantName }}</span>
												</div>
												<Tag size="small" name="EditionEnum" :value="item.tenant.edition" />
											</div>
											<div class="tenant__center">
												<span>{{ item.tenant.departmentName || "无部门..." }}</span>
												<span>{{ item.tenant.employeeNo || "无工号..." }}</span>
											</div>
											<div class="tenant__bottom">
												<Tag size="small" name="UserTypeEnum" :value="item.tenant.userType" />
												<div class="bottom__name">
													<span>{{ item.tenant.employeeName }}</span>
													<img :src="item.tenant.idPhoto" />
												</div>
											</div>
										</div>
										<el-icon @click="handleTenantRemove(idx, item)"><Close /></el-icon>
									</el-option>
									<template #footer>
										<el-button text type="info" @click="handleNewAccount">绑定新租户账号</el-button>
									</template>
								</el-select>
							</el-form-item>
							<el-form-item prop="account">
								<template #label>
									<FaFormItemTip label="账号" tips="授权租户不能修改账号，如需登录新的租户账号，请重新绑定" />
								</template>
								<el-input
									disabled
									v-model.trim="formData.account"
									placeholder="账号"
									type="text"
									tabindex="1"
									:prefixIcon="User"
									@change="handleAccountChange"
								/>
							</el-form-item>
							<el-form-item prop="password" label="密码">
								<el-input
									v-model.trim="formData.password"
									placeholder="密码"
									type="password"
									tabindex="2"
									:showPassword="!formData.encryptPassword"
									:prefixIcon="Lock"
									@input="handlePasswordInput"
								/>
							</el-form-item>
							<el-form-item prop="rememberMe">
								<el-checkbox v-model.checked="formData.rememberMe" size="default">记住密码</el-checkbox>
							</el-form-item>
							<FaButton ref="faButtonRef" class="w100" type="primary" size="default" @click="handleFormLogin">登录</FaButton>
						</el-form>
					</div>
					<!-- 新账号 -->
					<div v-else-if="formStep === 'NewAccount'" class="login-card__right">
						<div style="height: 60px; line-height: 60px">
							<span class="right__title">请输入</span>
							<span>新的租户账号进行登录</span>
						</div>
						<div class="right__button-back">
							<el-button type="primary" size="default" link :icon="ArrowLeftBold" @click="handleNewAccountBack">返回</el-button>
						</div>
						<el-form
							ref="elFormRef"
							labelPosition="top"
							:model="formData"
							:rules="props.formRules"
							size="default"
							labelSuffix=""
							@keyup.enter="handleKeyupEnter"
						>
							<el-form-item prop="account" label="账号">
								<el-input
									v-model.trim="formData.account"
									placeholder="账号"
									type="text"
									tabindex="1"
									:prefixIcon="User"
									@change="handleAccountChange"
								/>
							</el-form-item>
							<el-form-item prop="password" label="密码">
								<el-input
									v-model.trim="formData.password"
									placeholder="密码"
									type="password"
									tabindex="2"
									:showPassword="!formData.encryptPassword"
									:prefixIcon="Lock"
									@input="handlePasswordInput"
								/>
							</el-form-item>
							<el-form-item prop="rememberMe">
								<el-checkbox v-model.checked="formData.rememberMe" size="default">记住密码</el-checkbox>
							</el-form-item>
							<FaButton ref="faButtonRef" class="w100" type="primary" size="default" @click="handleFormLogin">登录</FaButton>
						</el-form>
					</div>
					<!-- 租户选择 -->
					<div v-else-if="formStep === 'SelectTenant'" class="login-card__right">
						<div style="height: 50px; line-height: 50px">
							<span class="right__title">请选择</span>
							<span>租户账号进行登录</span>
						</div>
						<div class="right__button-back">
							<el-button
								type="primary"
								size="default"
								link
								:icon="ArrowLeftBold"
								@click="
									() => {
										if (tenantList.length > 0) {
											formStep = 'NewAccount';
										} else {
											formStep = 'Account';
										}
									}
								"
							>
								返回
							</el-button>
						</div>
						<ul class="tenant__list">
							<el-scrollbar>
								<li v-for="(item, idx) in state.tenantSelector" :key="idx">
									<div class="tenant__warp">
										<div class="tenant__top">
											<div class="top__name">
												<img :src="item.logoUrl" />
												<span>{{ item.tenantName }}</span>
											</div>
											<Tag size="small" name="EditionEnum" :value="item.edition" />
										</div>
										<div class="tenant__center">
											<span>{{ item.departmentName || "无部门..." }}</span>
											<span>{{ item.employeeNo || "无工号..." }}</span>
										</div>
										<div class="tenant__bottom">
											<Tag size="small" name="UserTypeEnum" :value="item.userType" />
											<div class="bottom__name">
												<span>{{ item.employeeName }}</span>
												<img :src="item.idPhoto" />
											</div>
										</div>
									</div>
									<FaButton
										type="primary"
										size="default"
										@click="
											(event, done) => {
												formData.userKey = item.userKey;
												handleLogin(event, done);
											}
										"
									>
										登录
									</FaButton>
								</li>
							</el-scrollbar>
						</ul>
					</div>
				</transition>
			</div>
		</el-main>
		<el-footer :style="{ '--el-footer-height': addUnit(props.footerHeight) }">
			<Footer />
		</el-footer>
	</el-container>
</template>

<script lang="ts" setup>
import { computed, inject, reactive, ref } from "vue";
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from "element-plus";
import { ArrowLeftBold, Close, Lock, User } from "@element-plus/icons-vue";
import { type FaButtonInstance, formUtil } from "fast-element-plus";
import { Local, addUnit, cryptoUtil, definePropType, withDefineType } from "@fast-china/utils";
import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { loginApi } from "@/api/services/Auth/login";
import loginIkonImage from "@/assets/images/login_ikon.png";
import logoImage from "@/assets/logo.png";
import { useApp, useUserInfo } from "@/stores";
import type { IFormData, IFormStep, ITenantData } from "../index.vue";
import type { LoginOutput } from "@/api/services/Auth/login/models/LoginOutput";
import type { LoginTenantOutput } from "@/api/services/Auth/login/models/LoginTenantOutput";
import type { Ref } from "vue";

defineOptions({
	name: "ClassicLogin",
});

const props = defineProps({
	/** 背景 */
	background: String,
	/** 页脚高度 */
	footerHeight: Number,
	/** 表单规则 */
	formRules: definePropType<FormRules>(Object),
});

const appStore = useApp();
const userInfoStore = useUserInfo();

const elFormRef = ref<FormInstance>();
const faButtonRef = ref<FaButtonInstance>();

/** 表单数据 */
const formData = inject<Ref<IFormData>>("formData");
/** 租户集合 */
const tenantList = inject<Ref<ITenantData[]>>("tenantList");
/** 表单步骤 */
const formStep = inject<Ref<IFormStep>>("formStep");
/** 缓存Key */
const cFormKey = inject<string>("cFormKey");

const state = reactive({
	/** 租户选择器 */
	tenantSelector: withDefineType<LoginTenantOutput[]>([]),
	/** 当前选择租户 */
	currentTenant: computed<LoginTenantOutput>(() => tenantList.value?.find((f) => f.tenant.userKey === formData.value.userKey)?.tenant),
});

/** 租户改变 */
const handleTenantChange = (value: string) => {
	const fInfo = tenantList.value.find((f) => f.tenant.userKey === value);
	if (!fInfo) {
		ElMessage.error("租户信息不存在");
		return;
	}
	formData.value = { ...fInfo.formData, userKey: fInfo.tenant.userKey };
};

/** 租户刷新 */
const handleRefreshTenant = () => {
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
const handleTenantRemove = (index: number, value: ITenantData) => {
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
const handleNewAccount = () => {
	formStep.value = "NewAccount";
	formData.value = { account: undefined, password: undefined, userKey: undefined, rememberMe: false, encryptPassword: false };
};

/** 新账号返回 */
const handleNewAccountBack = () => {
	handleRefreshTenant();
	formStep.value = "TenantAccount";
};

/** 账号改变 */
const handleAccountChange = () => {
	formData.value.password = undefined;
	formData.value.encryptPassword = false;
};

/** 密码输入 */
const handlePasswordInput = (value: string) => {
	if (formData.value.encryptPassword) {
		const newValue = value.substring(value.length - 1);
		formData.value.password = newValue;
		formData.value.encryptPassword = false;
	}
};

/** 登录 */
const handleLogin = async (_, done?: () => void) => {
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
				state.tenantSelector = apiRes.tenantList;
				formStep.value = "SelectTenant";
				break;
		}
	} finally {
		done && done();
	}
};

/** 表单登录 */
const handleFormLogin = (_, done?: () => void) => {
	formUtil
		.validate(elFormRef)
		.then(() => handleLogin(_, done))
		.finally(() => done && done());
};

/** 回车键摁下 */
const handleKeyupEnter = () => {
	faButtonRef.value.doLoading(() => handleFormLogin(null));
};
</script>

<style scoped lang="scss">
@use "./index.scss";
</style>
