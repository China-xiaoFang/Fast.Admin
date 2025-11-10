<template>
	<view v-if="props.visible" class="fa-overlay" :style="{ backgroundColor: `rgba(0, 0, 0, ${props.transparent})` }" />
</template>

<script setup lang="ts">
defineOptions({
	name: "Overlay",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const props = defineProps({
	/** @description 显示 */
	visible: Boolean,
	/** @description 透明度（0 ~ 1） */
	transparent: {
		type: Number,
		default: 0,
	},
});
</script>

<style scoped lang="scss">
.fa-overlay {
	width: 100%;
	height: 100%;
	position: fixed;
	top: 0;
	left: 0;
	z-index: 2001;
	pointer-events: auto;
	cursor: not-allowed;
}
</style>
