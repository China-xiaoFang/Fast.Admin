<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HZ2DHR41G" rowKey="configId" :requestApi="configApi.queryConfigPaged" hideSearchTime>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
				<el-button plain type="warning" :icon="Delete" @click="handleDelete(row)">删除全部缓存</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="editFormRef.detail(row.dictionaryId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.dictionaryId)">编辑</el-button>
				<el-button size="small" plain type="warning" @click="handleDelete(row)">删除缓存</el-button>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { Delete, Plus } from "@element-plus/icons-vue";
import type { FastTableInstance } from "@/components";
import { configApi } from "@/api/services/config";

defineOptions({
	name: "DevConfig",
});

const fastTableRef = ref<FastTableInstance>();
</script>
