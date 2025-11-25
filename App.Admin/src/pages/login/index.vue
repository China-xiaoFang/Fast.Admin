<template>
	<view class="page">
		<image v-if="appStore.logoUrl" class="img_logo" :src="appStore.logoUrl" @error="state.logoUrl = defaultLogo" />
		<image v-else class="img_logo" :src="defaultLogo" />
		<view class="app-info">{{ appStore.appName }}是一款帮助企业实现智能化管理的移动互联网应用，需要登陆后方可进入系统进行管理</view>
		<wd-form ref="formRef" :model="state.formData" :rules="state.formRule" errorType="toast">
			<view v-if="state.hasUserInfo" class="tenant-info">
				<wd-icon name="user" />
				<view class="user-info">
					<text class="tenant-name">{{ userInfoStore.tenantName }}</text>
					<text class="nick-name">{{ userInfoStore.employeeName }}</text>
				</view>
			</view>
			<wd-input
				v-else
				prefixIcon="user"
				prop="account"
				v-model="state.formData.account"
				clearable
				placeholder="请输入账号"
				:maxlength="20"
				showWordLimit
			/>
			<wd-input
				prefixIcon="lock-off"
				prop="password"
				showPassword
				v-model="state.formData.password"
				clearable
				placeholder="请输入密码"
				@confirm="handleLogin"
			/>
			<wd-button type="info" plain block :round="false" @click="handleLogin">账号登录</wd-button>
			<!-- #ifdef MP-WEIXIN -->
			<wd-button openType="getUserInfo" type="primary" block :round="false" icon="mobile" @getuserinfo="handleWeChatLogin">快捷登陆</wd-button>
			<!-- #endif -->
			<view class="agreement">
				<wd-checkbox v-model="state.formData.agreementSelect" shape="square">我已阅读并同意</wd-checkbox>
				<text @click="router.push(CommonRoute.UserAgreement)">《用户协议》</text>
				<text @click="router.push(CommonRoute.PrivacyAgreement)">《隐私协议》</text>
				<text @click="router.push(CommonRoute.ServiceAgreement)">《服务协议》</text>
			</view>
			<!-- <view class="find-password" @click="router.push('/pages/setting/account/changePasswordByVerifyCode/index')">找回密码</view> -->
		</wd-form>
		<FaFooter />
	</view>
	<wd-message-box selector="confirm-agreement-box">
		<view class="agreement__warp">
			我已阅读并同意
			<text @click="router.push(CommonRoute.UserAgreement)">《用户协议》</text>
			<text @click="router.push(CommonRoute.PrivacyAgreement)">《隐私协议》</text>
			<text @click="router.push(CommonRoute.ServiceAgreement)">《服务协议》</text>
		</view>
	</wd-message-box>
	<!-- #ifdef MP-WEIXIN -->
	<FaPopup ref="authLoginPopupRef" width="80%" :closeOnClickModal="false">
		<view class="pop__auth-warp">
			<view class="auth-body">
				<view class="auth-title">
					<image v-if="appStore.logoUrl" class="auth-logo" :src="appStore.logoUrl" @error="state.logoUrl = defaultLogo" />
					<image v-else class="auth-logo" :src="defaultLogo" />
					<view>您尚未登录</view>
				</view>
				<view class="auth-content">
					<text>为了完整体验，需要您的授权登录</text>
					<text>您可使用手机号一键登录</text>
					<view class="agreement">
						登录即表示您已阅读并同意
						<text @click="router.push(CommonRoute.UserAgreement)">《用户协议》</text>
						<text @click="router.push(CommonRoute.PrivacyAgreement)">《隐私协议》</text>
						<text @click="router.push(CommonRoute.ServiceAgreement)">《服务协议》</text>
					</view>
				</view>
			</view>
			<view class="auth-actions">
				<wd-button type="info" plain block @click="authLoginPopupRef.close()">暂不登录</wd-button>
				<wd-button openType="getPhoneNumber" type="primary" block @getphonenumber="handlePhoneLogin">手机号一键登录</wd-button>
			</view>
		</view>
	</FaPopup>
	<!-- #endif -->
</template>

<script setup lang="ts">
import { reactive, ref } from "vue";
import { clickUtil, consoleLog, cryptoUtil, withDefineType } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { useMessage } from "wot-design-uni";
import type { LoginOutput } from "@/api/services/login/models/LoginOutput";
import type { FaPopupInstance } from "@/components";
import type { FormInstance, FormRules } from "wot-design-uni/components/wd-form/types";
import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { loginApi } from "@/api/services/login";
import { CommonRoute } from "@/common";
import { useMessageBox, useToast } from "@/hooks";
import defaultLogo from "@/static/logo.png";
import { useApp, useUserInfo } from "@/stores";

