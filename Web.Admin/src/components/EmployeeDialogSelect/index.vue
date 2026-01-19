<template>
	<FaInputDialogPage
		v-bind="$attrs"
		rowKey="employeeId"
		labelKey="employeeName"
		:requestApi="employeeApi.queryEmployeePaged"
		v-model="modelValue"
		v-model:label="employeeName"
		placeholder="请选择职员"
		@change="handleChange"
	>
		<FaTableColumn prop="idPhoto" label="头像" fixed type="image" width="50" smallWidth="50" />
		<FaTableColumn prop="employeeName" label="名称" fixed width="200" smallWidth="180" sortable />
		<FaTableColumn prop="employeeNo" label="工号" width="150" smallWidth="130" sortable />
		<FaTableColumn prop="mobile" label="手机" width="150" smallWidth="130" sortable />
		<FaTableColumn prop="departmentName" label="部门" width="150" smallWidth="130" sortable />
		<FaTableColumn prop="positionName" label="职位" width="150" smallWidth="130" sortable />
		<FaTableColumn prop="jobLevelName" label="职级" width="150" smallWidth="130" sortable />
	</FaInputDialogPage>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import { employeeApi } from "@/api/services/employee";
import { QueryEmployeePagedOutput } from "@/api/services/employee/models/QueryEmployeePagedOutput";

defineOptions({
	name: "EmployeeDialogSelect",
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
	change: (data: QueryEmployeePagedOutput) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: false });
const employeeName = useVModel(props, "employeeName", emit, { passive: true });
const employeeNo = useVModel(props, "employeeNo", emit, { passive: true });
const mobile = useVModel(props, "mobile", emit, { passive: true });

const handleChange = (data: QueryEmployeePagedOutput) => {
	if (data) {
		employeeNo.value = data.employeeNo;
		mobile.value = data.mobile;
		emit("change", data);
	} else {
		employeeNo.value = undefined;
		mobile.value = undefined;
		emit("change", undefined);
	}
};
</script>
