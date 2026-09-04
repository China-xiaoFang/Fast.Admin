<template>
	<div class="el-card" v-loading="state.loading" element-loading-text="加载中...">
		<el-scrollbar>
			<el-divider content-position="left">账号信息</el-divider>
			<FaForm ref="accountFaFormRef" :model="state.accountFormData" :rules="state.formRules" cols="3">
				<FaFormItem prop="nickName" label="昵称">
					<el-input v-model="state.accountFormData.nickName" maxlength="20" placeholder="请输入昵称" />
				</FaFormItem>
				<FaFormItem prop="mobile" label="手机">
					<el-input v-model="state.accountFormData.mobile" maxlength="11" placeholder="请输入手机" />
				</FaFormItem>
				<FaFormItem prop="email" label="邮箱">
					<el-input v-model="state.accountFormData.email" maxlength="50" placeholder="请输入邮箱" />
				</FaFormItem>
				<FaFormItem prop="lastLoginIp" label="Ip">
					<el-text type="success">{{ state.accountFormData.lastLoginIp }}</el-text>
				</FaFormItem>
				<FaFormItem prop="lastLoginTime" label="时间">
					<template v-if="state.accountFormData.lastLoginTime">
						{{ dayjs(state.accountFormData.lastLoginTime).format("YYYY-MM-DD HH:mm:ss") }}
					</template>
					<template v-else>-</template>
				</FaFormItem>
				<FaFormItem prop="avatar" label="头像">
					<FaUploadImage v-model="state.accountFormData.avatar" :upload-api="fileApi.uploadAvatar" />
				</FaFormItem>
			</FaForm>

			<template v-if="!userInfoStore.isSuperAdmin && !userInfoStore.isAdmin">
				<el-divider content-position="left">职员信息</el-divider>
				<FaForm ref="employeeFaFormRef" :model="state.employeeFormData" :rules="state.formRules" cols="3">
					<FaFormItem prop="employeeName" label="职员名称">
						<el-input v-model="state.employeeFormData.employeeName" maxlength="20" placeholder="请输入职员名称" />
					</FaFormItem>
					<FaFormItem prop="mobile" label="手机">
						<el-input v-model="state.employeeFormData.mobile" maxlength="11" placeholder="请输入手机" />
					</FaFormItem>
					<FaFormItem prop="email" label="邮箱">
						<el-input v-model="state.employeeFormData.email" maxlength="50" placeholder="请输入邮箱" />
					</FaFormItem>
					<FaFormItem prop="sex" label="性别">
						<RadioGroup name="GenderEnum" v-model="state.employeeFormData.sex" />
					</FaFormItem>
					<FaFormItem prop="idPhoto" label="证件照">
						<FaUploadImage v-model="state.employeeFormData.idPhoto" :upload-api="fileApi.uploadIdPhoto" />
					</FaFormItem>

					<FaLayoutGridItem span="3">
						<el-divider content-position="left">机构信息</el-divider>
					</FaLayoutGridItem>

					<FaLayoutGridItem span="3" style="min-height: 300px; max-height: 500px">
						<FaTable :data="state.employeeFormData.orgList" :pagination="false" :header-card="false">
							<FaTableColumn prop="orgName" label="机构" width="280" />
							<FaTableColumn prop="departmentName" label="部门" width="280" />
							<FaTableColumn prop="isPrimary" label="主部门" width="80" tag :enum="appStore.getDictionary('BooleanEnum')" />
							<FaTableColumn prop="positionName" label="职位" width="280" />
							<FaTableColumn prop="jobLevelName" label="职级" width="280" />
							<FaTableColumn prop="isPrincipal" label="负责人" width="80" tag :enum="appStore.getDictionary('BooleanEnum')" />
						</FaTable>
					</FaLayoutGridItem>
				</FaForm>
			</template>
		</el-scrollbar>
		<div style="margin-top: 20px; padding: 20px; display: flex; align-items: center; justify-content: center; border-top: var(--el-border)">
			<el-button type="primary" @click="changePasswordRef.open()">修改密码</el-button>
			<FaButton type="primary" @click="handleConfirm">保存</FaButton>
		</div>
	</div>
</template>

<script lang="ts" setup>
import { inject, onMounted, reactive, useTemplateRef } from "vue";
import { ElMessage, dayjs } from "element-plus";
import { type FaFormInstance, RegExps } from "fast-element-plus";
import { withDefineType } from "@fast-china/utils";
import { employeeApi } from "@/api/services/Admin/employee";
import { accountApi } from "@/api/services/Center/account";
import { fileApi } from "@/api/services/File";
import { changePasswordKey } from "@/layouts";
import { useApp, useUserInfo } from "@/stores";
import type { FormRules } from "element-plus";
import type { EditEmployeeInput } from "@/api/services/Admin/employee/models/EditEmployeeInput";
import type { EditAccountInput } from "@/api/services/Center/account/models/EditAccountInput";
import type { QueryAccountDetailOutput } from "@/api/services/Center/account/models/QueryAccountDetailOutput";

defineOptions({
	name: "SettingsAccount",
});

const appStore = useApp();
const userInfoStore = useUserInfo();

const accountFaFormRef = useTemplateRef<FaFormInstance>("accountFaFormRef");
const employeeFaFormRef = useTemplateRef<FaFormInstance>("employeeFaFormRef");
const changePasswordRef = inject(changePasswordKey);

const state = reactive({
	loading: false,
	accountFormData: withDefineType<EditAccountInput & QueryAccountDetailOutput>({}),
	employeeFormData: withDefineType<EditEmployeeInput>({}),
	formRules: withDefineType<FormRules<EditAccountInput & QueryAccountDetailOutput & EditEmployeeInput>>({
		employeeName: [{ required: true, message: "请输入职员名称", trigger: "blur" }],
		mobile: [
			{ required: true, message: "请输入手机", trigger: "blur" },
			{ pattern: RegExps.Mobile, message: "请输入正确的手机号", trigger: "blur" },
		],
		email: [
			{ required: true, message: "请输入邮箱", trigger: "blur" },
			{ pattern: RegExps.Email, message: "请输入正确的邮箱", trigger: "blur" },
			{ max: 50, message: "邮箱不能超过50位字符", trigger: "blur" },
		],
		idPhoto: [{ required: true, message: "请上传证件照", trigger: "change" }],
		entryDate: [{ required: true, message: "请选择入职日期", trigger: "change" }],
	}),
});

const handleConfirm = async (_event: MouseEvent, done: () => void) => {
	state.loading = true;
	try {
		await accountFaFormRef.value.validateScrollToField();
		await accountApi.editAccount(state.accountFormData);
		if (!userInfoStore.isSuperAdmin && !userInfoStore.isAdmin) {
			await employeeFaFormRef.value.validateScrollToField();
			await employeeApi.editSelfEmployee(state.employeeFormData);
		}
		ElMessage.success("保存成功！");
		window.location.reload();
	} finally {
		state.loading = false;
		done();
	}
};

onMounted(async () => {
	state.loading = true;
	try {
		state.accountFormData = await accountApi.queryEditAccountDetail();
		if (!userInfoStore.isSuperAdmin && !userInfoStore.isAdmin) {
			state.employeeFormData = await employeeApi.queryEmployeeDetail(userInfoStore.employeeId);
		}
	} finally {
		state.loading = false;
	}
});
</script>
