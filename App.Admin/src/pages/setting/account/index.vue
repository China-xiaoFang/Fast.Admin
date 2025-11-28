<template>
	<view class="page">
		<!-- #ifdef MP-WEIXIN -->
		<wd-cell v-if="wechatInfo.wechatID" customClass="mb20" value="解除绑定" clickable isLink center @click="handleUnBindWechat">
			<template #title>
				<view class="avatar">
					<image :src="wechatInfo.avatarUrl ?? defaultAvatar" />
					<wd-text :text="wechatInfo.nickName" />
				</view>
			</template>
		</wd-cell>
		<!-- #endif -->

		<wd-cell-group border>
			<wd-cell title="修改密码" clickable isLink to="/pages/setting/account/changePassword/index" />
			<wd-cell title="通过短信找回密码" clickable isLink to="/pages/setting/account/changePasswordByVerifyCode/index" />
			<wd-cell v-if="wechatInfo.wechatID" title="通过微信找回密码" clickable isLink to="/pages/setting/account/changePasswordByWechat/index" />
		</wd-cell-group>
	</view>
</template>

<script setup lang="ts">
definePage({
	name: "SettingAccount",
	layout: "layout",
	style: {
		navigationBarTitleText: "账号安全",
	},
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
