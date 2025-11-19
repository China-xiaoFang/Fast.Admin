<template>
	<view
		class="page"
		:style="{
			'background-image': configStore.layout.isDark ? '' : `url('/static/images/topImage2.png')`,
		}"
	>
		<view
			class="status-bar"
			:style="{
				'background-color': state.isScrolled ? 'var(--wot-navbar-bg-color)' : '',
			}"
		/>
		<view class="navbar">
			<view class="navbar__title">个人中心</view>
			<view class="navbar__warp">
				<template v-if="configStore.layout.isDark"></template>
				<template v-else>
					<image class="top_bg" src="@/static/images/card_top_bg.png" />
					<image class="bottom_bg" src="@/static/images/card_bottom_bg.png" />
				</template>
				<view class="navbar__warp__top">
					<view class="top__left" @tap="router.push('/pages/setting/userInfo/index')">
						<image class="avatar" :src="userInfoStore.avatar || defaultAvatar" />
						<view class="account-info">
							<template v-if="userInfoStore.hasUserInfo">
								<view class="nick-name">
									{{ userInfoStore.nickName }}
								</view>
								<view class="mobile">
									{{ userInfoStore.mobile }}
								</view>
							</template>
							<template v-else>
								<view class="nick-name">{{ appStore.appName }}用户</view>
								<view class="mobile">登录后体验更多功能</view>
							</template>
						</view>
					</view>
					<wd-button v-if="!userInfoStore.hasUserInfo" size="small" openType="getUserInfo" type="primary" @getuserinfo="handleWeChatLogin">
						登录
					</wd-button>
				</view>
			</view>
		</view>

		<view class="data-card">
			<wd-cell customClass="wd-cell-border" title="我的功能" />
			<view class="data-card__content">
				<view class="cell-item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">挂号记录</text>
				</view>
				<view class="cell-item">
					<wd-badge :modelValue="88" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">投诉建议</text>
				</view>
				<view class="cell-item">
					<wd-badge :modelValue="0" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">联系我们</text>
				</view>
			</view>
		</view>

		<wd-cell-group border>
			<wd-cell title="系统版本" :value="appVersion" />
			<!-- #ifdef APP-PLUS -->
			<wd-cell title="热更新版本" :value="`v${appStore.appVersion}`" />
			<!-- #endif -->
			<template v-if="appStore.tenantName">
				<wd-cell title="系统服务商" :value="appStore.tenantName" />
				<wd-cell title="服务有效期" value="2029-12-31 23:59:59" />
			</template>
		</wd-cell-group>

		<wd-cell-group border>
			<wd-cell customClass="mt20" title="消息通知" clickable isLink />
			<wd-cell title="通用" clickable isLink to="/pages/setting/general/index" />
			<wd-cell customClass="mb20" title="清除缓存" :value="`${currentSize}/${limitSize}`" clickable isLink @click="handleClearCache" />
		</wd-cell-group>

		<view class="agreement mb20">
			<text @tap.stop="router.push({ path: CommonRoute.UserAgreement })">《用户协议》</text>
			<text @tap.stop="router.push({ path: CommonRoute.PrivacyAgreement })">《隐私协议》</text>
			<text @tap.stop="router.push({ path: CommonRoute.ServiceAgreement })">《服务协议》</text>
		</view>

		<wd-button v-if="userInfoStore.hasUserInfo" customClass="btn__exit-login" type="primary" block :round="false" icon="exit" @tap="handleLogout">
			退出登录
		</wd-button>
		<wd-message-box selector="confirm-agreement-box">
			<view class="agreement__warp">
				我已阅读并同意
				<text @tap.stop="router.push({ path: CommonRoute.UserAgreement })">《用户协议》</text>
				<text @tap.stop="router.push({ path: CommonRoute.PrivacyAgreement })">《隐私协议》</text>
				<text @tap.stop="router.push({ path: CommonRoute.ServiceAgreement })">《服务协议》</text>
			</view>
		</wd-message-box>
	</view>
</template>

