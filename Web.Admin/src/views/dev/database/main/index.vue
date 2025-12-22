<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="DEV_MAIN_DATABASE" rowKey="mainDatabaseId" :requestApi="databaseApi.queryMainDatabasePaged">
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增主库模板</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.mainDatabaseId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FastTable>
		<MainDatabaseEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import MainDatabaseEdit from "./edit/index.vue";
import type { QueryMainDatabasePagedOutput } from "@/api/services/database/models/QueryMainDatabasePagedOutput";
import type { FastTableInstance } from "@/components";
import { databaseApi } from "@/api/services/database";

defineOptions({
	name: "DevMainDatabase",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof MainDatabaseEdit>>();

/** 处理删除 */
const handleDelete = (row: QueryMainDatabasePagedOutput) => {
	const { mainDatabaseId, rowVersion } = row;
	if (!mainDatabaseId || !rowVersion) {
		ElMessage.error("数据错误，无法删除");
		return;
	}
	ElMessageBox.confirm("确定要删除此主库模板吗？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			if (action === "confirm") {
				await databaseApi.deleteMainDatabase({ mainDatabaseId, rowVersion });
				ElMessage.success("删除成功！");
				fastTableRef.value?.refresh();
				done();
			} else {
				done();
			}
		},
	});
};
</script>
