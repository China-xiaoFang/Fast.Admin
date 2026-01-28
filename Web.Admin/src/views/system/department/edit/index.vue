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
			<FaFormItem prop="orgId" label="机构">
				<FaTreeSelect
					:requestApi="organizationApi.organizationSelector"
					v-model="state.formData.orgId"
					v-model:label="state.formData.orgName"
					placeholder="请选择机构"
					check-strictly
					filterable
					clearable
				/>
			</FaFormItem>
			<FaFormItem prop="parentId" label="父级">
				<FaTreeSelect
					:requestApi="departmentApi.departmentSelector"
					:initParam="state.formData.orgId"
					v-model="state.formData.parentId"
					v-model:label="state.formData.parentName"
					placeholder="请选择父级部门"
					check-strictly
					filterable
					clearable
				/>
			</FaFormItem>
			<FaFormItem prop="departmentName" label="部门名称">
				<el-input v-model="state.formData.departmentName" maxlength="20" placeholder="请输入部门名称" />
			</FaFormItem>
			<FaFormItem prop="departmentCode" label="部门编码">
				<el-input v-model="state.formData.departmentCode" maxlength="30" placeholder="请输入部门编码" />
			</FaFormItem>
			<FaFormItem prop="sort" label="排序" tips="从小到大">
				<el-input-number v-model="state.formData.sort" :min="1" :max="9999" placeholder="请输入排序" />
			</FaFormItem>
			<FaFormItem prop="contacts" label="联系人">
				<el-input v-model="state.formData.contacts" maxlength="20" placeholder="请输入联系人" />
			</FaFormItem>
			<FaFormItem prop="phone" label="电话">
				<el-input v-model="state.formData.phone" maxlength="20" placeholder="请输入电话" />
			</FaFormItem>
			<FaFormItem prop="email" label="邮箱">
				<el-input v-model="state.formData.email" maxlength="50" placeholder="请输入邮箱" />
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
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { EditDepartmentInput } from "@/api/services/Admin/department/models/EditDepartmentInput";
import { AddDepartmentInput } from "@/api/services/Admin/department/models/AddDepartmentInput";
import { departmentApi } from "@/api/services/Admin/department";
import { organizationApi } from "@/api/services/Admin/organization";

defineOptions({
	name: "SystemDepartmentEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditDepartmentInput & AddDepartmentInput & { orgName?: string; parentName?: string }>({}),
	formRules: withDefineType<FormRules>({
		orgId: [{ required: true, message: "请选择机构", trigger: "change" }],
		departmentName: [{ required: true, message: "请输入部门名称", trigger: "blur" }],
		departmentCode: [{ required: true, message: "请输入部门编码", trigger: "blur" }],
		sort: [{ required: true, message: "请输入排序", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
	dialogTitle: "部门",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await departmentApi.addDepartment(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await departmentApi.editDepartment(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (departmentId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await departmentApi.queryDepartmentDetail(departmentId);
		if (apiRes.parentId == 0) {
			apiRes.parentId = undefined;
		}
		state.formData = apiRes;
		state.dialogTitle = `部门详情 - ${apiRes.departmentName}`;
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加部门";
		state.formDisabled = false;
		state.formData = {};
	});
};

const edit = (departmentId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await departmentApi.queryDepartmentDetail(departmentId);
		if (apiRes.parentId == 0) {
			apiRes.parentId = undefined;
		}
		state.formData = apiRes;
		state.dialogTitle = `编辑部门 - ${apiRes.departmentName}`;
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
