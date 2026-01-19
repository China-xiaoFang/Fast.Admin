<template>
	<FaSelectPage
		v-bind="$attrs"
		:requestApi="employeeApi.employeeSelector"
		v-model="modelValue"
		v-model:label="employeeName"
		placeholder="请选择职员"
		clearable
		moreDetail
		@change="handleChange"
	>
		<template #default="data">
			<div style="display: flex; justify-content: space-between; align-items: center; gap: 8px; width: 100%">
				<FaAvatar :src="data.data.idPhoto" thumb size="small" />
				<div style="flex: 1">
					<span>{{ data.label }}</span>
					<span style="display: flex; justify-content: space-between; width: 100%">
						<span style="font-size: var(--el-font-size-extra-small); padding-right: 8px">{{ data.data?.employeeNo }}</span>
						<span style="font-size: var(--el-font-size-extra-small)">{{ data.data?.mobile }}</span>
					</span>
				</div>
			</div>
		</template>
	</FaSelectPage>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import type { ElSelectorOutput } from "fast-element-plus";
import { employeeApi } from "@/api/services/employee";

defineOptions({
	name: "EmployeeSelectPage",
});

const props = withDefaults(
	defineProps<{
		modelValue?: number | string;
		employeeName?: string;
		employeeNo?: string;
		mobile?: string;
	}>(),
	{}
);

const emit = defineEmits({
	"update:modelValue": (value: number | string) => true,
	"update:employeeName": (value: string) => true,
	"update:employeeNo": (value: string) => true,
	"update:mobile": (value: string) => true,
	change: (value: ElSelectorOutput<number | string>) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: true });
const employeeName = useVModel(props, "employeeName", emit, { passive: true });
const employeeNo = useVModel(props, "employeeNo", emit, { passive: true });
const mobile = useVModel(props, "mobile", emit, { passive: true });

const handleChange = (value: ElSelectorOutput<number | string>) => {
	if (value) {
		employeeNo.value = value.data.email;
		mobile.value = value.data.accountKey;
		emit("change", value);
	} else {
		employeeNo.value = undefined;
		mobile.value = undefined;
		emit("change", undefined);
	}
};
</script>
