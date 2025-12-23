<template>
	<FaDialog
		ref="faDialogRef"
		width="600"
		:title="state.dialogTitle"
		:showConfirmButton="!state.formDisabled"
		:showBeforeClose="!state.formDisabled"
		confirmButtonText="提交"
		@confirm-click="handleConfirm"
		@close="faFormRef.resetFields()"
	>
		<div v-if="state.formDisabled">
			<el-descriptions :column="1" border>
				<el-descriptions-item label="投诉人">{{ state.complaintData.nickName }}</el-descriptions-item>
				<el-descriptions-item label="联系电话">{{ state.complaintData.contactPhone }}</el-descriptions-item>
				<el-descriptions-item label="联系邮箱">{{ state.complaintData.contactEmail }}</el-descriptions-item>
				<el-descriptions-item label="投诉描述">{{ state.complaintData.description }}</el-descriptions-item>
				<el-descriptions-item label="投诉时间">{{ state.complaintData.createdTime }}</el-descriptions-item>
				<el-descriptions-item v-if="state.complaintData.handleTime" label="处理时间">
					{{ state.complaintData.handleTime }}
				</el-descriptions-item>
				<el-descriptions-item v-if="state.complaintData.handleDescription" label="处理说明">
					{{ state.complaintData.handleDescription }}
				</el-descriptions-item>
			</el-descriptions>
		</div>
		<FaForm v-else ref="faFormRef" :model="state.formData" :rules="state.formRules">
			<FaFormItem prop="handleDescription" label="处理说明">
				<el-input v-model="state.formData.handleDescription" type="textarea" :rows="5" maxlength="500" placeholder="请输入处理说明" />
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
import type { HandleComplaintInput } from "@/api/services/complaint/models/HandleComplaintInput";
import type { QueryComplaintPagedOutput } from "@/api/services/complaint/models/QueryComplaintPagedOutput";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { complaintApi } from "@/api/services/complaint";

defineOptions({
	name: "BusinessComplaintHandle",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<HandleComplaintInput>({}),
	complaintData: withDefineType<QueryComplaintPagedOutput>({}),
	formRules: withDefineType<FormRules>({
		handleDescription: [{ required: true, message: "请输入处理说明", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogTitle: "投诉详情",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		await complaintApi.handleComplaint(state.formData);
		ElMessage.success("处理成功！");
		emit("ok");
	});
};

const detail = (row: QueryComplaintPagedOutput) => {
	faDialogRef.value.open(() => {
		state.formDisabled = true;
		state.complaintData = row;
		state.dialogTitle = "投诉详情";
	});
};

const handle = (row: QueryComplaintPagedOutput) => {
	faDialogRef.value.open(() => {
		state.formDisabled = false;
		state.formData = {
			complaintId: row.complaintId,
			rowVersion: row.rowVersion,
		};
		state.dialogTitle = "处理投诉";
	});
};

defineExpose({ detail, handle });
</script>
