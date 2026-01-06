<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="1D1KR8WZ6U"
			rowKey="positionId"
			:requestApi="positionApi.queryPositionPaged"
			hideSearchTime
			@custom-cell-click="handleCustomCellClick"
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryPositionPagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.positionId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.positionId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FastTable>
		<PositionEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import PositionEdit from "./edit/index.vue";
import type { FastTableInstance } from "@/components";
import { positionApi } from "@/api/services/position";
import { QueryPositionPagedOutput } from "@/api/services/position/models/QueryPositionPagedOutput";

defineOptions({
	name: "SystemPosition",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof PositionEdit>>();

const handleCustomCellClick = (_, { row }: { row: QueryPositionPagedOutput }) => {
	editFormRef.value.detail(row.positionId);
};

/** 处理删除 */
const handleDelete = (row: QueryPositionPagedOutput) => {
	const { positionId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除职位？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await positionApi.deletePosition({ positionId, rowVersion });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
