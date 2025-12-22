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
			<FaFormItem prop="parentId" label="父组织">
				<el-tree-select
					v-model="state.formData.parentId"
					:data="state.organizationTree"
					:props="{ label: 'organizationName' }"
					node-key="organizationId"
					value-key="organizationId"
					check-strictly
					placeholder="请选择父组织"
				/>
			</FaFormItem>
			<FaFormItem prop="organizationName" label="组织名称">
				<el-input v-model="state.formData.organizationName" maxlength="50" placeholder="请输入组织名称" />
			</FaFormItem>
			<FaFormItem prop="organizationCode" label="组织编码">
				<el-input v-model="state.formData.organizationCode" maxlength="50" placeholder="请输入组织编码" />
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
			<FaFormItem v-if="state.dialogState !== 'add'" prop="status" label="状态">
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
import { organizationApi } from "@/api/services/organization";

defineOptions({
	name: "SystemOrganizationEdit",
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
		organizationName: [{ required: true, message: "请输入组织名称", trigger: "blur" }],
		organizationCode: [{ required: true, message: "请输入组织编码", trigger: "blur" }],
		status: [{ required: true, message: "请选择状态", trigger: "change" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "组织机构",
	organizationTree: [] as any[],
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await organizationApi.addOrganization(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await organizationApi.editOrganization(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const loadOrganizationTree = async () => {
	try {
		const result = await organizationApi.queryOrganizationPaged({ pageIndex: 1, pageSize: 1000 });
		state.organizationTree = result.rows || [];
	} catch {
		state.organizationTree = [];
	}
};

const detail = (organizationId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		await loadOrganizationTree();
		const apiRes = await organizationApi.queryOrganizationDetail(organizationId);
		state.formData = apiRes;
		state.dialogTitle = `组织机构详情 - ${apiRes.organizationName}`;
	});
};

const add = () => {
	faDialogRef.value.open(async () => {
		state.dialogState = "add";
		state.dialogTitle = "添加组织机构";
		state.formDisabled = false;
		state.formData = { status: 1, sort: 0 };
		await loadOrganizationTree();
	});
};

const edit = (organizationId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		await loadOrganizationTree();
		const apiRes = await organizationApi.queryOrganizationDetail(organizationId);
		state.formData = apiRes;
		state.dialogTitle = `编辑组织机构 - ${apiRes.organizationName}`;
	});
};

defineExpose({
	element: faDialogRef,
	detail,
	add,
	edit,
});
</script>
