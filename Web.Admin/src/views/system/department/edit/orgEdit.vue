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
			<FaFormItem prop="parentId" label="父级">
				<FaTreeSelect
					:requestApi="organizationApi.organizationSelector"
					v-model="state.formData.parentId"
					v-model:label="state.formData.parentName"
					placeholder="请选择父级机构"
					check-strictly
					filterable
					clearable
				/>
			</FaFormItem>
			<FaFormItem prop="orgName" label="机构名称">
				<el-input v-model="state.formData.orgName" maxlength="20" placeholder="请输入机构名称" />
			</FaFormItem>
			<FaFormItem prop="orgCode" label="机构编码">
				<el-input v-model="state.formData.orgCode" maxlength="30" placeholder="请输入机构编码" />
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
import { organizationApi } from "@/api/services/organization";
import { EditOrganizationInput } from "@/api/services/organization/models/EditOrganizationInput";
import { AddOrganizationInput } from "@/api/services/organization/models/AddOrganizationInput";

defineOptions({
	name: "SystemOrgEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditOrganizationInput & AddOrganizationInput & { parentName?: string }>({}),
	formRules: withDefineType<FormRules>({
		orgName: [{ required: true, message: "请输入机构名称", trigger: "blur" }],
		orgCode: [{ required: true, message: "请输入机构编码", trigger: "blur" }],
		sort: [{ required: true, message: "请输入排序", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
	dialogTitle: "机构",
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

const detail = (orgId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await organizationApi.queryOrganizationDetail(orgId);
		if (apiRes.parentId == 0) {
			apiRes.parentId = undefined;
		}
		state.formData = apiRes;
		state.dialogTitle = `机构详情 - ${apiRes.orgName}`;
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加机构";
		state.formDisabled = false;
	});
};

const edit = (orgId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await organizationApi.queryOrganizationDetail(orgId);
		if (apiRes.parentId == 0) {
			apiRes.parentId = undefined;
		}
		state.formData = apiRes;
		state.dialogTitle = `编辑机构 - ${apiRes.orgName}`;
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
