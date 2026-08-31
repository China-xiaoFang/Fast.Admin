<template>
	<FaDialog
		ref="faDialogRef"
		title="修改密码"
		:show-fullscreen="false"
		show-before-close
		width="450"
		@confirm-click="handleConfirm"
		@close="faFormRef.resetFields()"
	>
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules">
			<FaFormItem label="旧密码" prop="oldPassword">
				<el-input type="password" v-model.trim="state.formData.oldPassword" placeholder="请输入旧密码" />
			</FaFormItem>
			<FaFormItem label="新密码" prop="newPassword">
				<el-input type="password" v-model.trim="state.formData.newPassword" placeholder="请输入新密码" autocomplete="off" />
			</FaFormItem>
			<FaFormItem label="确认密码" prop="confirmPassword">
				<el-input v-model.trim="state.formData.confirmPassword" placeholder="请输入确认新密码" type="password" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>
<script lang="ts" setup>
import { reactive, useTemplateRef } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import { accountApi } from "@/api/services/Center/account";
import { useUserInfo } from "@/stores";
import type { FormRules } from "element-plus";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import type { ChangePasswordInput } from "@/api/services/Center/account/models/ChangePasswordInput";

defineOptions({
	name: "ChangePassword",
});

const userInfoStore = useUserInfo();

const faDialogRef = useTemplateRef<FaDialogInstance>("faDialogRef");
const faFormRef = useTemplateRef<FaFormInstance>("faFormRef");

const state = reactive({
	formData: withDefineType<ChangePasswordInput>({}),
	formRules: withDefineType<FormRules>({
		oldPassword: [{ required: true, message: "请输入旧密码", trigger: "blur" }],
		newPassword: [{ required: true, message: "请输入新密码", trigger: "blur" }],
		confirmPassword: [{ required: true, message: "请输入确认新密码", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
	dialogTitle: "配置",
});

const handleConfirm = async () => {
	await faFormRef.value.validateScrollToField();
	const { newPassword, confirmPassword } = state.formData;
	if (newPassword !== confirmPassword) {
		ElMessage.warning("两次密码输入不一致");
		return;
	}
	faDialogRef.value.close(() => {
		void ElMessageBox.confirm("确认修改密码", {
			type: "warning",
		}).then(async () => {
			await accountApi.changePassword(state.formData);
			await ElMessageBox.alert("修改成功，请重新登录！", {
				type: "success",
				confirmButtonText: "重新登录",
			});
			await userInfoStore.logout();
		});
	});
};

const open = () => {
	faDialogRef.value.open(async () => {
		const apiRes = await accountApi.queryEditAccountDetail();
		state.formData = {
			rowVersion: apiRes.rowVersion,
		};
	});
};

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	open,
});
</script>
