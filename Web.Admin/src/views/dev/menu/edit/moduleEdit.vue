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
			<FaFormItem prop="appId" label="应用">
				<ApplicationSelect v-model="state.formData.appId" v-model:appName="state.formData.appName" />
			</FaFormItem>
			<FaFormItem prop="moduleName" label="模块名称">
				<el-input v-model="state.formData.moduleName" maxlength="20" placeholder="请输入模块名称" />
			</FaFormItem>
			<FaFormItem prop="icon" label="图标">
				<el-cascader
					v-model="state.formData.icon"
					:options="state.iconList"
					placeholder="请选择Web端图标"
					:showAllLevels="false"
					filterable
					clearable
					:props="{ emitPath: false }"
				>
					<template #prefix>
						<FaIcon v-if="state.formData.icon" size="16" :name="state.formData.icon" />
					</template>
					<template #default="{ node, data }: { node: CascaderNode; data: ElSelectorOutput<string> }">
						<div style="display: flex; align-items: center; gap: 3px">
							<FaIcon v-if="node.isLeaf" size="16" :name="data.value" />
							<span>{{ data.label }}</span>
						</div>
					</template>
				</el-cascader>
			</FaFormItem>
			<FaFormItem prop="viewType" label="颜色">
				<el-color-picker style="width: 32px" v-model="state.formData.color" :predefine="state.predefineColorList" />
			</FaFormItem>
			<FaFormItem prop="viewType" label="查看类型">
				<RadioGroup name="ModuleViewTypeEnum" v-model="state.formData.viewType" />
			</FaFormItem>
			<FaFormItem prop="sort" label="排序" tips="从小到大">
				<el-input-number v-model="state.formData.sort" :min="1" :max="9999" placeholder="请输入排序" />
			</FaFormItem>
			<FaFormItem v-if="state.dialogState !== 'add'" prop="status" label="状态">
				<RadioGroup button name="CommonStatusEnum" v-model="state.formData.status" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { CascaderNode, ElMessage, type FormRules } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import type { ElSelectorOutput, FaDialogInstance, FaFormInstance } from "fast-element-plus";
import * as ElementPlusIconsVue from "@element-plus/icons-vue";
import * as FastElementPlusIconsVue from "@fast-element-plus/icons-vue";
import { EditModuleInput } from "@/api/services/module/models/EditModuleInput";
import { AddModuleInput } from "@/api/services/module/models/AddModuleInput";
import { ModuleViewTypeEnum } from "@/api/enums/ModuleViewTypeEnum";
import { moduleApi } from "@/api/services/module";
import { defaultThemeColor } from "@/stores";

defineOptions({
	name: "DevModuleEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditModuleInput & AddModuleInput & { appName?: string }>({
		viewType: ModuleViewTypeEnum.All,
	}),
	formRules: withDefineType<FormRules>({
		appId: [{ required: true, message: "请选择应用", trigger: "change" }],
		viewType: [{ required: true, message: "请选择查看类型", trigger: "change" }],
		moduleName: [{ required: true, message: "请输入模块名称", trigger: "blur" }],
		sort: [{ required: true, message: "请输入排序", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("add"),
	dialogTitle: "模块",
	iconList: withDefineType<ElSelectorOutput<string>[]>([
		{
			value: "el-icon",
			label: "el-icon",
			children: Object.keys(ElementPlusIconsVue).map((item) => ({
				value: `el-icon-${item}`,
				label: item,
			})),
		},
		{
			value: "fa-icon",
			label: "fa-icon",
			children: Object.keys(FastElementPlusIconsVue)
				.filter((f) => f !== "default")
				.map((item) => ({
					value: `fa-icon-${item}`,
					label: item,
				})),
		},
	]),
	predefineColorList: [
		defaultThemeColor,
		"#4488FE",
		"#2A3A93",
		"#EB4537", // 红色
		"#FF8225", // 橙色
		"#FAC230", // 黄色
		"#55AF7B", // 绿色
		"#FF4191", // 粉色
		"#4286F3", // 蓝色
		"#AD6DEF", // 紫色
	],
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await moduleApi.addModule(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await moduleApi.editModule(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加模块";
		state.formDisabled = false;
	});
};

const edit = (moduleId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await moduleApi.queryModuleDetail(moduleId);
		state.formData = apiRes;
		state.dialogTitle = `编辑模块 - ${apiRes.moduleName}`;
	});
};

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	add,
	edit,
});
</script>
