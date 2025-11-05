<template>
	<view class="page">登录</view>
</template>

<script setup lang="ts">
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
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
