<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="1D1KRB5KK9"
			rowKey="jobLevelId"
			:requestApi="jobLevelApi.queryJobLevelPaged"
			hideSearchTime
			@custom-cell-click="handleCustomCellClick"
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryJobLevelPagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.jobLevelId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.jobLevelId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FastTable>
		<JobLevelEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import JobLevelEdit from "./edit/index.vue";
import type { FastTableInstance } from "@/components";
import { jobLevelApi } from "@/api/services/jobLevel";
import { QueryJobLevelPagedOutput } from "@/api/services/jobLevel/models/QueryJobLevelPagedOutput";

defineOptions({
	name: "SystemJobLevel",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof JobLevelEdit>>();

const handleCustomCellClick = (_, { row }: { row: QueryJobLevelPagedOutput }) => {
	editFormRef.value.detail(row.jobLevelId);
};

/** 处理删除 */
const handleDelete = (row: QueryJobLevelPagedOutput) => {
	const { jobLevelId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除职级？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await jobLevelApi.deleteJobLevel({ jobLevelId, rowVersion });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
