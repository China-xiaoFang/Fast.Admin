<template>
	<div class="logo" :style="{ '--height': addUnit(configStore.layout.navBarHeight) }" title="首页" @click="router.push('/')">
		<img :src="logo" alt="Logo" @error="setFallBack" />
		<span v-if="!configStore.layout.menuCollapse" :title="appStore.appName">
			{{ appStore.appName }}
		</span>
	</div>
</template>

<script setup lang="ts">
import { addUnit } from "@fast-china/utils";
import { computed } from "vue";
import { useRouter } from "vue-router";
import LogoImg from "@/assets/logo.png";
import { useApp, useConfig } from "@/stores";

defineOptions({
	name: "Logo",
});

const router = useRouter();
const appStore = useApp();
const configStore = useConfig();

const logo = computed(() => appStore.logoUrl || LogoImg);

const setFallBack = (event: Event) => {
	const target = event.target as HTMLImageElement;
	target.src = LogoImg;
};
</script>

<style scoped lang="scss">
html.small {
	.logo {
		span {
			font-size: var(--el-font-size-medium);
		}
	}
}

.logo {
	height: var(--height);
	display: flex;
	align-items: center;
	justify-content: center;
	gap: 10px;
	padding: 0 10px;
	// border-bottom: var(--el-border);
	box-sizing: border-box;
	cursor: pointer;
	img {
		height: 80%;
	}
	span {
		font-size: var(--el-font-size-large);
		font-weight: var(--el-font-weight-primary);
		max-width: 100%;
		display: block;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}
}
</style>
