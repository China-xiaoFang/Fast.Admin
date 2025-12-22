<template>
	<FaDialog
		ref="faDialogRef"
		width="800"
		:title="state.dialogTitle"
		:showConfirmButton="!state.formDisabled"
		:showBeforeClose="!state.formDisabled"
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
		@close="faFormRef.resetFields()"
	>
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules" :disabled="state.formDisabled" cols="2">
			<FaFormItem prop="roleName" label="角色名称">
				<el-input v-model="state.formData.roleName" maxlength="50" placeholder="请输入角色名称" />
			</FaFormItem>
			<FaFormItem prop="roleCode" label="角色编码">
				<el-input v-model="state.formData.roleCode" maxlength="50" placeholder="请输入角色编码" />
			</FaFormItem>
			<FaFormItem v-if="state.dialogState !== 'add'" prop="status" label="状态" span="2">
				<el-radio-group v-model="state.formData.status">
					<el-radio :value="1">正常</el-radio>
					<el-radio :value="2">禁用</el-radio>
				</el-radio-group>
			</FaFormItem>
			<FaFormItem prop="remark" label="备注" span="2">
				<el-input v-model="state.formData.remark" type="textarea" maxlength="200" placeholder="请输入备注" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, type FormRules } from "element-plus";
import { FaDialog } from "fast-element-plus";
import { withDefineType } from "@fast-china/utils";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { roleApi } from "@/api/services/role";

defineOptions({
	name: "SystemRoleEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<any>({
		status: 1,
	}),
	formRules: withDefineType<FormRules>({
		roleName: [{ required: true, message: "请输入角色名称", trigger: "blur" }],
		roleCode: [{ required: true, message: "请输入角色编码", trigger: "blur" }],
		status: [{ required: true, message: "请选择状态", trigger: "change" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "角色",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await roleApi.addRole(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await roleApi.editRole(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (roleId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await roleApi.queryRoleDetail(roleId);
		state.formData = apiRes;
		state.dialogTitle = `角色详情 - ${apiRes.roleName}`;
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加角色";
		state.formDisabled = false;
		state.formData = { status: 1 };
	});
};

const edit = (roleId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await roleApi.queryRoleDetail(roleId);
		state.formData = apiRes;
		state.dialogTitle = `编辑角色 - ${apiRes.roleName}`;
	});
};

defineExpose({
	element: faDialogRef,
	detail,
	add,
	edit,
});
</script>
