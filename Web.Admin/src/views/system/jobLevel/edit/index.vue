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
			<FaFormItem prop="jobLevelName" label="职级名称">
				<el-input v-model="state.formData.jobLevelName" maxlength="50" placeholder="请输入职级名称" />
			</FaFormItem>
			<FaFormItem prop="jobLevelCode" label="职级编码">
				<el-input v-model="state.formData.jobLevelCode" maxlength="50" placeholder="请输入职级编码" />
			</FaFormItem>
			<FaFormItem prop="level" label="等级">
				<el-input-number v-model="state.formData.level" :min="1" placeholder="请输入等级" />
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
import { jobLevelApi } from "@/api/services/jobLevel";

defineOptions({
	name: "SystemJobLevelEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<any>({
		status: 1,
		level: 1,
	}),
	formRules: withDefineType<FormRules>({
		jobLevelName: [{ required: true, message: "请输入职级名称", trigger: "blur" }],
		jobLevelCode: [{ required: true, message: "请输入职级编码", trigger: "blur" }],
		level: [{ required: true, message: "请输入等级", trigger: "blur" }],
		status: [{ required: true, message: "请选择状态", trigger: "change" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "职级",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await jobLevelApi.addJobLevel(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await jobLevelApi.editJobLevel(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (jobLevelId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await jobLevelApi.queryJobLevelDetail(jobLevelId);
		state.formData = apiRes;
		state.dialogTitle = `职级详情 - ${apiRes.jobLevelName}`;
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加职级";
		state.formDisabled = false;
		state.formData = { status: 1, level: 1 };
	});
};

const edit = (jobLevelId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await jobLevelApi.queryJobLevelDetail(jobLevelId);
		state.formData = apiRes;
		state.dialogTitle = `编辑职级 - ${apiRes.jobLevelName}`;
	});
};

defineExpose({
	element: faDialogRef,
	detail,
	add,
	edit,
});
</script>
