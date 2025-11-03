<template>
	<div class="main" @dragstart.prevent @contextmenu.prevent @selectstart.prevent @pointerdown="tendrilRef.click">
		<div class="main_warp">
			<h2 class="text text1">遇见你，真幸运</h2>
			<h2 class="text text2">当你的能力还撑不起你的野心时，你就需要静下心来 好好学习。</h2>
			<h2 class="text text2">接受自己的平庸和普通，是成长的必修课。</h2>
			<h2 class="text text2">你羡慕的生活都是你没熬过的苦。</h2>
		</div>
		<Tendril ref="tendrilRef" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import Tendril from "@/components/Tendril/index.vue";

defineOptions({
	name: "Dashboard",
});

const tendrilRef = ref<InstanceType<typeof Tendril>>();
</script>

<style scoped lang="scss">
.main_warp {
	position: relative;
	cursor: none;
	z-index: 2;
	width: 100%;
	height: 100%;
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
	text-shadow: 0 0 4px var(--el-text-color-primary);
	.text {
		align-self: center;
		overflow-wrap: break-word;
		text-transform: lowercase;
	}
	.text1 {
		letter-spacing: 14px;
		font-size: 48px;
		padding-bottom: 10px;
	}
	.text2 {
		letter-spacing: 8px;
		font-size: 24px;
	}
}
</style>
