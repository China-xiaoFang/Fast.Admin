<template>
	<ElText v-if="props.name && dictionary?.show" v-bind="elTextProps" :type="dictionary?.type ?? props.type" :title="dictionary?.tips">
		<slot :label="dictionary?.label" :tips="dictionary?.tips">
			{{ dictionary?.label }}
		</slot>
	</ElText>
</template>

<script setup lang="ts">
import { useProps } from "@fast-china/utils";
import { textProps } from "element-plus";
import { isNil } from "lodash";
import { computed } from "vue";
import { useApp } from "@/stores";

defineOptions({
	// eslint-disable-next-line vue/no-reserved-component-names
	name: "Text",
});

const props = defineProps({
	...textProps,
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

const elTextProps = useProps(props, textProps, ["type"]);

const appStore = useApp();

/** 字典 */
const dictionaries = computed(() => (props.name ? appStore.getDictionary(props.name) : []));
/** 字典值 */
const dictionary = computed(() => (isNil(props.value) ? null : dictionaries.value.find((f) => f.value === props.value)));
</script>
