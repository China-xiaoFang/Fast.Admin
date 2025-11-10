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
							<text class="tenant-name">
								{{ userInfoStore.tenantName }}
							</text>
							<view>
								<text class="nick-name">{{ userInfoStore.employeeNo }}123</text>
								<text class="nick-name">{{ userInfoStore.departmentName }}123</text>
							</view>
							<text class="nick-name">
								{{ userInfoStore.employeeName || userInfoStore.nickName }}
							</text>
						</view>
					</view>
				</view>
				<view class="navbar__warp__bottom">
					<view class="bottom-item">
						<text class="item_txt1">999999.99</text>
						<text class="item_txt2">入库</text>
					</view>
					<view class="bottom-item">
						<text class="item_txt1">999999.99</text>
						<text class="item_txt2">出库</text>
					</view>
					<view class="bottom-item">
						<text class="item_txt1">999999.99</text>
						<text class="item_txt2">配送</text>
					</view>
				</view>
			</view>
		</view>

		<view class="data-card">
			<wd-cell customClass="wd-cell-border" title="作业" />
			<view class="data-card__content">
				<view class="cell-item">
					<wd-badge :modelValue="9999" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">收货</text>
				</view>
				<view class="cell-item">
					<wd-badge :modelValue="88" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">上架</text>
				</view>
				<view class="cell-item">
					<wd-badge :modelValue="0" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">拣货</text>
				</view>
				<view class="cell-item">
					<wd-badge :modelValue="99" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">复核</text>
				</view>
				<view class="cell-item">
					<wd-badge :modelValue="9" :max="99">
						<FaIcon name="performance" />
					</wd-badge>
					<text class="item_txt">盘点</text>
				</view>
			</view>
		</view>
		<view class="data-card">
			<wd-cell customClass="wd-cell-border" title="绩效" value="全部" clickable isLink />
			<view class="data-card__content">
				<view class="cell-item">
					<FaIcon name="performance" />
					<text class="item_txt">收货</text>
				</view>
				<view class="cell-item">
					<FaIcon name="performance" />
					<text class="item_txt">上架</text>
				</view>
				<view class="cell-item">
					<FaIcon name="performance" />
					<text class="item_txt">拣货</text>
				</view>
				<view class="cell-item">
					<FaIcon name="performance" />
					<text class="item_txt">复核</text>
				</view>
				<view class="cell-item">
					<FaIcon name="performance" />
					<text class="item_txt">配送</text>
				</view>
			</view>
		</view>

		<wd-cell-group border>
			<wd-cell title="个人资料" clickable isLink to="/pages/setting/userInfo/index">
				<template #icon>
					<FaIcon customClass="wd-cell__icon" name="idCard" />
				</template>
			</wd-cell>
		</wd-cell-group>

		<wd-cell title="设置" clickable isLink to="/pages/setting/index">
			<template #icon>
				<FaIcon customClass="wd-cell__icon" name="setting" />
			</template>
		</wd-cell>

		<wd-button customClass="btn__exit-login" type="primary" block :round="false" icon="exit" @tap="handleLogout">退出登录</wd-button>
	</view>
</template>

<script setup lang="ts">
import { onPageScroll, onPullDownRefresh } from "@dcloudio/uni-app";
import { reactive } from "vue";
import { clickUtil } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { useMessageBox, useToast } from "@/hooks";
import defaultAvatar from "@/static/images/avatar.jpg";
import { useConfig, useUserInfo } from "@/stores";

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

const configStore = useConfig();
const userInfoStore = useUserInfo();
const router = useRouter();

const state = reactive({
	/** 是否滚动 */
	isScrolled: false,
});

const handleLogout = async () => {
	await clickUtil.throttleAsync(async () => {
		try {
			await useMessageBox.confirm("确定要退出登录？");
			await userInfoStore.logout();
			useToast.success("退出登录成功");
		} catch {}
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
