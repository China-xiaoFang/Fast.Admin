<template>
	<view class="page">我的</view>
</template>

<script setup lang="ts">
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
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