<script setup lang="ts">
import { onPageScroll, onPullDownRefresh, onShow } from "@dcloudio/uni-app";
import { computed, reactive, ref } from "vue";
import { clickUtil, consoleLog } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { useMessage } from "wot-design-uni";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";
import { CommonRoute } from "@/common";
import { useMessageBox, useToast } from "@/hooks";
import defaultAvatar from "@/static/images/avatar.jpg";
import { useApp, useConfig, useUserInfo } from "@/stores";

definePage({
	name: "My",
	layout: "layout",
	isTabBar: true,
	style: {
		navigationStyle: "custom",
		navigationBarTitleText: "个人中心",
		enablePullDownRefresh: true,
	},
});

const appStore = useApp();
const configStore = useConfig();
const userInfoStore = useUserInfo();
const router = useRouter();

const confirmAgreementBox = useMessage("confirm-agreement-box");

const state = reactive({
	/** 是否滚动 */
	isScrolled: false,
});

const storageInfo = ref<Partial<UniNamespace.GetStorageInfoSuccess>>({});

const appVersion = computed(() => {
	let envName = "";
	switch (appStore.environmentType) {
		case EnvironmentTypeEnum.Production:
			envName = "生产版";
			break;
		case EnvironmentTypeEnum.Development:
			envName = "开发版";
			break;
		case EnvironmentTypeEnum.Test:
			envName = "测试版";
			break;
		case EnvironmentTypeEnum.UAT:
			envName = "验收版";
			break;
		case EnvironmentTypeEnum.PreProduction:
			envName = "预生产版";
			break;
		case EnvironmentTypeEnum.GrayDeployment:
			envName = "灰度测试版";
			break;
		case EnvironmentTypeEnum.StressTest:
			envName = "压测版";
			break;
	}
	return `${envName} ${appStore.appBaseInfo.appVersion}`;
});

const currentSize = computed(() => {
	if (storageInfo.value.currentSize >= 1024) {
		const mb = storageInfo.value.currentSize / 1024;
		return `${Number.isInteger(mb) ? mb : mb.toFixed(2)}MB`;
	} else {
		return `${storageInfo.value.currentSize}KB`;
	}
});

const limitSize = computed(() => {
	if (storageInfo.value.limitSize >= 1024) {
		const mb = storageInfo.value.limitSize / 1024;
		return `${Number.isInteger(mb) ? mb : mb.toFixed(2)}MB`;
	} else {
		return `${storageInfo.value.limitSize}KB`;
	}
});

/** 微信登录 */
const handleWeChatLogin = async (detail: UniNamespace.GetUserInfoRes) => {
	await clickUtil.throttleAsync(async () => {
		try {
			await confirmAgreementBox.confirm({
				confirmButtonText: "同意",
				cancelButtonText: "取消",
				closeOnClickModal: false,
			});
		} catch {
			useToast.warning(`登录前需确认您已阅读并同意《用户协议》、《隐私协议》、《服务协议》，以便为您提供更优质的服务。`);
		}
		const { iv, encryptedData, userInfo } = detail;
		if (userInfo) {
			consoleLog("Login", "GetUserInfo", userInfo);
			await userInfoStore.login(
				{
					iv,
					encryptedData,
				},
				true
			);
		} else {
			useToast.warning("授权失败，无法获取您的信息。请重新授权以继续使用我们的服务。");
		}
	});
};

const handleClearCache = async () => {
	await clickUtil.throttleAsync(async () => {
		await useMessageBox.confirm("清除缓存后需要重新登录，确定要清除？");
		appStore.clearAppCache();
		await userInfoStore.logout();
		useToast.success("退出登录成功");
	});
};

const handleLogout = async () => {
	await clickUtil.throttleAsync(async () => {
		await useMessageBox.confirm("确定要退出登录？");
		await userInfoStore.logout();
		useToast.success("退出登录成功");
	});
};

onShow(() => {
	storageInfo.value = uni.getStorageInfoSync();
});

onPageScroll(({ scrollTop }) => {
	state.isScrolled = scrollTop > 0;
});

onPullDownRefresh(async () => {
	// 刷新App信息
	await userInfoStore.refreshApp();
	uni.stopPullDownRefresh();
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
