<template>
	<div>
		<FaTable
			v-show="!state.isEdit"
			ref="faTableRef"
			rowKey="tableId"
			:columns="tableColumns"
			:requestApi="tableApi.queryTableConfigPaged"
			hideSearchTime
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="editFormRef.detail(row.tableId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.tableId)">编辑</el-button>
				<el-button size="small" plain type="info" @click="editFormRef.copy(row.tableId)">复制</el-button>
				<el-button size="small" plain type="success" @click="handleColumnConfigClick(row)">配置列</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FaTable>
		<TableColumnConfig v-show="state.isEdit" ref="tableColumnConfigRef" @back="handleBack" />
		<TableConfigEdit ref="editFormRef" @ok="faTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { withDefineType } from "@fast-china/utils";
import TableColumnConfig from "./config/index.vue";
import TableConfigEdit from "./edit/index.vue";
import type { QueryTableConfigPagedOutput } from "@/api/services/table/models/QueryTableConfigPagedOutput";
import type { FaTableColumnCtx, FaTableInstance } from "fast-element-plus";
import { tableApi } from "@/api/services/table";

defineOptions({
	name: "DevTableConfig",
});

const faTableRef = ref<FaTableInstance>();
const editFormRef = ref<InstanceType<typeof TableConfigEdit>>();
const tableColumnConfigRef = ref<InstanceType<typeof TableColumnConfig>>();

const state = reactive({
	/** 是否编辑 */
	isEdit: false,
});

/** 处理配置列点击 */
const handleColumnConfigClick = (row: QueryTableConfigPagedOutput) => {
	tableColumnConfigRef.value.edit(row.tableId, row.tableName, row.rowVersion);
	state.isEdit = true;
};

/** 处理返回 */
const handleBack = () => {
	state.isEdit = false;
};

/** 处理删除 */
const handleDelete = (row: QueryTableConfigPagedOutput) => {
	const { tableId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除表格配置？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await tableApi.deleteTableConfig({ tableId, rowVersion });
			ElMessage.success("删除成功！");
			faTableRef.value?.refresh();
		},
	});
};

const tableColumns = withDefineType<FaTableColumnCtx[]>([
	{
		prop: "tableKey",
		label: "表格Key",
		sortable: true,
		link: true,
		click({ row, $index }) {
			editFormRef.value.detail(row.tableId);
		},
		copy: true,
		fixed: "left",
		width: 280,
	},
	{
		prop: "tableName",
		label: "表格名称",
		sortable: true,
		width: 280,
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
