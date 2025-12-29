<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="1HZ2DHR41G"
			rowKey="configId"
			:requestApi="configApi.queryConfigPaged"
			hideSearchTime
			@custom-cell-click="(_, { row }: { row: QueryConfigPagedOutput }) => editFormRef.detail(row.configId)"
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
				<el-button plain type="warning" :icon="Delete" @click="handleDeleteAll">删除全部缓存</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryConfigPagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.configId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.configId)">编辑</el-button>
				<el-button size="small" plain type="warning" @click="handleDelete(row)">删除缓存</el-button>
			</template>
		</FastTable>
		<ConfigEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Delete, Plus } from "@element-plus/icons-vue";
import ConfigEdit from "./edit/index.vue";
import type { QueryConfigPagedOutput } from "@/api/services/config/models/QueryConfigPagedOutput";
import type { FastTableInstance } from "@/components";
import { configApi } from "@/api/services/config";

defineOptions({
	name: "DevConfig",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof ConfigEdit>>();

/** 处理删除缓存 */
const handleDelete = (row: QueryConfigPagedOutput) => {
	const { configCode } = row;
	ElMessageBox.confirm("确定要删除缓存？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await configApi.deleteConfigCache({ configCode });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};

/** 处理删除全部缓存 */
const handleDeleteAll = () => {
	ElMessageBox.confirm("确定要删除全部缓存？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await configApi.deleteAllConfigCache();
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
