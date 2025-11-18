<template>
	<div>
		<FaTable ref="faTableRef" rowKey="dictionaryId" :columns="tableColumns" :requestApi="dictionaryApi.queryDictionaryPaged" hideSearchTime>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="editFormRef.detail(row.dictionaryId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.dictionaryId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FaTable>
		<DictionaryEdit ref="editFormRef" @ok="faTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { withDefineType } from "@fast-china/utils";
import DictionaryEdit from "./edit/index.vue";
import type { QueryDictionaryPagedOutput } from "@/api/services/dictionary/models/QueryDictionaryPagedOutput";
import type { FaTableColumnCtx, FaTableInstance } from "fast-element-plus";
import { dictionaryApi } from "@/api/services/dictionary";

defineOptions({
	name: "DevDictionary",
});

const faTableRef = ref<FaTableInstance>();
const editFormRef = ref<InstanceType<typeof DictionaryEdit>>();

const handleDelete = (row: QueryDictionaryPagedOutput) => {
	const { dictionaryId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除数据字典？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await dictionaryApi.deleteDictionary({ dictionaryId, rowVersion });
			ElMessage.success("删除成功！");
			faTableRef.value?.refresh();
		},
	});
};

const tableColumns = withDefineType<FaTableColumnCtx[]>([
	{
		prop: "dictionaryKey",
		label: "字典Key",
		sortable: true,
		link: true,
		click({ row, $index }) {
			editFormRef.value.detail(row.dictionaryId);
		},
		copy: true,
		fixed: "left",
		width: 280,
	},
	{
		prop: "dictionaryName",
		label: "字典名称",
		sortable: true,
		width: 200,
	},
	{
		prop: "valueType",
		label: "值类型",
		sortable: true,
		tag: true,
		enum: [
			{ label: "字符串", value: 1, type: "info" },
			{ label: "Int", value: 2, type: "success" },
			{ label: "Long", value: 4, type: "primary" },
			{ label: "Boolean", value: 8, type: "danger" },
		],
		width: 100,
	},
	{
		prop: "hasFlags",
		label: "Flags枚举",
		sortable: true,
		tag: true,
		enum: [
			{ label: "否", value: 0, type: "danger" },
			{ label: "是", value: 1, type: "primary" },
		],
		width: 120,
	},
	{
		prop: "status",
		label: "状态",
		sortable: true,
		tag: true,
		enum: [
			{ label: "正常", value: 1, type: "primary" },
			{ label: "禁用", value: 2, type: "danger" },
		],
		width: 80,
	},
	{
		prop: "remark",
		label: "备注",
		sortable: true,
		width: 280,
	},
	{
		type: "submitInfo",
		prop: "createdTime",
		label: "创建时间",
		sortable: true,
		width: 180,
	},
	{
		type: "submitInfo",
		prop: "updatedTime",
		label: "更新时间",
		sortable: true,
		submitInfoField: {
			submitClerkName: "updatedUserName",
			submitTime: "updatedTime",
		},
		width: 180,
	},
]);
</script>
