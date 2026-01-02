<template>
	<FaDialog
		ref="faDialogRef"
		width="1000"
		fullHeight
		:title="state.dialogTitle"
		:showConfirmButton="!state.formDisabled"
		:showBeforeClose="!state.formDisabled"
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
		@close="faFormRef.resetFields()"
	>
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules" :disabled="state.formDisabled" cols="2">
			<FaFormItem prop="edition" label="版本" span="2">
				<RadioGroup name="EditionEnum" v-model="state.formData.edition" />
			</FaFormItem>
			<FaFormItem prop="appName" label="应用名称">
				<el-input v-model="state.formData.appName" maxlength="30" placeholder="请输入应用名称" />
			</FaFormItem>
			<FaFormItem prop="themeColor" label="主题色">
				<ColorPicker style="width: 32px" v-model="state.formData.themeColor" />
			</FaFormItem>
			<FaFormItem prop="icpSecurityCode" label="ICP备案号">
				<el-input v-model="state.formData.icpSecurityCode" maxlength="20" placeholder="请输入ICP备案号" />
			</FaFormItem>
			<FaFormItem prop="publicSecurityCode" label="公安备案号">
				<el-input v-model="state.formData.publicSecurityCode" maxlength="30" placeholder="请输入公安备案号" />
			</FaFormItem>
			<FaFormItem prop="tenantId" label="租户" span="2">
				<TenantSelectPage v-model="state.formData.tenantId" v-model:tenantName="state.formData.tenantName" />
			</FaFormItem>
			<FaFormItem prop="remark" label="备注">
				<el-input type="textarea" v-model="state.formData.remark" :rows="2" maxlength="200" placeholder="请输入备注" />
			</FaFormItem>
			<FaFormItem prop="logoUrl" label="Logo">
				<FaUploadImage v-model="state.formData.logoUrl" :uploadApi="fileApi.uploadLogo" />
			</FaFormItem>
			<FaFormItem prop="userAgreement" label="用户协议" span="2">
				<Editor v-model="state.formData.userAgreement" placeholder="请输入用户协议" />
			</FaFormItem>
			<FaFormItem prop="privacyAgreement" label="隐私协议" span="2">
				<Editor v-model="state.formData.privacyAgreement" placeholder="请输入隐私协议" />
			</FaFormItem>
			<FaFormItem prop="serviceAgreement" label="服务协议" span="2">
				<Editor v-model="state.formData.serviceAgreement" placeholder="请输入服务协议" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, type FormRules } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { EditApplicationInput } from "@/api/services/application/models/EditApplicationInput";
import { AddApplicationInput } from "@/api/services/application/models/AddApplicationInput";
import { applicationApi } from "@/api/services/application";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { fileApi } from "@/api/services/file";

defineOptions({
	name: "SystemApplicationEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditApplicationInput & AddApplicationInput>({
		edition: EditionEnum.None,
		userAgreement: "<p><br></p>",
		privacyAgreement: "<p><br></p>",
		serviceAgreement: "<p><br></p>",
	}),
	formRules: withDefineType<FormRules>({
		edition: [{ required: true, message: "请选择版本", trigger: "change" }],
		appName: [{ required: true, message: "请输入应用名称", trigger: "blur" }],
		themeColor: [{ required: true, message: "请选择主题色", trigger: "blur" }],
		logoUrl: [{ required: true, message: "请上传Logo", trigger: "change" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
	dialogTitle: "应用",
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await applicationApi.addApplication(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await applicationApi.editApplication(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (appId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await applicationApi.queryApplicationDetail(appId);
		state.formData = apiRes;
		state.dialogTitle = `应用详情 - ${apiRes.appName}`;
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加应用";
		state.formDisabled = false;
	});
};

const edit = (appId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await applicationApi.queryApplicationDetail(appId);
		state.formData = apiRes;
		state.dialogTitle = `编辑应用 - ${apiRes.appName}`;
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
