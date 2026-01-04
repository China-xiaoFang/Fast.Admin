<template>
	<FaDialog ref="faDialogRef" width="500" :title="state.dialogTitle" @confirm-click="handleConfirm" @close="faFormRef.resetFields()">
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules">
			<FaFormItem prop="resignDate" label="离职日期">
				<el-date-picker
					type="date"
					v-model="state.formData.resignDate"
					:disabledDate="dateUtil.getDisabledDate"
					placeholder="请选择离职日期"
				/>
			</FaFormItem>
			<FaFormItem prop="resignReason" label="离职原因">
				<el-input type="textarea" v-model="state.formData.resignReason" :rows="2" maxlength="200" placeholder="请输入离职原因" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, type FormRules } from "element-plus";
import { dateUtil, withDefineType } from "@fast-china/utils";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { employeeApi } from "@/api/services/employee";
import { EmployeeResignedInput } from "@/api/services/employee/models/EmployeeResignedInput";

defineOptions({
	name: "SystemEmployeeResignedEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EmployeeResignedInput>({
		resignDate: new Date(),
	}),
	formRules: withDefineType<FormRules>({
		resignDate: [{ required: true, message: "请选择离职日期", trigger: "change" }],
		resignReason: [{ required: true, message: "请输入离职原因", trigger: "blur" }],
	}),
	dialogTitle: "职员离职",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		await employeeApi.employeeResigned(state.formData);
		ElMessage.success("离职成功！");
		emit("ok");
	});
};

const open = (employeeId: number) => {
	faDialogRef.value.open(async () => {
		const apiRes = await employeeApi.queryEmployeeDetail(employeeId);
		state.formData = {
			employeeId: apiRes.employeeId,
			rowVersion: apiRes.rowVersion,
		};
		state.dialogTitle = `职员离职 - ${apiRes.employeeName}`;
	});
};

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	open,
});
</script>
