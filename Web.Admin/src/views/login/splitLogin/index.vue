<template>
	<div class="split-login" :style="{ '--login-bg': props.background }">
		<slot name="help" />
		<!-- 左侧装饰面板 -->
		<div class="split-login__panel">
			<div class="panel__decoration">
				<div class="deco-circle deco-circle--1"></div>
				<div class="deco-circle deco-circle--2"></div>
				<div class="deco-circle deco-circle--3"></div>
			</div>
			<div class="panel__content">
				<div class="panel__logo">
					<img :src="logoImage" class="logo-img" />
					<span class="logo-text">{{ appStore.appName }}</span>
				</div>
				<div class="panel__icon">
					<svg viewBox="0 0 200 200" fill="none" xmlns="http://www.w3.org/2000/svg">
						<circle cx="100" cy="100" r="80" fill="rgba(255,255,255,0.08)" />
						<circle cx="100" cy="100" r="60" fill="rgba(255,255,255,0.06)" />
						<!-- 医疗十字 -->
						<rect x="82" y="52" width="36" height="96" rx="8" fill="rgba(255,255,255,0.9)" />
						<rect x="52" y="82" width="96" height="36" rx="8" fill="rgba(255,255,255,0.9)" />
						<!-- 心跳线 -->
						<polyline
							points="30,140 55,140 65,120 78,160 90,110 103,155 115,135 130,140 170,140"
							stroke="rgba(255,255,255,0.6)"
							stroke-width="3"
							stroke-linecap="round"
							stroke-linejoin="round"
							fill="none"
						/>
					</svg>
				</div>
				<div class="panel__tagline">
					<h2>智慧医院管理平台</h2>
					<p>高效、安全、便捷的一体化医院管理解决方案</p>
				</div>
			</div>
		</div>
		<!-- 右侧登录表单 -->
		<div class="split-login__form-wrap">
			<transition mode="out-in" name="fade-up">
				<!-- 账号登录 -->
				<div v-if="formStep === 'Account'" key="Account" class="form-container">
					<div class="form-header">
						<h1 class="form-title">欢迎登录</h1>
						<p class="form-subtitle">{{ appStore.appName }}平台管理系统</p>
					</div>
					<el-form
						ref="elFormRef"
						:model="formData"
						:rules="props.formRules"
						size="large"
						labelPosition="top"
						@keyup.enter="handleKeyupEnter"
					>
						<el-form-item prop="account" label="账号">
							<el-input
								v-model.trim="formData.account"
								placeholder="请输入账号"
								type="text"
								tabindex="1"
								:prefixIcon="User"
								@change="handleAccountChange"
							/>
						</el-form-item>
						<el-form-item prop="password" label="密码">
							<el-input
								v-model.trim="formData.password"
								placeholder="请输入密码"
								type="password"
								tabindex="2"
								:showPassword="!formData.encryptPassword"
								:prefixIcon="Lock"
								@input="handlePasswordInput"
							/>
						</el-form-item>
						<div class="form-options">
							<el-checkbox v-model.checked="formData.rememberMe">记住密码</el-checkbox>
						</div>
						<FaButton ref="faButtonRef" class="login-btn" type="primary" size="large" @click="handleFormLogin">
							<span>登 录</span>
						</FaButton>
					</el-form>
				</div>
				<!-- 租户账号 -->
				<div v-else-if="formStep === 'TenantAccount'" key="TenantAccount" class="form-container">
					<div class="form-header">
						<h1 class="form-title">欢迎回来</h1>
						<p class="form-subtitle">
							{{ currentTenant?.tenantName }} -
							<strong>{{ currentTenant?.employeeName }}</strong>
						</p>
					</div>
					<el-form
						ref="elFormRef"
						:model="formData"
						:rules="props.formRules"
						size="large"
						labelPosition="top"
						@keyup.enter="handleKeyupEnter"
					>
						<el-form-item v-if="tenantList.length > 0" prop="userKey">
							<el-select
								popperClass="tenant__select__popper"
								v-model="formData.userKey"
								placeholder="选择租户"
								@change="handleTenantChange"
							>
								<el-option v-for="(item, idx) in tenantList" :key="idx" :label="item.tenant.tenantName" :value="item.tenant.userKey">
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
								placeholder="请输入密码"
								type="password"
								tabindex="2"
								:showPassword="!formData.encryptPassword"
								:prefixIcon="Lock"
								@input="handlePasswordInput"
							/>
						</el-form-item>
						<div class="form-options">
							<el-checkbox v-model.checked="formData.rememberMe">记住密码</el-checkbox>
						</div>
						<FaButton ref="faButtonRef" class="login-btn" type="primary" size="large" @click="handleFormLogin">
							<span>登 录</span>
						</FaButton>
					</el-form>
				</div>
				<!-- 新账号 -->
				<div v-else-if="formStep === 'NewAccount'" key="NewAccount" class="form-container">
					<div class="form-header">
						<el-button type="primary" size="small" link :icon="ArrowLeftBold" class="back-btn" @click="handleNewAccountBack"
							>返回</el-button
						>
						<h1 class="form-title">绑定账号</h1>
						<p class="form-subtitle">请输入新的租户账号进行登录</p>
					</div>
					<el-form
						ref="elFormRef"
						:model="formData"
						:rules="props.formRules"
						size="large"
						labelPosition="top"
						@keyup.enter="handleKeyupEnter"
					>
						<el-form-item prop="account" label="账号">
							<el-input
								v-model.trim="formData.account"
								placeholder="请输入账号"
								type="text"
								tabindex="1"
								:prefixIcon="User"
								@change="handleAccountChange"
							/>
						</el-form-item>
						<el-form-item prop="password" label="密码">
							<el-input
								v-model.trim="formData.password"
								placeholder="请输入密码"
								type="password"
								tabindex="2"
								:showPassword="!formData.encryptPassword"
								:prefixIcon="Lock"
								@input="handlePasswordInput"
							/>
						</el-form-item>
						<div class="form-options">
							<el-checkbox v-model.checked="formData.rememberMe">记住密码</el-checkbox>
						</div>
						<FaButton ref="faButtonRef" class="login-btn" type="primary" size="large" @click="handleFormLogin">
							<span>登 录</span>
						</FaButton>
					</el-form>
				</div>
				<!-- 租户选择 -->
				<div v-else-if="formStep === 'SelectTenant'" key="SelectTenant" class="form-container">
					<div class="form-header">
						<el-button
							type="primary"
							size="small"
							link
							:icon="ArrowLeftBold"
							class="back-btn"
							@click="
								() => {
									if (tenantList.length > 0) {
										formStep = 'NewAccount';
									} else {
										formStep = 'Account';
									}
								}
							"
							>返回</el-button
						>
						<h1 class="form-title">选择租户</h1>
						<p class="form-subtitle">请选择租户账号进行登录</p>
					</div>
					<ul class="tenant__list">
						<el-scrollbar>
							<li v-for="(item, idx) in tenantSelector" :key="idx">
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
			<div class="form-footer">
				<Footer />
			</div>
		</div>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { FormInstance, type FormRules } from "element-plus";
import { ArrowLeftBold, Close, Lock, User } from "@element-plus/icons-vue";
import { FaButtonInstance } from "fast-element-plus";
import { definePropType } from "@fast-china/utils";
import logoImage from "@/assets/logo.png";
import { useApp } from "@/stores";
import { useLogin } from "../useLogin";

defineOptions({
	name: "SplitLogin",
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

const elFormRef = ref<FormInstance>();
const faButtonRef = ref<FaButtonInstance>();

const {
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
} = useLogin(elFormRef, faButtonRef);
</script>

<style scoped lang="scss">
@use "./index.scss";
</style>
