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
			<FaFormItem prop="employeeName" label="职员姓名">
				<el-input v-model="state.formData.employeeName" maxlength="50" placeholder="请输入职员姓名" />
			</FaFormItem>
			<FaFormItem prop="employeeNo" label="工号">
				<el-input v-model="state.formData.employeeNo" maxlength="50" placeholder="请输入工号" />
			</FaFormItem>
			<FaFormItem prop="phone" label="手机号">
				<el-input v-model="state.formData.phone" maxlength="20" placeholder="请输入手机号" />
			</FaFormItem>
			<FaFormItem prop="email" label="邮箱">
				<el-input v-model="state.formData.email" maxlength="100" placeholder="请输入邮箱" />
			</FaFormItem>
			<FaFormItem prop="gender" label="性别">
				<el-radio-group v-model="state.formData.gender">
					<el-radio :value="1">男</el-radio>
					<el-radio :value="2">女</el-radio>
				</el-radio-group>
			</FaFormItem>
			<FaFormItem prop="birthday" label="出生日期">
				<el-date-picker
					v-model="state.formData.birthday"
					type="date"
					placeholder="请选择出生日期"
					style="width: 100%"
				/>
			</FaFormItem>
			<FaFormItem prop="address" label="地址" span="2">
				<el-input v-model="state.formData.address" maxlength="200" placeholder="请输入地址" />
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
import { employeeApi } from "@/api/services/employee";

defineOptions({
	name: "SystemEmployeeEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<any>({
		status: 1,
		gender: 1,
	}),
	formRules: withDefineType<FormRules>({
		employeeName: [{ required: true, message: "请输入职员姓名", trigger: "blur" }],
		employeeNo: [{ required: true, message: "请输入工号", trigger: "blur" }],
		phone: [{ required: true, message: "请输入手机号", trigger: "blur" }],
		status: [{ required: true, message: "请选择状态", trigger: "change" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "职员",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await employeeApi.addEmployee(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await employeeApi.editEmployee(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (employeeId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await employeeApi.queryEmployeeDetail(employeeId);
		state.formData = apiRes;
		state.dialogTitle = `职员详情 - ${apiRes.employeeName}`;
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加职员";
		state.formDisabled = false;
		state.formData = { status: 1, gender: 1 };
	});
};

const edit = (employeeId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await employeeApi.queryEmployeeDetail(employeeId);
		state.formData = apiRes;
		state.dialogTitle = `编辑职员 - ${apiRes.employeeName}`;
	});
};

defineExpose({
	element: faDialogRef,
	detail,
	add,
	edit,
});
</script>
