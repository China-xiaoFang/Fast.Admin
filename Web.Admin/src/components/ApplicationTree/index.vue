<template>
	<FaTree
		v-bind="$attrs"
		v-model="modelValue"
		v-model:label="appName"
		title="应用列表"
		width="240"
		:request-api="applicationApi.applicationSelector"
		@change="(data) => emit('change', data)"
	>
		<template #label="{ data }">
			<FaAvatar style="margin-right: 5px" :src="data.data?.logoUrl" thumb size="small" />
			<span>{{ data.label }}</span>
		</template>
		<template #default="{ data }">
			<Tag size="small" effect="plain" name="EditionEnum" :value="data.data?.edition" />
		</template>
	</FaTree>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import { applicationApi } from "@/api/services/Center/application";
import type { ElTreeOutput } from "fast-element-plus";

defineOptions({
	name: "ApplicationTree",
});

const props = defineProps<{
	modelValue?: string;
	appName?: string;
}>();

const emit = defineEmits({
	"update:modelValue": (_value: string) => true,
	"update:appName": (_value: string) => true,
	change: (_value: ElTreeOutput) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: false });
const appName = useVModel(props, "appName", emit, { passive: false });
</script>
