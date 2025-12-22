<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="DEV_SLAVE_DATABASE" rowKey="slaveDatabaseId" :requestApi="databaseApi.querySlaveDatabasePaged">
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增从库模板</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.slaveDatabaseId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FastTable>
		<SlaveDatabaseEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import SlaveDatabaseEdit from "./edit/index.vue";
import type { QuerySlaveDatabasePagedOutput } from "@/api/services/database/models/QuerySlaveDatabasePagedOutput";
import type { FastTableInstance } from "@/components";
import { databaseApi } from "@/api/services/database";

defineOptions({
	name: "DevSlaveDatabase",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof SlaveDatabaseEdit>>();

/** 处理删除 */
const handleDelete = (row: QuerySlaveDatabasePagedOutput) => {
	const { slaveDatabaseId, rowVersion } = row;
	if (!slaveDatabaseId || !rowVersion) {
		ElMessage.error("数据错误，无法删除");
		return;
	}
	ElMessageBox.confirm("确定要删除此从库模板吗？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			if (action === "confirm") {
				await databaseApi.deleteSlaveDatabase({ slaveDatabaseId, rowVersion });
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
