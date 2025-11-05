<template>
	<view class="page">工作台</view>
</template>

<script setup lang="ts">
definePage({
	name: "Workbench",
	layout: "layout",
	isTabBar: true,
	style: {
		navigationStyle: "custom",
		navigationBarTitleText: "工作台",
		enablePullDownRefresh: true,
	},
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
