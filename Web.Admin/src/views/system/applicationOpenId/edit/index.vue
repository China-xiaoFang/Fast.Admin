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
			<FaFormItem prop="appId" label="应用">
				<ApplicationSelect v-model="state.formData.appId" v-model:appName="state.formData.appName" />
			</FaFormItem>
			<FaFormItem prop="openId" label="应用标识">
				<el-input v-model="state.formData.openId" maxlength="50" placeholder="请输入应用标识" />
			</FaFormItem>
			<FaFormItem prop="appType" label="应用类型">
				<FaSelect :data="appEnvironmentEnum" v-model="state.formData.appType" />
			</FaFormItem>
			<FaFormItem prop="openSecret" label="开放平台密钥">
				<el-input v-model="state.formData.openSecret" maxlength="50" placeholder="请输入开放平台密钥" />
			</FaFormItem>
			<FaFormItem prop="environmentType" label="环境类型" span="2">
				<RadioGroup name="EnvironmentTypeEnum" v-model="state.formData.environmentType" />
			</FaFormItem>
			<FaFormItem prop="loginComponent" label="登录组件">
				<el-select v-model="state.formData.loginComponent" placeholder="请选择登录组件" clearable>
					<el-option value="ClassicLogin" label="ClassicLogin" />
				</el-select>
			</FaFormItem>
			<FaFormItem prop="webSocketUrl" label="WebSocket地址">
				<el-input v-model="state.formData.webSocketUrl" maxlength="50" placeholder="请输入WebSocket地址" />
			</FaFormItem>
			<FaFormItem prop="requestEncipher" label="请求加密">
				<el-checkbox v-model="state.formData.requestEncipher">加密</el-checkbox>
			</FaFormItem>
			<FaFormItem prop="requestTimeout" label="请求超时时间">
				<el-input-number v-model="state.formData.requestTimeout" :min="5000" :max="60000" placeholder="请输入请求超时时间">
					<template #suffix>毫秒</template>
				</el-input-number>
			</FaFormItem>
			<FaFormItem prop="remark" label="备注">
				<el-input type="textarea" v-model="state.formData.remark" :rows="2" maxlength="200" placeholder="请输入备注" />
			</FaFormItem>

			<FaLayoutGridItem span="2">
				<el-divider contentPosition="left">支付相关</el-divider>
			</FaLayoutGridItem>
			<FaFormItem prop="weChatMerchantId" label="微信商户号">
				<MerchantSelect
					:merchantType="PaymentChannelEnum.WeChat"
					v-model="state.formData.weChatMerchantId"
					v-model:merchantNo="state.formData.weChatMerchantNo"
					maxlength="20"
					placeholder="请选择微信商户号"
				/>
			</FaFormItem>
			<FaFormItem prop="alipayMerchantId" label="支付宝商户号">
				<MerchantSelect
					:merchantType="PaymentChannelEnum.Alipay"
					v-model="state.formData.alipayMerchantId"
					v-model:merchantNo="state.formData.alipayMerchantNo"
					maxlength="20"
					placeholder="请选择支付宝商户号"
				/>
			</FaFormItem>

			<FaLayoutGridItem span="2">
				<el-divider contentPosition="left">联系信息</el-divider>
			</FaLayoutGridItem>
			<FaFormItem prop="contactPhone" label="联系电话">
				<el-input v-model="state.formData.contactPhone" maxlength="20" placeholder="请输入联系电话" />
			</FaFormItem>
			<FaFormItem prop="address" label="地址">
				<el-input type="textarea" v-model="state.formData.address" :rows="2" maxlength="200" placeholder="请输入地址" />
			</FaFormItem>
			<FaFormItem prop="latitude" label="纬度">
				<el-input v-model="state.formData.latitude" maxlength="20" placeholder="请输入纬度" />
			</FaFormItem>
			<FaFormItem prop="longitude" label="经度">
				<el-input v-model="state.formData.longitude" maxlength="20" placeholder="请输入经度" />
			</FaFormItem>

			<FaLayoutGridItem span="2">
				<el-divider contentPosition="left">图片相关</el-divider>
			</FaLayoutGridItem>
			<FaFormItem prop="statusBarImageUrl" label="状态栏图片">
				<FaUploadImage v-model="state.formData.statusBarImageUrl" :uploadApi="fileApi.uploadFile" />
			</FaFormItem>
			<FaFormItem prop="bannerImages" label="Banner图" span="2">
				<FaUploadImages v-model="state.formData.bannerImages" :uploadApi="fileApi.uploadFile" />
			</FaFormItem>

			<template v-if="state.dialogState !== 'add'">
				<FaLayoutGridItem span="2">
					<el-divider contentPosition="left">模板Id</el-divider>
				</FaLayoutGridItem>
				<FaLayoutGridItem span="2" style="min-height: 300px; max-height: 500px">
					<FaTable rowKey="buttonId" :data="state.formData.templateIdList">
						<!-- 表格按钮操作区域 -->
						<template #header>
							<el-button type="primary" :icon="Plus" @click="handleTemplateIdAdd">新增</el-button>
						</template>
						<FaTableColumn prop="templateId" label="模板Id" width="300">
							<template #default="{ row, $index }: { row: EditApplicationTemplateIdInput; $index: number }">
								<el-form-item
									:prop="`templateIdList.${$index}.templateId`"
									:rules="[{ required: true, message: '请输入模板Id', trigger: 'blur' }]"
								>
									<el-input v-model="row.templateId" maxlength="50" placeholder="请输入模板Id" />
								</el-form-item>
							</template>
						</FaTableColumn>
						<FaTableColumn prop="templateType" label="模板类型" width="280">
							<template #default="{ row, $index }: { row: EditApplicationTemplateIdInput; $index: number }">
								<el-form-item
									:prop="`templateIdList.${$index}.templateType`"
									:rules="[{ required: true, message: '请选择模板类型', trigger: 'change' }]"
								>
									<FaSelect :data="applicationTemplateTypeEnum" v-model="row.templateType" />
								</el-form-item>
							</template>
						</FaTableColumn>
						<!-- 表格操作 -->
						<template #operation="{ $index }: { $index: number }">
							<el-button size="small" plain type="danger" @click="handleTemplateIdDelete($index)">删除</el-button>
						</template>
					</FaTable>
				</FaLayoutGridItem>
			</template>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, type FormRules } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";
