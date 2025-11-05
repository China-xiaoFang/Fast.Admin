<template>
	<view class="page">选择租户</view>
</template>

<script setup lang="ts">
definePage({
	name: "SelectTenant",
	layout: "layout",
	pageScroll: false,
	authForbidView: true,
	noLogin: true,
	style: {
		navigationBarTitleText: "选择租户",
		enablePullDownRefresh: true,
	},
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
