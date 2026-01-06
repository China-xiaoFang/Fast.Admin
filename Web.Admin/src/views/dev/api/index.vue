<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1D11YYQH9M" rowKey="apiId" :requestApi="apiApi.queryApiPaged" hideSearchTime>
			<template #tags="{ row }: { row?: ApiInfoModel }">
				<el-tag v-for="(item, index) in row.tags" :key="index" type="info" effect="dark">{{ item }}</el-tag>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import type { ApiInfoModel } from "@/api/services/api/models/ApiInfoModel";
import { apiApi } from "@/api/services/api";

defineOptions({
	name: "DevApi",
});
</script>
