<template>
	<el-watermark id="watermark" v-bind="watermarkProps">
		<slot />
	</el-watermark>
</template>

<script setup lang="ts">
import { withDefineType } from "@fast-china/utils";
import { useGlobalSize } from "element-plus";
import { computed, reactive } from "vue";
import { useApp, useConfig, useUserInfo } from "@/stores";

defineOptions({
	name: "Watermark",
});

const _globalSize = useGlobalSize();
const appStore = useApp();
const configStore = useConfig();
const userInfoStore = useUserInfo();

const watermarkProps = reactive({
	rotate: -38,
	gap: withDefineType<[number, number]>([-50, -50]),
	offset: withDefineType<[number, number]>([-10, 0]),
	width: 200,
	height: 300,
	font: computed(() => {
		return {
			fontSize: _globalSize.value === "small" ? 12 : 14,
			color: configStore.layout.isDark ? "rgba(255,255,255,0.2)" : "rgba(0,0,0,0.2)",
		};
	}),
	content: computed(() => {
		let watermarkContent = [appStore.appName];
		if (userInfoStore.tenantName) {
			watermarkContent = [userInfoStore.tenantName];
		}
		if (userInfoStore.asyncRouterGen) {
			watermarkContent.push(userInfoStore.employeeName || userInfoStore.nickName);
		}

		return watermarkContent;
	}),
});
</script>
