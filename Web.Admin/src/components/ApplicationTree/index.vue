<template>
	<FaTree
		v-bind="$attrs"
		v-model="modelValue"
		v-model:label="appName"
		title="应用列表"
		width="240"
		:requestApi="applicationApi.applicationSelector"
		@change="(value) => emit('change', value)"
	>
		<template #label="{ data }: { data: ElSelectorOutput<number | string> }">
			<FaAvatar style="margin-right: 5px" :src="data.data.logoUrl" thumb size="small" />
			<span>{{ data.label }}</span>
		</template>
		<template #default="{ data }: { data: ElSelectorOutput<number | string> }">
			<Tag size="small" effect="plain" name="EditionEnum" :value="data.data.edition" />
		</template>
	</FaTree>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import { type ElSelectorOutput } from "fast-element-plus";
import { applicationApi } from "@/api/services/application";

defineOptions({
	name: "ApplicationTree",
});

const props = defineProps<{
	modelValue?: number | string;
	appName?: string;
}>();

const emit = defineEmits({
	"update:modelValue": (value: number | string) => true,
	"update:appName": (value: string) => true,
	change: (value: ElSelectorOutput<number | string>) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: true });
const appName = useVModel(props, "appName", emit, { passive: true });
</script>
