<template>
	<FaTreeSelect
		v-bind="$attrs"
		:request-api="() => departmentApi.departmentSelector(props.orgId)"
		v-model="modelValue"
		v-model:label="departmentName"
		:placeholder="props.placeholder || '请选择部门'"
		check-strictly
		filterable
		clearable
		@change="(value) => emit('change', value)"
	/>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import { departmentApi } from "@/api/services/Admin/department";
import type { ElSelectorOutput } from "fast-element-plus";

defineOptions({
	name: "DepartmentTreeSelect",
});

const props = defineProps<{
	modelValue?: string | string[];
	departmentName?: string;
	orgId?: string;
	placeholder?: string;
}>();

const emit = defineEmits({
	"update:modelValue": (_value: string | string[]) => true,
	"update:departmentName": (_value: string | string[]) => true,
	change: (_value: ElSelectorOutput) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: false });
const departmentName = useVModel(props, "departmentName", emit, { passive: false });
</script>
