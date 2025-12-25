<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HZQY5LNNZ" rowKey="mainId" :requestApi="databaseApi.queryDatabasePaged" hideSearchTime>
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
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { Plus } from "@element-plus/icons-vue";
import type { FastTableInstance } from "@/components";
import { databaseApi } from "@/api/services/database";

defineOptions({
	name: "DevApi",
});

const fastTableRef = ref<FastTableInstance>();
</script>
