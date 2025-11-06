<template>
	<view class="page">
		<rich-text :nodes="appStore.privacyAgreement" />
	</view>
</template>

<script setup lang="ts">
import { useApp } from "@/stores";

definePage({
	name: "PrivacyAgreement",
	layout: "layout",
	footer: false,
	watermark: false,
	noLogin: true,
	style: {
		navigationBarTitleText: "隐私协议",
	},
});

const appStore = useApp();
</script>
