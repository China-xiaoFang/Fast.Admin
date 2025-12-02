<template>
	<view class="page">
		<rich-text :nodes="appStore.serviceAgreement" />
	</view>
</template>

<script setup lang="ts">
import { useApp } from "@/stores";

definePage({
	name: "ServiceAgreement",
	layout: "layout",
	footer: false,
	watermark: false,
	style: {
		navigationBarTitleText: "服务协议",
	},
});

const appStore = useApp();
</script>

<style scoped lang="scss">
.page {
	background-color: var(--wot-bg-color);
}
</style>