definePage({
	name: "Login",
	layout: "layout",
	backgroundColor: "var(--wot-bg-color)",
	footer: false,
	watermark: false,
	pageScroll: false,
	authForbidView: true,
	noLogin: true,
	style: {
		navigationBarTitleText: "登录",
	},
});

const appStore = useApp();
const userInfoStore = useUserInfo();
const router = useRouter();

const confirmAgreementBox = useMessage("confirm-agreement-box");

const formRef = ref<FormInstance>();
const authLoginPopupRef = ref<FaPopupInstance>();

const state = reactive({
	/** Logo 图片 */
	logoUrl: appStore.logoUrl,
	/** 存在用户信息 */
	hasUserInfo: false,
	formRule: withDefineType<FormRules>({
		account: [{ required: true, message: "请输入账号" }],
		password: [{ required: true, message: "请输入密码" }],
	}),
	formData: {
		account: "",
		password: "",
		agreementSelect: false,
	},
});

/** 协议检查 */
const agreementCheck = async () => {
	// 判断是否同意了协议
	if (state.formData.agreementSelect) {
		return true;
	} else {
		try {
			await confirmAgreementBox.confirm({
				confirmButtonText: "同意",
				cancelButtonText: "取消",
				closeOnClickModal: false,
			});
			// 弹窗同意
			state.formData.agreementSelect = true;
			return true;
		} catch {
			useToast.warning(`登录前需确认您已阅读并同意《用户协议》、《隐私协议》、《服务协议》，以便为您提供更优质的服务。`);
			return false;
		}
	}
};

/** 登录成功 */
const loginSuccess = (loginRes: LoginOutput) => {
	consoleLog("Login", "LoginRes", loginRes);
	userInfoStore.fakeLogin(loginRes);
	switch (loginRes.status) {
		case LoginStatusEnum.Success:
			userInfoStore.login(loginRes);
			break;
		case LoginStatusEnum.SelectTenant:
			router.push(CommonRoute.SelectTenant);
			break;
		case LoginStatusEnum.AuthExpired:
			useMessageBox.alert(loginRes.message);
			break;
		case LoginStatusEnum.NotAccount:
			// #ifdef MP-WEIXIN
			authLoginPopupRef.value.open();
			// #endif
			// #ifndef MP-WEIXIN
			useMessageBox.alert(loginRes.message);
			// #endif
			break;
	}
};

/** 登录 */
const handleLogin = async () => {
	await clickUtil.throttleAsync(async () => {
		const { valid } = await formRef.value.validate();
		if (valid) {
			if (await agreementCheck()) {
				const { account, password } = state.formData;
				let loginRes: LoginOutput = null;
				if (state.hasUserInfo) {
					const { userKey } = userInfoStore;
					// 租户登录
					loginRes = await loginApi.tenantLogin({
						userKey,
						password: cryptoUtil.sha1.encrypt(password),
					});
				} else {
					// 账号密码登录
					loginRes = await loginApi.login({
						account,
						password: cryptoUtil.sha1.encrypt(password),
					});
				}
				loginSuccess(loginRes);
			}
		}
	});
};

/** 微信登录 */
const handleWeChatLogin = async (detail: UniNamespace.GetUserInfoRes) => {
	await clickUtil.throttleAsync(async () => {
		if (await agreementCheck()) {
			const { iv, encryptedData, userInfo } = detail;
			if (userInfo) {
				consoleLog("Login", "GetUserInfo", userInfo);
				const weChatCode = await userInfoStore.getWeChatCode();
				if (weChatCode) {
					const loginRes = await loginApi.weChatLogin({
						weChatCode,
						iv,
						encryptedData,
					});
					loginSuccess(loginRes);
				} else {
					useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
				}
			} else {
				useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
			}
		}
	});
};

/** 手机登录 */
const handlePhoneLogin = async (detail: UniHelper.ButtonOnGetphonenumberDetail) => {
	await clickUtil.throttleAsync(async () => {
		authLoginPopupRef.value.close(async () => {
			consoleLog("Login", "GetPhoneNumber", detail);
			const { code } = detail;
			if (code) {
				// 手动同意
				state.formData.agreementSelect = true;
				const weChatCode = await userInfoStore.getWeChatCode();
				if (weChatCode) {
					const loginRes = await loginApi.weChatAuthLogin({
						weChatCode,
						code,
					});
					loginSuccess(loginRes);
				} else {
					useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
				}
			} else {
				useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
			}
		});
	});
};
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