import { fileApi } from "@/api/services/file";
import { EditApplicationOpenIdInput } from "@/api/services/applicationOpenId/models/EditApplicationOpenIdInput";
import { AddApplicationOpenIdInput } from "@/api/services/applicationOpenId/models/AddApplicationOpenIdInput";
import { applicationOpenIdApi } from "@/api/services/applicationOpenId";
import { useApp } from "@/stores";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";
import { PaymentChannelEnum } from "@/api/enums/PaymentChannelEnum";
import { Plus } from "@element-plus/icons-vue";
import { EditApplicationTemplateIdInput } from "@/api/services/applicationOpenId/models/EditApplicationTemplateIdInput";

defineOptions({
	name: "SystemApplicationOpenIdEdit",
});

const emit = defineEmits(["ok"]);

const appStore = useApp();
const appEnvironmentEnum = appStore.getDictionary("AppEnvironmentEnum");
const applicationTemplateTypeEnum = appStore.getDictionary("ApplicationTemplateTypeEnum");

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditApplicationOpenIdInput & AddApplicationOpenIdInput & { appName?: string }>({
		appType: AppEnvironmentEnum.Web,
		environmentType: EnvironmentTypeEnum.Production,
		requestTimeout: 60000,
		requestEncipher: true,
		templateIdList: [],
	}),
	formRules: withDefineType<FormRules>({
		appId: [{ required: true, message: "请选择应用", trigger: "change" }],
		openId: [{ required: true, message: "请输入应用标识", trigger: "blur" }],
		requestTimeout: [{ required: true, message: "请输入请求超时时间", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
	dialogTitle: "应用OpenId",
});

const handleTemplateIdAdd = () => {
	state.formData.templateIdList.push({});
};

const handleTemplateIdDelete = (index: number) => {
	state.formData.templateIdList.splice(index, 1);
};

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await applicationOpenIdApi.addApplicationOpenId(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await applicationOpenIdApi.editApplicationOpenId(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const detail = (recordId: number) => {
	faDialogRef.value.open(async () => {
		state.formDisabled = true;
		const apiRes = await applicationOpenIdApi.queryApplicationOpenIdDetail(recordId);
		state.formData = apiRes;
		state.dialogTitle = `应用OpenId详情 - ${apiRes.appName}`;
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加应用OpenId";
		state.formDisabled = false;
	});
};

const edit = (recordId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await applicationOpenIdApi.queryApplicationOpenIdDetail(recordId);
		state.formData = apiRes;
		state.dialogTitle = `编辑应用OpenId - ${apiRes.openId}`;
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
