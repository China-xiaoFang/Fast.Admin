<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HD88SK728" rowKey="menuId" :requestApi="menuApi.queryMenuPaged" hideSearchTime :pagination="false">
			<!-- 表格按钮操作区域 -->
			<template #header> </template>
			<!-- 表格操作 -->
			<template #operation="{ row }"> </template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { menuApi } from "@/api/services/menu";

defineOptions({
	name: "DevMenu",
});

const fastTableRef = ref<FastTableInstance>();
</script>
