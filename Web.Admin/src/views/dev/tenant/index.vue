<template>
	<div>
		<!-- @custom-cell-click="(_, { row }) => editFormRef.detail(row.dictionaryId)" -->
		<FastTable ref="fastTableRef" tableKey="127GKZPG4L" rowKey="127GKZPG4L" :requestApi="tenantApi.queryTenantPaged">
			<!-- 表格按钮操作区域 -->
			<template #header>
				<!-- <el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button> -->
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<!-- <el-button size="small" plain @click="editFormRef.detail(row.dictionaryId)">详情</el-button> -->
				<!-- <el-button size="small" plain type="primary" @click="editFormRef.edit(row.dictionaryId)">编辑</el-button> -->
				<!-- <el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button> -->
			</template>
		</FastTable>
		<!-- <DictionaryEdit ref="editFormRef" @ok="faTableRef.refresh()" /> -->
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
// import { Plus } from "@element-plus/icons-vue";
// import TenantEdit from "./edit/index.vue";
import type { FastTableInstance } from "@/components";
import type { FaTableColumnCtx, FaTableInstance } from "fast-element-plus";
import { dictionaryApi } from "@/api/services/dictionary";
import { tenantApi } from "@/api/services/tenant";

defineOptions({
	name: "DevTenant",
});

const fastTableRef = ref<FastTableInstance>();
// const editFormRef = ref<InstanceType<typeof TenantEdit>>();

// /** 处理删除 */
// const handleDelete = (row: QueryDictionaryPagedOutput) => {
// 	const { dictionaryId, rowVersion } = row;
// 	ElMessageBox.confirm("确定要删除数据字典？", {
// 		type: "warning",
// 		async beforeClose(action, instance, done) {
// 			await dictionaryApi.deleteDictionary({ dictionaryId, rowVersion });
// 			ElMessage.success("删除成功！");
// 			faTableRef.value?.refresh();
// 		},
// 	});
// };
</script>
