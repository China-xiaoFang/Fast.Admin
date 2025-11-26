<template>
	<wd-img
		v-bind="wdImgProps"
		:customClass="`fa-image ${!state.src || state.hideImage ? 'is-small' : ''} ${props.customClass}`"
		:customStyle="`--height:${addUnit(props.height)};--width:${addUnit(props.width)};`"
		:src="state.hideImage ? artwork : (state.src ?? notImage)"
		:previewSrc="state.previewSrc"
		:enablePreview="props.enablePreview && !!state.src"
		@click.stop="(event) => emit('click', event)"
		@load="(event) => emit('load', event)"
		@error="(event) => emit('error', event)"
	>
		<template #loading>
			<wd-icon customClass="fa-image__loading" classPrefix="iconfont" name="loadingImage" />
		</template>
		<template #error>
			<image class="fa-image__error" :src="notImage" mode="scaleToFill" />
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
	/** @description 隐藏图片，优先级最高 */
	hideImage: {
		type: Boolean,
		default: undefined,
	},
	/** @description Base64图片 */
	base64: Boolean,
	/** @description 原图 */
	original: Boolean,
	/** @description 标准 */
	normal: Boolean,
	/** @description 小图 */
	small: Boolean,
	/** @description 缩略图 */
	thumb: Boolean,
});

const wdImgProps = useProps(props, imgProps, ["customClass", "customStyle", "src", "previewSrc", "enablePreview"]);

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
	/** 隐藏图片 */
	hideImage: computed(() => (isNil(props.hideImage) ? configStore.tableLayout.hideImage : props.hideImage)),
	/** 预览地址 */
	previewSrc: computed(() => {
		if (props.src) {
			if (props.base64) {
				return `data:image/png;base64,${props.src}`;
			}
			return props.src;
		}
		return undefined;
	}),
	/** 图片地址 */
	src: computed(() => {
		if (props.src) {
			if (props.base64) {
				return `data:image/png;base64,${props.src}`;
			} else if (props.original) {
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
.is-small {
	padding: calc(var(--height) * 0.1);
	box-sizing: border-box;
}
:deep() {
	.fa-image__loading {
		width: var(--width);
		height: var(--height);
		text-align: center;
		line-height: var(--height);
		font-size: calc(var(--width) * 0.8);
		background-color: var(--wot-bg-color);
		color: var(--wot-text-color-placeholder);
	}
	.fa-image__error {
		width: calc(var(--width) * 0.8);
		height: calc(var(--height) * 0.8);
		padding-top: calc(var(--height) * 0.1);
		padding-left: calc(var(--width) * 0.1);
	}
}
</style>
