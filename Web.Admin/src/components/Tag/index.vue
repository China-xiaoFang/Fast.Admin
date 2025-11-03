<template>
	<ElTag
		v-if="props.name && dictionary?.show"
		v-bind="elTagProps"
		:type="dictionary?.type ?? props.type"
		:title="dictionary?.tips"
		@click="(evt: MouseEvent) => emit('click', evt)"
		@close="(evt: MouseEvent) => emit('close', evt)"
	>
		<slot label="dictionary?.label" tips="dictionary?.tips">
			{{ dictionary?.label }}
		</slot>
	</ElTag>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { tagEmits, tagProps } from "element-plus";
import { useProps } from "@fast-china/utils";
import { isNil } from "lodash";
import { useApp } from "@/stores";

defineOptions({
	name: "Tag",
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

const elTagProps = useProps(props, tagProps, ["type"]);

const emit = defineEmits({
	...tagEmits,
});

const appStore = useApp();

/** 字典 */
const dictionaries = computed(() => (props.name ? appStore.getDictionary(props.name) : []));
/** 字典值 */
const dictionary = computed(() => (isNil(props.value) ? null : dictionaries.value.find((f) => f.value === props.value)));
</script>
