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
			<FaFormItem prop="databaseName" label="数据库名称">
				<el-input v-model="state.formData.databaseName" maxlength="100" placeholder="请输入数据库名称" />
			</FaFormItem>
			<FaFormItem prop="dbType" label="数据库类型">
				<el-select v-model="state.formData.dbType" placeholder="请选择数据库类型" style="width: 100%">
					<el-option label="MySQL" :value="0" />
					<el-option label="SqlServer" :value="1" />
					<el-option label="PostgreSQL" :value="2" />
					<el-option label="Oracle" :value="3" />
				</el-select>
			</FaFormItem>
			<FaFormItem prop="host" label="主机地址">
				<el-input v-model="state.formData.host" maxlength="200" placeholder="请输入主机地址" />
			</FaFormItem>
			<FaFormItem prop="port" label="端口">
				<el-input-number v-model="state.formData.port" :min="1" :max="65535" placeholder="请输入端口" style="width: 100%" />
			</FaFormItem>
			<FaFormItem prop="userName" label="用户名">
				<el-input v-model="state.formData.userName" maxlength="100" placeholder="请输入用户名" />
			</FaFormItem>
			<FaFormItem prop="password" label="密码">
				<el-input v-model="state.formData.password" type="password" maxlength="100" placeholder="请输入密码" showPassword />
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
import type { AddMainDatabaseInput } from "@/api/services/database/models/AddMainDatabaseInput";
import type { EditMainDatabaseInput } from "@/api/services/database/models/EditMainDatabaseInput";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { databaseApi } from "@/api/services/database";

defineOptions({
	name: "DevMainDatabaseEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditMainDatabaseInput & AddMainDatabaseInput>({}),
	formRules: withDefineType<FormRules>({
		databaseName: [{ required: true, message: "请输入数据库名称", trigger: "blur" }],
		dbType: [{ required: true, message: "请选择数据库类型", trigger: "change" }],
		host: [{ required: true, message: "请输入主机地址", trigger: "blur" }],
		port: [{ required: true, message: "请输入端口", trigger: "blur" }],
		userName: [{ required: true, message: "请输入用户名", trigger: "blur" }],
		password: [{ required: true, message: "请输入密码", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "主库模板",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await databaseApi.addMainDatabase(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await databaseApi.editMainDatabase(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (mainDatabaseId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await databaseApi.queryMainDatabaseDetail(mainDatabaseId);
		state.formData = apiRes;
		state.dialogState = "detail";
		state.dialogTitle = "主库模板详情";
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.formDisabled = false;
		state.formData = {};
		state.dialogState = "add";
		state.dialogTitle = "新增主库模板";
	});
};

const edit = (mainDatabaseId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = false;
		const apiRes = await databaseApi.queryMainDatabaseDetail(mainDatabaseId);
		state.formData = apiRes;
		state.dialogState = "edit";
		state.dialogTitle = "编辑主库模板";
	});
};

defineExpose({ detail, add, edit });
</script>
