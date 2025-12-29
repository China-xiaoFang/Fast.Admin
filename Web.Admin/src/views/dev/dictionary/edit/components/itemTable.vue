<template>
	<FaTable rowKey="dictionaryItemId" :columns="tableColumns" :data="modelValue">
		<!-- 表格按钮操作区域 -->
		<template #header>
			<el-button :disabled="props.disabled" type="primary" :icon="Plus" @click="editFormRef.add()">新增字典项</el-button>
		</template>
		<!-- 表格操作 -->
		<template #operation="{ row, $index }: { row: EditDictionaryItemInput; $index: number }">
			<el-button size="small" plain @click="editFormRef.detail(row)">详情</el-button>
			<el-button :disabled="props.disabled" size="small" plain type="primary" @click="editFormRef.edit(row, $index)">编辑</el-button>
			<el-button :disabled="props.disabled" size="small" plain type="danger" @click="handleDelete(row, $index)">删除</el-button>
		</template>
	</FaTable>
	<DictionaryEditItemEdit ref="editFormRef" v-model="modelValue" />
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { definePropType, withDefineType } from "@fast-china/utils";
import { useVModel } from "@vueuse/core";
import DictionaryEditItemEdit from "./itemEdit.vue";
import type { EditDictionaryItemInput } from "@/api/services/dictionary/models/EditDictionaryItemInput";
import type { FaTableColumnCtx } from "fast-element-plus";

defineOptions({
	name: "DevDictionaryEditItemTable",
});

const props = defineProps({
	/** @description 是否禁用 */
	disabled: Boolean,
	/** @description v-model绑定值 */
	modelValue: definePropType<EditDictionaryItemInput[]>([Array]),
});

const emit = defineEmits(["update:modelValue"]);

const modelValue = useVModel(props, "modelValue", emit, { passive: true });

const editFormRef = ref<InstanceType<typeof DictionaryEditItemEdit>>();

const handleDelete = (row: EditDictionaryItemInput, index: number) => {
	ElMessageBox.confirm("确定要删除数据字典项？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			modelValue.value.splice(index, 1);
		},
	});
};

const tableColumns = withDefineType<FaTableColumnCtx[]>([
	{
		prop: "label",
		label: "字典项名称",
		width: 120,
		minWidth: 100,
	},
	{
		prop: "value",
		label: "字典项值",
		width: 120,
		minWidth: 100,
	},
	{
		prop: "type",
		label: "标签类型",
		tag: true,
		enum: [
			{ label: "Primary", value: 1, type: "primary" },
			{ label: "Success", value: 2, type: "success" },
			{ label: "Info", value: 4, type: "info" },
			{ label: "Warning", value: 8, type: "warning" },
			{ label: "Danger", value: 16, type: "danger" },
		],
		width: 100,
		minWidth: 80,
	},
	{
		prop: "order",
		label: "排序",
		width: 100,
		minWidth: 80,
	},
	{
		prop: "visible",
		label: "显示",
		tag: true,
		enum: [
			{ label: "隐藏", value: 0, type: "danger" },
			{ label: "显示", value: 1, type: "primary" },
		],
		width: 100,
		minWidth: 80,
	},
	{
		prop: "status",
		label: "状态",
		tag: true,
		enum: [
			{ label: "正常", value: 1, type: "primary" },
			{ label: "禁用", value: 2, type: "danger" },
		],
		width: 100,
		minWidth: 80,
	},
	{
		prop: "tips",
		label: "提示",
		sortable: true,
		width: 120,
		minWidth: 100,
	},
]);
</script>
