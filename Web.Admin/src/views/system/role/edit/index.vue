<template>
	<FaDialog
		ref="faDialogRef"
		width="500"
		:title="state.dialogTitle"
		:showConfirmButton="!state.formDisabled"
		:showBeforeClose="!state.formDisabled"
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
		@close="faFormRef.resetFields()"
	>
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules" :disabled="state.formDisabled">
			<FaFormItem prop="roleName" label="角色名称">
				<el-input v-model="state.formData.roleName" maxlength="20" placeholder="请输入角色名称" />
			</FaFormItem>
			<FaFormItem prop="roleCode" label="角色编码">
				<el-input v-model="state.formData.roleCode" maxlength="30" placeholder="请输入角色编码" />
			</FaFormItem>
			<FaFormItem prop="sort" label="排序" tips="从小到大">
				<el-input-number v-model="state.formData.sort" :min="1" :max="9999" placeholder="请输入排序" />
			</FaFormItem>
			<FaFormItem prop="dataScopeType" label="数据范围">
				<RadioGroup name="DataScopeTypeEnum" v-model="state.formData.dataScopeType" />
			</FaFormItem>
			<FaFormItem prop="remark" label="备注">
				<el-input type="textarea" v-model="state.formData.remark" :rows="2" maxlength="200" placeholder="请输入备注" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, type FormRules } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import { DataScopeTypeEnum } from "@/api/enums/DataScopeTypeEnum";
import { roleApi } from "@/api/services/Admin/role";
import { AddRoleInput } from "@/api/services/Admin/role/models/AddRoleInput";
import { EditRoleInput } from "@/api/services/Admin/role/models/EditRoleInput";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";

defineOptions({
	name: "SystemRoleEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditRoleInput & AddRoleInput>({}),
	formRules: withDefineType<FormRules>({
		roleName: [{ required: true, message: "请输入角色名称", trigger: "blur" }],
		roleCode: [{ required: true, message: "请输入角色编码", trigger: "blur" }],
		sort: [{ required: true, message: "请输入排序", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
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
		state.formData = {
			dataScopeType: DataScopeTypeEnum.All,
			sort: 1,
		};
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

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	detail,
	add,
	edit,
});
</script>
