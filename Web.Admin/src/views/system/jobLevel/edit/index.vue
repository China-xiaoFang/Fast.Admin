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
			<FaFormItem prop="jobLevelName" label="职级名称">
				<el-input v-model="state.formData.jobLevelName" maxlength="20" placeholder="请输入职级名称" />
			</FaFormItem>
			<FaFormItem prop="level" label="职级等级" tips="T:技术职级; M:管理职级; C:综合职级; 数字越大 职级越高">
				<el-input v-model="state.formData.level" maxlength="5" placeholder="请输入职级等级" />
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
import { EditJobLevelInput } from "@/api/services/jobLevel/models/EditJobLevelInput";
import { AddJobLevelInput } from "@/api/services/jobLevel/models/AddJobLevelInput";
import { jobLevelApi } from "@/api/services/jobLevel";

defineOptions({
	name: "SystemJobLevelEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditJobLevelInput & AddJobLevelInput>({}),
	formRules: withDefineType<FormRules>({
		jobLevelName: [{ required: true, message: "请输入职级名称", trigger: "blur" }],
		level: [{ required: true, message: "请输入职级等级", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
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
		state.formData = {};
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

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	detail,
	add,
	edit,
});
</script>
