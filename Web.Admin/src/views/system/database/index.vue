<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="1D1F3QYJST"
			rowKey="mainId"
			:requestApi="databaseApi.queryDatabasePaged"
			hideSearchTime
			@custom-cell-click="handleCustomCellClick"
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryDatabasePagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.mainId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.mainId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
				<el-button v-if="!row.isInitialized" size="small" plain type="warning" @click="handleInitDatabase(row)">初始化</el-button>
			</template>
		</FastTable>
		<DatabaseEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import DatabaseEdit from "./edit/index.vue";
import type { QueryDatabasePagedOutput } from "@/api/services/database/models/QueryDatabasePagedOutput";
import type { FastTableInstance } from "@/components";
import { databaseApi } from "@/api/services/database";
import { tenantDatabaseApi } from "@/api/services/tenantDatabase";

defineOptions({
	name: "SystemDatabase",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof DatabaseEdit>>();

const handleCustomCellClick = (_, { row }: { row: QueryDatabasePagedOutput }) => {
	editFormRef.value.detail(row.mainId);
};

/** 处理删除 */
const handleDelete = (row: QueryDatabasePagedOutput) => {
	const { mainId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除数据库？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await databaseApi.deleteDatabase({ mainId, rowVersion });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};

/** 处理初始化 */
const handleInitDatabase = (row: QueryDatabasePagedOutput) => {
	const { tenantId, databaseType, isInitialized } = row;
	if (isInitialized) return;
	ElMessageBox.confirm("确定要初始化数据库？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await tenantDatabaseApi.initDatabase({ tenantId, databaseType });
			ElMessage.success("初始化成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
