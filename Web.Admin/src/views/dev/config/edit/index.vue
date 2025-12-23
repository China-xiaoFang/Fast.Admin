<template>
	<FaDialog
		ref="faDialogRef"
		width="600"
		:title="state.dialogTitle"
		:showConfirmButton="!state.formDisabled"
		:showBeforeClose="!state.formDisabled"
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
		@close="faFormRef.resetFields()"
	>
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules" :disabled="state.formDisabled">
			<FaFormItem prop="configCode" label="配置编码">
				<el-input v-model="state.formData.configCode" maxlength="100" placeholder="请输入配置编码" />
			</FaFormItem>
			<FaFormItem prop="configName" label="配置名称">
				<el-input v-model="state.formData.configName" maxlength="100" placeholder="请输入配置名称" />
			</FaFormItem>
			<FaFormItem prop="configValue" label="配置值">
				<el-input v-model="state.formData.configValue" type="textarea" :rows="4" maxlength="500" placeholder="请输入配置值" />
			</FaFormItem>
			<FaFormItem prop="remark" label="备注">
				<el-input v-model="state.formData.remark" type="textarea" :rows="3" maxlength="200" placeholder="请输入备注" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, type FormRules } from "element-plus";
import { FaDialog } from "fast-element-plus";
import { withDefineType } from "@fast-china/utils";
import type { AddConfigInput } from "@/api/services/config/models/AddConfigInput";
import type { EditConfigInput } from "@/api/services/config/models/EditConfigInput";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { configApi } from "@/api/services/config";

defineOptions({
	name: "DevConfigEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditConfigInput & AddConfigInput>({}),
	formRules: withDefineType<FormRules>({
		configCode: [{ required: true, message: "请输入配置编码", trigger: "blur" }],
		configName: [{ required: true, message: "请输入配置名称", trigger: "blur" }],
		configValue: [{ required: true, message: "请输入配置值", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "配置",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await configApi.addConfig(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await configApi.editConfig(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (configId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await configApi.queryConfigDetail(configId);
		state.formData = apiRes;
		state.dialogState = "detail";
		state.dialogTitle = "配置详情";
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.formDisabled = false;
		state.formData = {};
		state.dialogState = "add";
		state.dialogTitle = "新增配置";
	});
};

const edit = (configId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = false;
		const apiRes = await configApi.queryConfigDetail(configId);
		state.formData = apiRes;
		state.dialogState = "edit";
		state.dialogTitle = "编辑配置";
	});
};

defineExpose({ detail, add, edit });
</script>
