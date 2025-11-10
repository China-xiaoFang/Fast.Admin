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
					<wd-button v-if="!userInfoStore.hasUserInfo" size="small" type="primary" @tap="handleLogin">登录</wd-button>
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
			<wd-cell title="热更新版本" :value="`v${appStore.appVersion}`" />
			<wd-cell title="系统服务商" value="FastDotNet" clickable isLink />
			<wd-cell customClass="mb20" title="服务有效期" value="2029-12-31 23:59:59" clickable isLink />
		</wd-cell-group>

		<wd-cell-group border>
			<wd-cell title="消息通知" clickable isLink />
			<wd-cell title="通用" clickable isLink to="/pages/setting/general/index" />
			<wd-cell customClass="mb20" title="清除缓存" :value="`${currentSize}/${limitSize}`" clickable isLink @click="handleClearCache" />
		</wd-cell-group>

		<view class="agreement mb20">
			<text @tap.stop="router.push({ path: CommonRoute.UserAgreement })">《用户协议》</text>
			<text @tap.stop="router.push({ path: CommonRoute.PrivacyAgreement })">《隐私协议》</text>
			<text @tap.stop="router.push({ path: CommonRoute.ServiceAgreement })">《服务协议》</text>
		</view>

		<wd-button customClass="btn__exit-login" type="primary" block :round="false" icon="exit" @tap="handleLogout">退出登录</wd-button>
	</view>
</template>

<script setup lang="ts">
import { onPageScroll, onPullDownRefresh, onShow } from "@dcloudio/uni-app";
import { computed, reactive, ref } from "vue";
import { clickUtil } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
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
