<template>
	<ElRadioGroup v-bind="elRadioGroupProps" v-model="modelValue" @change="(value: string | number | boolean) => emit('change', value)">
		<template v-if="props.button">
			<ElRadioButton
				v-for="(item, index) in dictionaries"
				:key="index"
				:value="item.value"
				:disabled="item.disabled === false ? undefined : item.disabled"
				border
			>
				{{ item.label }}
			</ElRadioButton>
		</template>
		<template v-else>
			<ElRadio
				v-for="(item, index) in dictionaries"
				:key="index"
				:value="item.value"
				:disabled="item.disabled === false ? undefined : item.disabled"
			>
				{{ item.label }}
			</ElRadio>
		</template>
	</ElRadioGroup>
</template>

<script setup lang="ts">
import { useProps } from "@fast-china/utils";
import { useVModel } from "@vueuse/core";
import { radioGroupEmits, radioGroupProps } from "element-plus";
import { computed } from "vue";
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

const modelValue = useVModel(props, "modelValue", emit, { passive: false });

const appStore = useApp();
/** 字典 */
const dictionaries = computed(() => (props.name ? appStore.getDictionary(props.name).filter((f) => f.show) : []));
</script>
