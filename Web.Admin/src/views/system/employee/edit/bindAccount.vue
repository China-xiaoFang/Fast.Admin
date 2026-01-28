<template>
	<FaDialog ref="faDialogRef" width="500" :title="state.dialogTitle" @confirm-click="handleConfirm" @close="faFormRef.resetFields()">
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules">
			<FaFormItem prop="account" label="账号">
				<el-input v-model="state.formData.account" maxlength="20" placeholder="请输入账号">
					<template #prepend>{{ userInfoStore.tenantCode }}_</template>
				</el-input>
			</FaFormItem>
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
import { reactive, ref } from "vue";
import { ElMessage, type FormRules } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import { employeeApi } from "@/api/services/Admin/employee";
import { BindLoginAccountInput } from "@/api/services/Admin/employee/models/BindLoginAccountInput";
import { useUserInfo } from "@/stores";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";

defineOptions({
	name: "SystemEmployeeBindAccount",
});

const emit = defineEmits(["ok"]);

const userInfoStore = useUserInfo();

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<BindLoginAccountInput>({}),
	formRules: withDefineType<FormRules>({
		account: [{ required: true, message: "请输入账号", trigger: "blur" }],
		mobile: [{ required: true, message: "请输入手机", trigger: "blur" }],
		email: [{ required: true, message: "请输入邮箱", trigger: "blur" }],
	}),
	dialogTitle: "绑定登录账号",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		await employeeApi.bindLoginAccount(state.formData);
		ElMessage.success("绑定成功！");
		emit("ok");
	});
};

const open = (employeeId: number) => {
	faDialogRef.value.open(async () => {
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
