<template>
	<FaTable rowKey="buttonId" :data="modelValue">
		<!-- 表格按钮操作区域 -->
		<template #header>
			<el-button :disabled="props.disabled" type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
		</template>
		<FaTableColumn prop="buttonName" label="按钮名称" width="160" />
		<FaTableColumn prop="buttonCode" label="按钮编码" width="220" />
		<FaTableColumn prop="edition" label="版本" width="120" tag :enum="editionEnum" />
		<FaTableColumn
			prop="hasWeb"
			label="Web端"
			width="80"
			tag
			:enum="[
				{ label: '是', value: true, type: 'primary' },
				{ label: '否', value: false, type: 'info' },
			]"
		/>
		<FaTableColumn
			prop="hasMobile"
			label="移动端"
			width="80"
			tag
			:enum="[
				{ label: '是', value: true, type: 'primary' },
				{ label: '否', value: false, type: 'info' },
			]"
		/>
		<FaTableColumn
			prop="hasDesktop"
			label="桌面端"
			width="80"
			tag
			:enum="[
				{ label: '是', value: true, type: 'primary' },
				{ label: '否', value: false, type: 'info' },
			]"
		/>
		<FaTableColumn prop="sort" label="排序" width="80" />
		<FaTableColumn
			prop="status"
			label="状态"
			width="100"
			tag
			:enum="[
				{ label: '正常', value: 1, type: 'primary' },
				{ label: '禁用', value: 2, type: 'danger' },
			]"
		/>
		<!-- 表格操作 -->
		<template #operation="{ row, $index }: { row: EditMenuButtonInput; $index: number }">
			<el-button :disabled="props.disabled" size="small" plain type="primary" @click="editFormRef.edit(row, $index)">编辑</el-button>
			<el-button :disabled="props.disabled" size="small" plain type="danger" @click="handleDelete($index)">删除</el-button>
		</template>
	</FaTable>
	<ButtonEdit ref="editFormRef" v-model="modelValue" />
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { definePropType } from "@fast-china/utils";
import { useVModel } from "@vueuse/core";
import { useApp } from "@/stores";
import type { EditMenuButtonInput } from "@/api/services/Center/menu/models/EditMenuButtonInput";
import ButtonEdit from "./buttonEdit.vue";

defineOptions({
	name: "DevMenuEditButtonTable",
});

const props = defineProps({
	/** @description 是否禁用 */
	disabled: Boolean,
	/** @description v-model绑定值 */
	modelValue: definePropType<EditMenuButtonInput[]>([Array]),
});

const emit = defineEmits(["update:modelValue"]);

const modelValue = useVModel(props, "modelValue", emit, { passive: false });

const appStore = useApp();
const editionEnum = appStore.getDictionary("EditionEnum");

const editFormRef = ref<InstanceType<typeof ButtonEdit>>();

const handleDelete = (index: number) => {
	ElMessageBox.confirm("确定要删除该按钮？", {
		type: "warning",
		async beforeClose() {
			modelValue.value.splice(index, 1);
		},
	});
};
</script>
