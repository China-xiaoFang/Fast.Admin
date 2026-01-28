<template>
	<wd-tag
		v-bind="wdTagProps"
		:type="type"
		@click="(evt: MouseEvent) => emit('click', evt)"
		@close="(evt: MouseEvent) => emit('close', evt)"
		@confirm="({ value }: { value: string }) => emit('confirm', { value })"
	>
		<slot :label="dictionary?.label" :tips="dictionary?.tips">
			{{ dictionary?.label }}
		</slot>
		<template #icon>
			<slot name="icon" />
		</template>
		<template #add>
			<slot name="add" />
		</template>
	</wd-tag>
</template>

<script setup lang="ts">
import { useProps } from "@fast-china/utils";
import { isNil } from "lodash-unified";
import { computed } from "vue";
import { tagProps } from "wot-design-uni/components/wd-tag/types";
import { useApp } from "@/stores";

defineOptions({
	name: "Tag",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const props = defineProps({
	...tagProps,
	/** @description 字典名称 */
	name: {
		type: String,
		required: true,
	},
	/** @description 值 */
	value: {
		type: [Number, String, Boolean],
		default: undefined,
	},
});

const wdTagProps = useProps(props, tagProps, ["type"]);

const emit = defineEmits({
	/** @description 标签点击时触发 */
	click: (event: MouseEvent): boolean => true,
	/** @description 点击关闭按钮时触发 */
	close: (event: MouseEvent): boolean => true,
	/** @description 新增标签输入内容确定后触发 */
	confirm: ({ value }: { value: string }): boolean => true,
});

const appStore = useApp();

/** 字典 */
const dictionaries = computed(() => (props.name ? appStore.getDictionary(props.name) : []));
/** 字典值 */
const dictionary = computed(() => (isNil(props.value) ? null : dictionaries.value.find((f) => f.value === props.value)));

const type = computed(() => (dictionary?.value.type === "info" ? "default" : dictionary?.value.type));
</script>
