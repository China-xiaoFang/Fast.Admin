<template>
	<view class="page">首页</view>
</template>

<script setup lang="ts">
definePage({
	name: "Home",
	layout: "layout",
	isTabBar: true,
	style: {
		navigationBarTitleText: "首页",
		enablePullDownRefresh: true,
	},
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
