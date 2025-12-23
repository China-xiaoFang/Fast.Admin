<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="LOG_OPERATION" rowKey="operationLogId" :requestApi="operationLogApi.queryOperationLogPaged">
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { operationLogApi } from "@/api/services/operationLog";

defineOptions({
	name: "LogOperation",
});

const fastTableRef = ref<FastTableInstance>();
</script>
