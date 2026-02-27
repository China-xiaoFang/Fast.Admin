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
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules" :disabled="state.formDisabled" cols="2">
			<FaFormItem prop="buttonName" label="按钮名称">
				<el-input v-model="state.formData.buttonName" maxlength="20" placeholder="请输入按钮名称" />
			</FaFormItem>
			<FaFormItem prop="buttonCode" label="按钮编码">
				<el-input v-model="state.formData.buttonCode" maxlength="50" placeholder="请输入按钮编码" />
			</FaFormItem>
			<FaFormItem prop="edition" label="版本" span="2">
				<FaSelect :data="editionEnum" v-model="state.formData.edition" />
			</FaFormItem>
			<FaFormItem prop="hasWeb" label="Web端">
				<el-checkbox v-model="state.formData.hasWeb" />
			</FaFormItem>
			<FaFormItem prop="hasMobile" label="移动端">
				<el-checkbox v-model="state.formData.hasMobile" />
			</FaFormItem>
			<FaFormItem prop="hasDesktop" label="桌面端">
				<el-checkbox v-model="state.formData.hasDesktop" />
			</FaFormItem>
			<FaFormItem prop="sort" label="排序" tips="从小到大">
				<el-input-number v-model="state.formData.sort" :min="1" :max="9999" placeholder="请输入排序" />
			</FaFormItem>
			<FaFormItem prop="status" label="状态" span="2">
				<RadioGroup button name="CommonStatusEnum" v-model="state.formData.status" />
			</FaFormItem>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { type FormRules } from "element-plus";
import { definePropType, withDefineType } from "@fast-china/utils";
import { useVModel } from "@vueuse/core";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import type { EditMenuButtonInput } from "@/api/services/Center/menu/models/EditMenuButtonInput";
import { useApp } from "@/stores";
import type { FaDialogInstance, FaFormInstance } from "fast-element-plus";

defineOptions({
	name: "DevMenuEditButtonEdit",
});

const props = defineProps({
	/** @description v-model绑定值 */
	modelValue: definePropType<EditMenuButtonInput[]>([Array]),
});

const emit = defineEmits(["update:modelValue"]);

const modelValue = useVModel(props, "modelValue", emit, { passive: false });

const appStore = useApp();
const editionEnum = appStore.getDictionary("EditionEnum");

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditMenuButtonInput>({}),
	formRules: withDefineType<FormRules>({
		buttonName: [{ required: true, message: "请输入按钮名称", trigger: "blur" }],
		buttonCode: [{ required: true, message: "请输入按钮编码", trigger: "blur" }],
		edition: [{ required: true, message: "请选择版本", trigger: "change" }],
		sort: [{ required: true, message: "请输入排序", trigger: "blur" }],
		status: [{ required: true, message: "请选择状态", trigger: "change" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
	dialogTitle: "按钮",
	tableIndex: withDefineType<number>(),
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				modelValue.value.push({ ...state.formData });
				break;
			case "edit":
				modelValue.value[state.tableIndex] = { ...state.formData };
				break;
		}
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加按钮";
		state.formDisabled = false;
		state.formData = {
			edition: EditionEnum.None,
			hasWeb: true,
			hasMobile: false,
			hasDesktop: false,
			sort: 1,
			status: CommonStatusEnum.Enable,
		};
	});
};

const edit = (row: EditMenuButtonInput, index: number) => {
	faDialogRef.value.open(() => {
		state.dialogState = "edit";
		state.formDisabled = false;
		state.tableIndex = index;
		state.formData = { ...row };
		state.dialogTitle = `编辑按钮 - ${row.buttonName}`;
	});
};

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	add,
	edit,
});
</script>
