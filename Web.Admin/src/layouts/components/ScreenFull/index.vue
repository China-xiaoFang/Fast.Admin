<template>
	<el-icon class="screen-full fa__hover__twinkle" :title="isFullscreen ? '退出全屏' : '全屏'" @click="handleScreenFullClick">
		<FullScreenExit v-if="isFullscreen" />
		<FullScreen v-else />
	</el-icon>
</template>

<script setup lang="ts">
import { computed, onMounted } from "vue";
import { useScreenFull } from "fast-element-plus";
import { FullScreen, FullScreenExit } from "@fast-element-plus/icons-vue";

defineOptions({
	name: "ScreenFull",
});

const isFullscreen = computed(() => useScreenFull.isFullscreen());

const handleScreenFullClick = (): void => {
	useScreenFull.toggle();
};

onMounted(() => {
	useScreenFull.init();
});
</script>

<style scoped lang="scss">
.screen-full {
	width: 100%;
	height: 100%;
	font-size: var(--el-font-size-large);
	cursor: pointer;
}
html.small {
	.screen-full {
		font-size: var(--el-font-size-medium);
	}
}
</style>
