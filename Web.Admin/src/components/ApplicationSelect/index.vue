<template>
	<FaSelect
		v-bind="$attrs"
		:requestApi="applicationApi.applicationSelector"
		v-model="modelValue"
		v-model:label="appName"
		placeholder="请选择应用"
		clearable
		moreDetail
		@change="(value) => emit('change', value)"
	>
		<template #default="data">
			<div style="display: flex; justify-content: space-between; align-items: center; gap: 8px; width: 100%">
				<FaAvatar v-if="data.data?.logoUrl" :src="data.data?.logoUrl" thumb size="small" />
				<div style="flex: 1">
					<span>{{ data.label }}</span>
					<span style="display: flex; justify-content: space-between; width: 100%">
						<span style="font-size: var(--el-font-size-extra-small); padding-right: 8px">{{ data.data?.appNo }}</span>
						<Tag name="EditionEnum" :value="data.data?.edition" size="small" />
					</span>
				</div>
			</div>
		</template>
	</FaSelect>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import type { ElSelectorOutput } from "fast-element-plus";
import { applicationApi } from "@/api/services/application";

defineOptions({
	name: "ApplicationSelect",
});

const props = withDefaults(
	defineProps<{
		modelValue?: number | string;
		appName?: string;
	}>(),
	{}
);

const emit = defineEmits({
	"update:modelValue": (value: number | string) => true,
	"update:appName": (value: string) => true,
	change: (value: ElSelectorOutput<number | string>) => true,
});

const modelValue = useVModel(props, "modelValue", emit);
const appName = useVModel(props, "appName", emit);
</script>
