<template>
	<view class="fa-footer">
		<text>FastDotnet提供计算服务</text>
		<text>© {{ new Date().getFullYear() }} {{ domain }} v{{ appStore.appVersion }}</text>
	</view>
</template>

<script setup lang="ts">
import { useApp } from "@/stores";

defineOptions({
	// eslint-disable-next-line vue/no-reserved-component-names
	name: "Footer",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const appStore = useApp();

let domain = "";
const host = import.meta.env.VITE_AXIOS_REQUEST_URL.replace(/^https?:\/\//, "").split(/[/:]/)[0];
// ip 或者 localhost
if (/^\d{1,3}(?:\.\d{1,3}){3}$/.test(host) || host === "localhost") {
	domain = host;
} else {
	const hostParts = host.split(".");
	domain =
		hostParts.length >= 2
			? hostParts.slice(-2).join(".") // example.com
			: host; // localhost 或 IP
}
</script>

<style scoped lang="scss">
.fa-footer {
	box-sizing: border-box;
	height: var(--wot-footer-height, 40px);
	font-size: var(--wot-font-size-small);
	color: var(--wot-text-color-secondary);
	text-decoration: none;
	letter-spacing: 0.5px;
	padding: 0;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
	transition: height var(--wot-transition-duration);
	overflow: hidden;
}
</style>
