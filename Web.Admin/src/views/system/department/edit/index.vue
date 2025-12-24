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
			<FaFormItem prop="parentId" label="父部门">
				<el-tree-select
					v-model="state.formData.parentId"
					:data="state.departmentTree"
					:props="{ label: 'departmentName' }"
					node-key="departmentId"
					value-key="departmentId"
					check-strictly
					placeholder="请选择父部门"
				/>
			</FaFormItem>
			<FaFormItem prop="organizationId" label="所属组织">
				<el-select v-model="state.formData.organizationId" placeholder="请选择所属组织">
					<el-option
						v-for="item in state.organizationList"
						:key="item.value"
						:label="item.label"
						:value="item.value"
					/>
				</el-select>
			</FaFormItem>
			<FaFormItem prop="departmentName" label="部门名称">
				<el-input v-model="state.formData.departmentName" maxlength="50" placeholder="请输入部门名称" />
			</FaFormItem>
			<FaFormItem prop="departmentCode" label="部门编码">
				<el-input v-model="state.formData.departmentCode" maxlength="50" placeholder="请输入部门编码" />
			</FaFormItem>
			<FaFormItem prop="leader" label="负责人">
				<el-input v-model="state.formData.leader" maxlength="50" placeholder="请输入负责人" />
			</FaFormItem>
			<FaFormItem prop="phone" label="联系电话">
				<el-input v-model="state.formData.phone" maxlength="20" placeholder="请输入联系电话" />
			</FaFormItem>
			<FaFormItem prop="email" label="邮箱">
				<el-input v-model="state.formData.email" maxlength="100" placeholder="请输入邮箱" />
			</FaFormItem>
			<FaFormItem prop="sort" label="排序">
				<el-input-number v-model="state.formData.sort" :min="0" placeholder="请输入排序" />
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
import { departmentApi } from "@/api/services/department";
import { organizationApi } from "@/api/services/organization";

defineOptions({
	name: "SystemDepartmentEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<any>({
		status: 1,
		sort: 0,
	}),
	formRules: withDefineType<FormRules>({
		departmentName: [{ required: true, message: "请输入部门名称", trigger: "blur" }],
		departmentCode: [{ required: true, message: "请输入部门编码", trigger: "blur" }],
		status: [{ required: true, message: "请选择状态", trigger: "change" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "部门",
	departmentTree: [] as any[],
	organizationList: [] as any[],
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

const loadDepartmentTree = async () => {
	try {
		const result = await departmentApi.queryDepartmentPaged({ pageIndex: 1, pageSize: 1000 });
		state.departmentTree = result.rows || [];
	} catch {
		state.departmentTree = [];
	}
};

const loadOrganizationList = async () => {
	try {
		state.organizationList = await organizationApi.organizationSelector();
	} catch {
		state.organizationList = [];
	}
};

const detail = (departmentId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		await Promise.all([loadDepartmentTree(), loadOrganizationList()]);
		const apiRes = await departmentApi.queryDepartmentDetail(departmentId);
		state.formData = apiRes;
		state.dialogTitle = `部门详情 - ${apiRes.departmentName}`;
	});
};

const add = () => {
	faDialogRef.value.open(async () => {
		state.dialogState = "add";
		state.dialogTitle = "添加部门";
		state.formDisabled = false;
		state.formData = { status: 1, sort: 0 };
		await Promise.all([loadDepartmentTree(), loadOrganizationList()]);
	});
};

const edit = (departmentId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		await Promise.all([loadDepartmentTree(), loadOrganizationList()]);
		const apiRes = await departmentApi.queryDepartmentDetail(departmentId);
		state.formData = apiRes;
		state.dialogTitle = `编辑部门 - ${apiRes.departmentName}`;
	});
};

defineExpose({
	element: faDialogRef,
	detail,
	add,
	edit,
});
</script>
