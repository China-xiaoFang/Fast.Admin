<template>
	<view class="page">
		<view class="logo">
			<FaImage width="128rpx" height="128rpx" radius="50%" :src="userInfoStore.avatar" :hideImage="false" />
			<text>{{ userInfoStore.tenantName || appStore.appName }}</text>
		</view>

		<wd-cell-group border>
			<wd-cell title="系统版本" :value="appVersion" />
			<!-- #ifdef APP-PLUS -->
			<wd-cell title="热更新版本" :value="`v${appStore.appVersion}`" />
			<!-- #endif -->
			<wd-cell title="系统服务商" value="FastDotNet" />
			<wd-cell customClass="mb20" title="服务有效期" value="2029-12-31 23:59:59" />
		</wd-cell-group>

		<wd-cell-group border>
			<wd-cell title="消息通知" clickable isLink @click="useToast.info('敬请期待')" />
			<wd-cell customClass="mb20" title="通用" clickable isLink to="/pages/setting/general/index" />
		</wd-cell-group>

		<wd-cell-group border>
			<wd-cell title="用户协议" clickable isLink @click="router.push(CommonRoute.UserAgreement)" />
			<wd-cell title="隐私协议" clickable isLink @click="router.push(CommonRoute.PrivacyAgreement)" />
			<wd-cell title="服务协议" clickable isLink @click="router.push(CommonRoute.ServiceAgreement)" />
			<wd-cell customClass="mb20" title="清除缓存" :value="`${currentSize}/${limitSize}`" clickable isLink @click="handleClearCache" />
		</wd-cell-group>

		<wd-button customClass="btn__exit-login" type="primary" block :round="false" icon="exit" @click="handleLogout">退出登录</wd-button>
	</view>
</template>

<script setup lang="ts">
import { onShow } from "@dcloudio/uni-app";
import { computed, ref } from "vue";
import { clickUtil } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";
import { CommonRoute } from "@/common";
import { useMessageBox, useToast } from "@/hooks";
import { useApp, useUserInfo } from "@/stores";

definePage({
	name: "Setting",
	layout: "layout",
	style: {
		navigationBarTitleText: "设置",
	},
});

const appStore = useApp();
const userInfoStore = useUserInfo();
const router = useRouter();

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
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
