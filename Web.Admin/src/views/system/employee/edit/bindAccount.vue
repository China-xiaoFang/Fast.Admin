<template>
	<FaDialog ref="faDialogRef" width="500" :title="state.dialogTitle" @confirm-click="handleConfirm" @close="faFormRef.resetFields()">
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules">
			<FaFormItem prop="mobile" label="手机">
				<el-input v-model="state.formData.mobile" maxlength="11" placeholder="请输入手机" />
			</FaFormItem>
			<FaFormItem prop="email" label="邮箱">
				<el-input v-model="state.formData.email" maxlength="50" placeholder="请输入邮箱" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, useTemplateRef } from "vue";
import { ElMessage } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import { employeeApi } from "@/api/services/Admin/employee";
import type { FormRules } from "element-plus";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import type { BindLoginAccountInput } from "@/api/services/Admin/employee/models/BindLoginAccountInput";

defineOptions({
	name: "SystemEmployeeBindAccount",
});

const emit = defineEmits(["ok"]);

const faDialogRef = useTemplateRef<FaDialogInstance>("faDialogRef");
const faFormRef = useTemplateRef<FaFormInstance>("faFormRef");

const state = reactive({
	formData: withDefineType<BindLoginAccountInput>({}),
	formRules: withDefineType<FormRules>({
		mobile: [{ required: true, message: "请输入手机", trigger: "blur" }],
		email: [{ required: true, message: "请输入邮箱", trigger: "blur" }],
	}),
	dialogTitle: "绑定登录账号",
});

const handleConfirm = () => {
	void faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		await employeeApi.bindLoginAccount(state.formData);
		ElMessage.success("绑定成功！");
		emit("ok");
	});
};

const open = (employeeId: number) => {
	void faDialogRef.value.open(async () => {
		const apiRes = await employeeApi.queryEmployeeDetail(employeeId);
		state.formData = {
			employeeId: apiRes.employeeId,
			mobile: apiRes.mobile,
			email: apiRes.email,
			rowVersion: apiRes.rowVersion,
		};
		state.dialogTitle = `绑定登录账号 - ${apiRes.employeeName}`;
	});
};

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	open,
});
</script>
