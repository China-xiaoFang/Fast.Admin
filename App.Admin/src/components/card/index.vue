<template>
	<view class="fa__card mx20 mb20" :style="{ color: 'var(--wot-text-color)' }">
		<view v-if="props.title || props.more" class="fa__card__top mb10">
			<view class="fa__card__title">{{ props.title }}</view>
			<view v-if="props.more" class="fa__card__more" @click="emit('moreClick', $event)">
				更多
				<wd-icon class="fa__card__more-icon" color="var(--wot-orange-dark)" name="arrow-right" />
			</view>
		</view>

		<view v-bind="$attrs" class="fa__card__warp fa-card-shadow">
			<slot />
		</view>
	</view>
</template>

<script setup lang="ts">
defineOptions({
	name: "FaCard",
	inheritAttrs: false,
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const props = defineProps({
	/** 标题 */
	title: [String, Boolean],
	/** 更多 */
	more: {
		type: Boolean,
		default: true,
	},
});

const emit = defineEmits<{
	(e: "moreClick", event: Event): void;
}>();
</script>

<style scoped lang="scss">
.fa__card {
	.fa__card__title {
		position: relative;
		font-size: var(--wot-font-size-lg);
		font-weight: 500;
		user-select: none;
		&::after {
			content: "";
			position: absolute;
			left: 0;
			bottom: -8rpx;
			width: 240rpx;
			height: 6rpx;
			background: linear-gradient(to right, var(--wot-color-primary), transparent);
			border-radius: var(--wot-radius-sm);
		}
	}
	.fa__card__top {
		display: flex;
		align-items: center;
		justify-content: space-between;
		padding-right: 10rpx;
		user-select: none;
	}
	.fa__card__more {
		display: flex;
		align-items: flex-end;
		gap: 5rpx;
		cursor: pointer;
	}
	.fa__card__more-icon {
		font-size: var(--wot-font-size-lg);
	}
	.fa__card__warp {
		box-sizing: border-box;
		padding: 20rpx;
		border-radius: 30rpx;
		background-color: var(--wot-bg-color-container);
		overflow: hidden;
	}
	:deep(.eChart__warp) {
		width: 100%;
		min-height: 360rpx;
	}
}
</style>
