<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HZ9ZTWD4S" rowKey="apiId" :requestApi="apiApi.queryApiPaged" hideSearchTime>
			<template #tags="{ row }">
				<el-tag v-for="(item, index) in row.tags" :key="index" type="info" effect="dark">{{ item }}</el-tag>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { apiApi } from "@/api/services/api";

defineOptions({
	name: "DevApi",
});
</script>
