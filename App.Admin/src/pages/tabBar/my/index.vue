<template>
	<view
		class="page"
		:style="{
			'background-image': configStore.layout.isDark ? '' : `url(${appStore.statusBarImageUrl})`,
		}"
	>
		<view
			class="status-bar"
			:style="{
				'background-color': state.isScrolled ? 'var(--wot-navbar-bg-color)' : '',
			}"
		/>

		<wd-navbar class="fa-navbar" :customClass="state.isScrolled ? 'is-scrolled' : ''" :bordered="false" title="个人中心" />

		<view class="user__card">
			<template v-if="!configStore.layout.isDark">
				<image class="top_bg" src="@/static/images/card_top_bg.png" />
				<image class="bottom_bg" src="@/static/images/card_bottom_bg.png" />
			</template>

			<view class="user__warp">
				<view class="top">
					<FaImage width="120rpx" height="120rpx" round original :hideImage="false" :src="userInfoStore.avatar" />
					<view class="user__info" @click="router.push('/pages/setting/userInfo/index')">
						<view class="employeeName">{{ userInfoStore.employeeName }}</view>
						<view class="employeeNo">{{ userInfoStore.employeeNo }}（{{ userInfoStore.departmentName || "暂无部门" }}）</view>
						<view class="nickName">{{ userInfoStore.nickName }}</view>
					</view>
				</view>

				<view class="bottom">
					<view class="bottom__item">
						<text class="txt__1">999999.99</text>
						<text class="txt__2">Fast</text>
					</view>
					<view class="bottom__item">
						<text class="txt__1">999999.99</text>
						<text class="txt__2">Fast</text>
					</view>
					<view class="bottom__item">
						<text class="txt__1">999999.99</text>
						<text class="txt__2">Fast</text>
					</view>
				</view>
			</view>
		</view>

		<view class="mb30 data-card">
			<wd-cell customClass="card__cell" title="我的功能" />
			<view class="card__content">
				<view class="card__item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="test" />
					</wd-badge>
					<text>Fast</text>
				</view>
				<view class="card__item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="test" />
					</wd-badge>
					<text>Fast</text>
				</view>
				<view class="card__item" @click="appStore.makePhoneCall">
					<FaIcon name="call" />
					<text>联系我们</text>
				</view>
				<view class="card__item" @click="router.push(CommonRoute.ComplaintSubmit)">
					<wd-icon name="evaluation" />
					<text>投诉建议</text>
				</view>
			</view>
		</view>

		<view class="mb20 data-card">
			<wd-cell customClass="card__cell" title="绩效" value="全部" clickable isLink />
			<view class="card__content">
				<view class="card__item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="test" />
					</wd-badge>
					<text>Fast</text>
				</view>
				<view class="card__item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="test" />
					</wd-badge>
					<text>Fast</text>
				</view>
				<view class="card__item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="test" />
					</wd-badge>
					<text>Fast</text>
				</view>
				<view class="card__item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="test" />
					</wd-badge>
					<text>Fast</text>
				</view>
			</view>
		</view>

		<wd-cell-group border>
			<wd-cell title="个人资料" clickable isLink to="/pages/setting/userInfo/index">
				<template #icon>
					<FaIcon customClass="wd-cell__icon" name="idCard" />
				</template>
			</wd-cell>
			<wd-cell customClass="mb20" title="账号安全" clickable isLink to="/pages/setting/account/index">
				<template #icon>
					<FaIcon customClass="wd-cell__icon" name="accountSafe" />
				</template>
			</wd-cell>
		</wd-cell-group>

		<wd-cell title="设置" clickable isLink to="/pages/setting/index">
			<template #icon>
				<FaIcon customClass="wd-cell__icon" name="setting" />
			</template>
		</wd-cell>

		<wd-button
			v-if="userInfoStore.hasUserInfo"
			customClass="btn__exit-login"
			type="primary"
			block
			:round="false"
			icon="exit"
			@click="handleLogout"
		>
			退出登录
		</wd-button>
	</view>
</template>

<script setup lang="ts">
import { onPageScroll, onPullDownRefresh } from "@dcloudio/uni-app";
import { reactive } from "vue";
import { clickUtil } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { CommonRoute } from "@/common";
import { useMessageBox, useToast } from "@/hooks";
import { useApp, useConfig, useUserInfo } from "@/stores";

definePage({
	name: "My",
	layout: "layout",
	isTabBar: true,
	style: {
		navigationStyle: "custom",
		navigationBarTitleText: "我的",
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

/** 退出登录 */
const handleLogout = async () => {
	await clickUtil.throttleAsync(async () => {
		await useMessageBox.confirm("确定要退出登录？");
		await userInfoStore.logout();
		useToast.success("退出登录成功");
	});
};

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
