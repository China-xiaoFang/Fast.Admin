<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HDVBM7PR8" rowKey="fileId" :requestApi="fileApi.queryFilePaged">
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="editFormRef.detail(row.dictionaryId)">预览</el-button>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { fileApi } from "@/api/services/file";

defineOptions({
	name: "SystemFile",
});
</script>
