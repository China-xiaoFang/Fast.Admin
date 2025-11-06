<template>
	<view class="page">
		<rich-text :nodes="appStore.userAgreement" />
	</view>
</template>

<script setup lang="ts">
import { useApp } from "@/stores";

definePage({
	name: "UserAgreement",
	layout: "layout",
	footer: false,
	watermark: false,
	noLogin: true,
	style: {
		navigationBarTitleText: "用户协议",
	},
});

const appStore = useApp();
</script>
