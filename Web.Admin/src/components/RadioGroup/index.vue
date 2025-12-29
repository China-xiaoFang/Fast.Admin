<template>
	<ElRadioGroup v-bind="elRadioGroupProps" v-model="modelValue" @change="(value: string | number | boolean) => emit('change', value)">
		<ElRadio v-if="!props.button" v-for="(item, index) in dictionaries" :key="index" :value="item.value" :disabled="item.disabled">
			{{ item.label }}
		</ElRadio>
		<ElRadioButton v-if="props.button" v-for="(item, index) in dictionaries" :key="index" :value="item.value" :disabled="item.disabled" border>
			{{ item.label }}
		</ElRadioButton>
	</ElRadioGroup>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { radioGroupEmits, radioGroupProps } from "element-plus";
import { useProps } from "@fast-china/utils";
import { useVModel } from "@vueuse/core";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { useApp } from "@/stores";

defineOptions({
	name: "RadioGroup",
});

const props = defineProps({
	...radioGroupProps,
	/** @description 按钮 */
	button: {
		type: Boolean,
		default: false,
	},
	/** @description 字典名称 */
	name: {
		type: String,
		required: true,
	},
	/** @description v-model绑定值 */
	modelValue: {
		type: [String, Number, Boolean],
		default: CommonStatusEnum.Enable,
	},
});

const elRadioGroupProps = useProps(props, radioGroupProps, ["modelValue"]);

const emit = defineEmits({
	...radioGroupEmits,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: true });

const appStore = useApp();
/** 字典 */
const dictionaries = computed(() => (props.name ? appStore.getDictionary(props.name) : []));
</script>
