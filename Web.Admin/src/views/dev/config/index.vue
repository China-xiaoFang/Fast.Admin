<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="DEV_CONFIG" rowKey="configId" :requestApi="configApi.queryConfigPaged">
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增配置</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.configId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FastTable>
		<ConfigEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import ConfigEdit from "./edit/index.vue";
import type { FastTableInstance } from "@/components";
import type { QueryConfigPagedOutput } from "@/api/services/config/models/QueryConfigPagedOutput";
import { configApi } from "@/api/services/config";

defineOptions({
	name: "DevConfig",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof ConfigEdit>>();

/** 处理删除 */
const handleDelete = (row: QueryConfigPagedOutput) => {
	const { configId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除此配置吗？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			if (action === "confirm") {
				await configApi.deleteConfig({ configId: configId!, rowVersion: rowVersion! });
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
