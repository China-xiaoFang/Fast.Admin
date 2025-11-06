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
			<wd-input v-else prefixIcon="user" prop="account" v-model="state.formData.account" clearable placeholder="请输入账号" />
			<wd-input
				prefixIcon="lock-off"
				prop="password"
				showPassword
				v-model="state.formData.password"
				clearable
				placeholder="请输入密码"
				@confirm="handleLogin"
			/>
			<wd-button type="info" plain block :round="false" @tap="handleLogin">账号登录</wd-button>
			<!-- #ifdef MP-WEIXIN -->
			<wd-button openType="getUserInfo" type="primary" block :round="false" icon="mobile" @getuserinfo="handleWeChatLogin">快捷登陆</wd-button>
			<!-- #endif -->
			<view class="agreement">
				<wd-checkbox v-model="state.formData.agreementSelect" shape="square">我已阅读并同意</wd-checkbox>
				<text @tap.stop="router.push({ path: CommonRoute.UserAgreement })">《用户协议》</text>
				<text @tap.stop="router.push({ path: CommonRoute.PrivacyAgreement })">《隐私协议》</text>
				<text @tap.stop="router.push({ path: CommonRoute.ServiceAgreement })">《服务协议》</text>
			</view>
			<!-- <view class="find-password" @tap="router.push('/pages/setting/account/changePasswordByVerifyCode/index')">找回密码</view> -->
		</wd-form>
		<FaFooter />
	</view>
	<wd-message-box selector="confirm-agreement-box">
		<view class="agreement__warp">
			我已阅读并同意
			<text @tap.stop="router.push({ path: CommonRoute.UserAgreement })">《用户协议》</text>
			<text @tap.stop="router.push({ path: CommonRoute.PrivacyAgreement })">《隐私协议》</text>
			<text @tap.stop="router.push({ path: CommonRoute.ServiceAgreement })">《服务协议》</text>
		</view>
	</wd-message-box>
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
						<text @tap.stop="router.push({ path: CommonRoute.UserAgreement })">《用户协议》</text>
						<text @tap.stop="router.push({ path: CommonRoute.PrivacyAgreement })">《隐私协议》</text>
						<text @tap.stop="router.push({ path: CommonRoute.ServiceAgreement })">《服务协议》</text>
					</view>
				</view>
			</view>
			<view class="auth-actions">
				<wd-button type="info" plain block @click="authLoginPopupRef.close()">暂不登录</wd-button>
				<wd-button openType="getPhoneNumber" type="primary" block @getphonenumber="handlePhoneLogin">手机号一键登录</wd-button>
			</view>
		</view>
	</FaPopup>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue";
import { clickUtil, consoleLog, withDefineType } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { useMessage } from "wot-design-uni";
import type { FaPopupInstance } from "@/components/popup";
import type { FormInstance, FormRules } from "wot-design-uni/components/wd-form/types";
import { CommonRoute } from "@/common";
import { useToast } from "@/hooks";
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

/** 登录 */
const handleLogin = async () => {
	await clickUtil.throttleAsync(async () => {
		authLoginPopupRef.value.open();
		const { valid } = await formRef.value.validate();
		if (valid) {
			if (await agreementCheck()) {
				// let loginRes: LoginResultDataModel = null;
				// if (state.hasUserInfo) {
				// 	// 商户Key登录
				// 	loginRes = await loginApi.loginByShopClerk({
				// 		shopKey: userInfoStore.userInfo.unionKey,
				// 		clerkKey: userInfoStore.userInfo.clerkKey,
				// 		password: state.formData.password,
				// 	});
				// } else {
				// 	// 账号密码登录
				// 	loginRes = await loginApi.loginByAccount({
				// 		account: state.formData.account,
				// 		password: state.formData.password,
				// 	});
				// }
				// loginSuccess(loginRes);
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
				try {
					const weChatCode = await userInfoStore.getWeChatCode();
					if (weChatCode) {
						// const loginRes = await loginApi.loginByWechat({
						// 	code: weChatCode,
						// 	iv,
						// 	encryptedData,
						// });
						// loginSuccess(loginRes);
					} else {
						useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
					}
				} finally {
					userInfoStore.delWeChatCode();
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
		consoleLog("Login", "GetPhoneNumber", detail);
		const { iv, encryptedData } = detail;
		if (iv && encryptedData) {
			// 手动同意
			state.formData.agreementSelect = true;
			const weChatCode = await userInfoStore.getWeChatCode();
			if (weChatCode) {
				// const loginRes = await loginApi.loginByWechatAuth({
				// 	code: weChatCode,
				// 	iv,
				// 	encryptedData,
				// });
				// loginSuccess(loginRes);
			} else {
				useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
			}
		} else {
			useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
		}
	});
};
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
