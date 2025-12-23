<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="LOG_EXCEPTION" rowKey="exceptionLogId" :requestApi="exceptionLogApi.queryExceptionLogPaged">
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { exceptionLogApi } from "@/api/services/exceptionLog";

defineOptions({
	name: "LogException",
});

const fastTableRef = ref<FastTableInstance>();
</script>
