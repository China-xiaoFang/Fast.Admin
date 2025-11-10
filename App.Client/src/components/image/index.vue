<template>
	<wd-img
		v-bind="wdImgProps"
		:customClass="`fa-image ${props.customClass}`"
		:customStyle="`--height:${addUnit(props.height)};--width:${addUnit(props.width)};--border-radio:${addUnit(props.radius)};`"
		:src="state.hideImage ? artwork : (state.src ?? notImage)"
		:previewSrc="props.src"
		@click="(event) => emit('click', event)"
		@load="(event) => emit('load', event)"
		@error="(event) => emit('error', event)"
	>
		<template #loading>
			<wd-icon customClass="fa-image__loading" classPrefix="iconfont" name="loadingImage" />
		</template>
		<template #error>
			<WdIcon customClass="fa-image__error" classPrefix="iconfont" name="notImage" />
		</template>
	</wd-img>
</template>

<script setup lang="ts">
import { computed, reactive } from "vue";
import { addUnit, useProps } from "@fast-china/utils";
import { isNil } from "lodash-unified";
import { imgProps } from "wot-design-uni/components/wd-img/types";
import artwork from "./images/artwork.png";
import notImage from "./images/notImage.png";
import { useConfig } from "@/stores";

defineOptions({
	// eslint-disable-next-line vue/no-reserved-component-names
	name: "Image",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const props = defineProps({
	...imgProps,
	/** @description 是否允许预览 */
	enablePreview: {
		type: Boolean,
		default: true,
	},
	/** @description 宽度 */
	width: {
		type: [String, Number],
		default: "100%",
	},
	/** @description 高度 */
	height: {
		type: [String, Number],
		default: "100%",
	},
	/** @description 圆角大小 */
	radius: {
		type: [String, Number],
		default: "unset",
	},
	/** @description 隐藏图片，优先级最高 */
	hideImage: Boolean,
	/** @description 原图 */
	original: Boolean,
	/** @description 标准 */
	normal: Boolean,
	/** @description 小图 */
	small: Boolean,
	/** @description 缩略图 */
	thumb: Boolean,
});

const wdImgProps = useProps(props, imgProps, ["customClass", "customStyle", "src", "previewSrc"]);

const emit = defineEmits({
	/** @description 点击事件 */
	click: (event: MouseEvent): boolean => true,
	/** @description 当图片载入完毕时触发 */
	load: (event: Event): boolean => true,
	/** @description 当错误发生时触发 */
	error: (event: Event): boolean => true,
});

const configStore = useConfig();

const state = reactive({
	hideImage: computed(() => (isNil(props.hideImage) ? configStore.tableLayout.hideImage : props.hideImage)),
	src: computed(() => {
		if (props.src) {
			if (props.original) {
				return props.src;
			} else if (props.normal) {
				return `${props.src}@!normal`;
			} else if (props.small) {
				return `${props.src}@!small`;
			} else if (props.thumb) {
				return `${props.src}@!thumb`;
			} else {
				// 默认使用缩略图
				return `${props.src}@!thumb`;
			}
		}
		return undefined;
	}),
});

defineExpose({
	/** @description 图片路径 */
	src: computed(() => state.src),
});
</script>

<style scoped lang="scss">
:deep() {
	.fa-image__loading,
	.fa-image__error {
		width: var(--width);
		height: var(--height);
		text-align: center;
		line-height: var(--height);
		font-size: calc(var(--width) * 0.8);
		background-color: var(--wot-bg-color);
		color: var(--wot-text-color-placeholder);
	}
}
</style>
