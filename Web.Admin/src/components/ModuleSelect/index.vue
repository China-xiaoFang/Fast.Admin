<template>
	<FaSelect
		v-bind="$attrs"
		:requestApi="moduleApi.moduleSelector"
		v-model="modelValue"
		v-model:label="moduleName"
		placeholder="请选择模块"
		clearable
		@change="(value) => emit('change', value)"
	>
		<template #default="data">
			<div style="display: flex; justify-content: space-between; align-items: center; gap: 8px; width: 100%">
				<FaIcon v-if="data.data?.icon" size="16" :name="data.data.icon" />
				<span style="flex: 1">{{ data.label }}</span>
				<Tag v-if="data.data?.status" size="small" effect="plain" name="CommonStatusEnum" :value="data.data.status" />
			</div>
		</template>
	</FaSelect>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import type { ElSelectorOutput } from "fast-element-plus";
import { moduleApi } from "@/api/services/module";

defineOptions({
	name: "ModuleSelect",
});

const props = withDefaults(
	defineProps<{
		modelValue?: number | string;
		moduleName?: string;
	}>(),
	{}
);

const emit = defineEmits({
	"update:modelValue": (value: number | string) => true,
	"update:moduleName": (value: string) => true,
	change: (value: ElSelectorOutput) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: true });
const moduleName = useVModel(props, "moduleName", emit, { passive: true });
</script>
