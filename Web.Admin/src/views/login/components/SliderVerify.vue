<template>
	<div class="slider-verify" :class="{ 'is-success': state.isSuccess }">
		<div class="slider-verify__track" ref="trackRef">
			<div class="slider-verify__fill" :style="{ width: state.moveX + 'px' }"></div>
			<div
				class="slider-verify__thumb"
				:style="{ left: state.moveX + 'px' }"
				@mousedown="handleMouseDown"
				@touchstart.passive="handleTouchStart"
			>
				<el-icon v-if="state.isSuccess"><Check /></el-icon>
				<el-icon v-else><DArrowRight /></el-icon>
			</div>
			<span class="slider-verify__text">{{ state.isSuccess ? "验证成功" : "请按住滑块拖动验证" }}</span>
		</div>
	</div>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue";
import { Check, DArrowRight } from "@element-plus/icons-vue";

defineOptions({
	name: "SliderVerify",
});

const emit = defineEmits<{
	(e: "success"): void;
}>();

const trackRef = ref<HTMLElement>();
const thumbWidth = 42;

const state = reactive({
	isSuccess: false,
	isDragging: false,
	startX: 0,
	moveX: 0,
});

const getMaxDistance = () => {
	if (!trackRef.value) return 0;
	return trackRef.value.offsetWidth - thumbWidth;
};

const handleMouseDown = (event: MouseEvent) => {
	if (state.isSuccess) return;
	state.isDragging = true;
	state.startX = event.clientX - state.moveX;
	document.addEventListener("mousemove", handleMouseMove);
	document.addEventListener("mouseup", handleMouseUp);
};

const handleMouseMove = (event: MouseEvent) => {
	if (!state.isDragging) return;
	const maxDistance = getMaxDistance();
	let moveX = event.clientX - state.startX;
	moveX = Math.max(0, Math.min(moveX, maxDistance));
	state.moveX = moveX;
};

const handleMouseUp = () => {
	if (!state.isDragging) return;
	state.isDragging = false;
	document.removeEventListener("mousemove", handleMouseMove);
	document.removeEventListener("mouseup", handleMouseUp);
	const maxDistance = getMaxDistance();
	if (state.moveX >= maxDistance - 2) {
		state.isSuccess = true;
		state.moveX = maxDistance;
		emit("success");
	} else {
		state.moveX = 0;
	}
};

const handleTouchStart = (event: TouchEvent) => {
	if (state.isSuccess) return;
	state.isDragging = true;
	state.startX = event.touches[0].clientX - state.moveX;
	document.addEventListener("touchmove", handleTouchMove, { passive: true });
	document.addEventListener("touchend", handleTouchEnd);
};

const handleTouchMove = (event: TouchEvent) => {
	if (!state.isDragging) return;
	const maxDistance = getMaxDistance();
	let moveX = event.touches[0].clientX - state.startX;
	moveX = Math.max(0, Math.min(moveX, maxDistance));
	state.moveX = moveX;
};

const handleTouchEnd = () => {
	if (!state.isDragging) return;
	state.isDragging = false;
	document.removeEventListener("touchmove", handleTouchMove);
	document.removeEventListener("touchend", handleTouchEnd);
	const maxDistance = getMaxDistance();
	if (state.moveX >= maxDistance - 2) {
		state.isSuccess = true;
		state.moveX = maxDistance;
		emit("success");
	} else {
		state.moveX = 0;
	}
};

/** 重置 */
const reset = () => {
	state.isSuccess = false;
	state.isDragging = false;
	state.startX = 0;
	state.moveX = 0;
};

defineExpose({ reset });
</script>

<style scoped lang="scss">
.slider-verify {
	width: 100%;
	.slider-verify__track {
		position: relative;
		height: 40px;
		background-color: var(--el-fill-color);
		border-radius: var(--el-border-radius-base);
		border: 1px solid var(--el-border-color);
		overflow: hidden;
		.slider-verify__fill {
			position: absolute;
			top: 0;
			left: 0;
			height: 100%;
			background-color: var(--el-color-primary-light-7);
			transition: none;
			border-radius: var(--el-border-radius-base) 0 0 var(--el-border-radius-base);
		}
		.slider-verify__thumb {
			position: absolute;
			top: 0;
			left: 0;
			width: 42px;
			height: 100%;
			display: flex;
			align-items: center;
			justify-content: center;
			background-color: var(--el-color-white);
			border: 1px solid var(--el-border-color-darker);
			border-radius: var(--el-border-radius-base);
			cursor: grab;
			z-index: 1;
			transition: none;
			user-select: none;
			.el-icon {
				font-size: 18px;
				color: var(--el-color-primary);
			}
			&:hover {
				border-color: var(--el-color-primary);
			}
			&:active {
				cursor: grabbing;
			}
		}
		.slider-verify__text {
			position: absolute;
			top: 0;
			left: 0;
			width: 100%;
			height: 100%;
			display: flex;
			align-items: center;
			justify-content: center;
			font-size: var(--el-font-size-small);
			color: var(--el-text-color-secondary);
			user-select: none;
			pointer-events: none;
		}
	}
	&.is-success {
		.slider-verify__track {
			border-color: var(--el-color-success);
			.slider-verify__fill {
				background-color: var(--el-color-success-light-7);
			}
			.slider-verify__thumb {
				border-color: var(--el-color-success);
				background-color: var(--el-color-success);
				.el-icon {
					color: var(--el-color-white);
				}
			}
			.slider-verify__text {
				color: var(--el-color-success);
				font-weight: var(--el-font-weight-primary);
			}
		}
	}
}
</style>
