<template>
	<el-container :style="{ background: props.background }">
		<slot name="help" />
		<el-main>
			<div class="modern-login">
				<!-- 左侧品牌展示区 -->
				<div class="modern-login__brand">
					<div class="brand-bg">
						<div class="brand-particle brand-particle--1" />
						<div class="brand-particle brand-particle--2" />
						<div class="brand-particle brand-particle--3" />
						<div class="brand-particle brand-particle--4" />
						<div class="brand-ring brand-ring--1" />
						<div class="brand-ring brand-ring--2" />
					</div>
					<div class="brand-content">
						<div class="brand-logo">
							<img :src="logoImage" alt="Logo" />
						</div>
						<h1 class="brand-title">{{ appStore.appName }}</h1>
						<p class="brand-subtitle">管理平台</p>
						<div class="brand-divider" />
						<ul class="brand-features">
							<li>
								<el-icon><Monitor /></el-icon>
								<span>现代化企业级管理系统</span>
							</li>
							<li>
								<el-icon><Lock /></el-icon>
								<span>安全可靠的数据保护</span>
							</li>
							<li>
								<el-icon><TrendCharts /></el-icon>
								<span>高效便捷的业务流程</span>
							</li>
							<li>
								<el-icon><Iphone /></el-icon>
								<span>多终端适配体验</span>
							</li>
						</ul>
					</div>
				</div>
				<!-- 右侧登录表单 -->
				<div class="modern-login__form">
					<transition mode="out-in" name="slide-left">
						<!-- 账号 -->
						<div v-if="formStep === 'Account'" class="form-panel">
							<div class="form-header" style="margin-bottom: 28px">
								<h2>欢迎登录</h2>
								<p>
									<span class="bold">{{ appStore.appName }}</span>
									管理平台
								</p>
							</div>
							<el-form
								ref="elFormRef"
								labelPosition="top"
								:model="formData"
								:rules="props.formRules"
								size="large"
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
								<FaButton ref="faButtonRef" class="w100 login-btn" type="primary" size="large" @click="handleFormLogin">
									登 录
								</FaButton>
							</el-form>
						</div>
						<!-- 租户账号 -->
						<div v-else-if="formStep === 'TenantAccount'" class="form-panel">
							<div class="form-header" style="margin-bottom: 18px">
								<h2>欢迎回来</h2>
								<p>
									{{ currentTenant?.tenantName }} -
									<span class="bold">{{ currentTenant?.employeeName }}</span>
								</p>
							</div>
							<el-form
								ref="elFormRef"
								labelPosition="top"
								:model="formData"
								:rules="props.formRules"
								size="large"
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
								<FaButton ref="faButtonRef" class="w100 login-btn" type="primary" size="large" @click="handleFormLogin">
									登 录
								</FaButton>
							</el-form>
						</div>
						<!-- 新账号 -->
						<div v-else-if="formStep === 'NewAccount'" class="form-panel">
							<div class="form-header" style="margin-bottom: 18px">
								<h2>请输入</h2>
								<p>新的租户账号进行登录</p>
							</div>
							<div class="form-back">
								<el-button type="primary" size="default" link :icon="ArrowLeftBold" @click="handleNewAccountBack">返回</el-button>
							</div>
							<el-form
								ref="elFormRef"
								labelPosition="top"
								:model="formData"
								:rules="props.formRules"
								size="large"
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
								<FaButton ref="faButtonRef" class="w100" type="primary" size="large" @click="handleFormLogin">登录</FaButton>
							</el-form>
						</div>
						<!-- 租户选择 -->
						<div v-else-if="formStep === 'SelectTenant'" class="form-panel">
							<div class="form-header" style="margin-bottom: 8px">
								<h2>请选择</h2>
								<p>租户账号进行登录</p>
							</div>
							<div class="form-back">
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
					<div class="form-footer-text">Powered by FastDotNet</div>
				</div>
			</div>
		</el-main>
		<el-footer :style="{ '--el-footer-height': addUnit(props.footerHeight) }">
			<Footer />
		</el-footer>
	</el-container>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { FormInstance, type FormRules } from "element-plus";
import { ArrowLeftBold, Close, Iphone, Lock, Monitor, TrendCharts, User } from "@element-plus/icons-vue";
import { FaButtonInstance } from "fast-element-plus";
import { addUnit, definePropType } from "@fast-china/utils";
import logoImage from "@/assets/logo.png";
import { useApp } from "@/stores";
import { useLogin } from "../useLogin";

defineOptions({
	name: "ModernLogin",
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
